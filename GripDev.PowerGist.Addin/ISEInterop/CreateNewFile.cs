using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GripDev.PowerGist.Addin.ISEInterop
{
    public class CreateNewFile
    {
        private ObjectModelRoot host;
        public CreateNewFile()
        {
            host = Config.ObjectModelRoot;
            if (host == null)
            {
                throw new ArgumentNullException("ObjectModelRoot not set in config");
            }
        }

        public void Invoke(string name, string id, string content)
        {
            var newFile = CreateNewFileInISE();

            newFile.Editor.InsertText(content);
            newFile.Editor.SetCaretPosition(1, 1);

            var directoryPath = string.Format("{0}\\{1}\\{2}", Config.PowerGistFolder, id, name);
            var filePath = System.IO.Path.Combine(directoryPath, name);

            if (FileNoStoredLocally(filePath))
            {
                CreateDirIfNeeded(directoryPath);
                SaveFileLocally(newFile, filePath);
                return;
            }

            if (NoLocalchanges(content, filePath))
            {
                SaveFileLocally(newFile, filePath);
                return;
            }

            if (OverwriteLocalChanges(filePath))
            {
                SaveFileLocally(newFile, filePath);
                return;
            }

        }

        private static void CreateDirIfNeeded(string directoryPath)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }
        }

        private ISEFile CreateNewFileInISE()
        {
            var newFile = host.CurrentPowerShellTab.Files.Add();
            host.CurrentPowerShellTab.Files.SelectedFile = newFile;
            return newFile;
        }

        private static bool FileNoStoredLocally(string filePath)
        {
            return !System.IO.File.Exists(filePath);
        }

        private static bool NoLocalchanges(string content, string filePath)
        {
            return System.IO.File.ReadAllText(filePath) == content;
        }

        private static bool OverwriteLocalChanges(string filePath)
        {
            return MessageBox.Show("File already exists with changes locally, do you want to overwrite? (if not I'll leave it unsaved) Path: " + filePath, "Overwrite?", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        private static void SaveFileLocally(ISEFile newFile, string filePath)
        {
            newFile.SaveAs(filePath);
        }
    }
}
