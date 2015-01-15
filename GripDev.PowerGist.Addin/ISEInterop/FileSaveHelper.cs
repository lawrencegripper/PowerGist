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
        public static string SaveFile(string directoryPath, string name, string id, string content, ISEFile newFile)
        {
            string filePath = System.IO.Path.Combine(directoryPath, name);

            if (FileNoStoredLocally(filePath))
            {
                CreateDirIfNeeded(directoryPath);
                return SaveFileLocally(newFile, filePath);
            }

            if (NoLocalchanges(content, filePath) || OverwriteLocalChanges(filePath))
            {
                return SaveFileLocally(newFile, filePath);
            }

            return null;
            //if (OverwriteLocalChanges(filePath))
            //{
            //    return SaveFileLocally(newFile, filePath);
            //}
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
            var existing = System.IO.File.ReadAllText(filePath).Replace("\r\n", "\n");
            return existing == content;
        }

        private static bool OverwriteLocalChanges(string filePath)
        {
            return MessageBox.Show("File already exists with changes locally, do you want to overwrite? (if not I'll leave it unsaved) Path: " + filePath, "Overwrite?", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        private static string SaveFileLocally(ISEFile newFile, string filePath)
        {
            newFile.SaveAs(filePath);
            return filePath;
        }
    }
}
