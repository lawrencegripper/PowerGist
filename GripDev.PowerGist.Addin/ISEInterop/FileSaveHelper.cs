using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GripDev.PowerGist.Addin.ISEInterop
{
    class FileSaveHelper
    {
        public static void SaveFile(string name, string id, string content, ISEFile newFile)
        {
            string directoryPath = GetDirPath(name, id);
            string filePath = GetFilePath(name, directoryPath);

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

        private static string GetFilePath(string name, string directoryPath)
        {
            return System.IO.Path.Combine(directoryPath, name);
        }

        private static string GetDirPath(string name, string id)
        {
            return string.Format("{0}\\{1}\\{2}", Config.PowerGistFolder, id, name);
        }

        private static void CreateDirIfNeeded(string directoryPath)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }
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
