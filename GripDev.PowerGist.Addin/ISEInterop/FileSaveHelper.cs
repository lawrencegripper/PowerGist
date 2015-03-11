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

			if (FileNotStoredLocally(filePath))
            {
				//create the directory if needed
                CreateDirIfNeeded(directoryPath);
            }
			else if (HasLocalchanges(content, filePath) && !OverwriteLocalChanges(filePath))
			{
				//No override wanted so we'll just return the file
				return filePath;
			}

			//Save the file locally and return the path
			return SaveFileLocally(newFile, filePath);
		}





        private static void CreateDirIfNeeded(string directoryPath)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }
        }


        private static bool FileNotStoredLocally(string filePath)
        {
            return !System.IO.File.Exists(filePath);
        }

        private static bool HasLocalchanges(string content, string filePath)
        {
            var existing = System.IO.File.ReadAllText(filePath).Replace("\r\n", "\n");
            return !(existing == content);
        }

        private static bool OverwriteLocalChanges(string filePath)
        {
            return MessageBox.Show("File already exists with changes locally, do you want to overwrite? (if not I'll load the local file) Path: " + filePath, "Overwrite?", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        private static string SaveFileLocally(ISEFile newFile, string filePath)
        {
            newFile.SaveAs(filePath);
            return filePath;
        }
    }
}
