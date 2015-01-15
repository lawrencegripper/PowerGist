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

        public string FilePath { get; private set; }
        public string DirPath { get; private set; }


        public ISEFile Invoke(string name, string id, string content)
        {
            var newFile = CreateNewFileInISE();

            newFile.Editor.InsertText(content);
            newFile.Editor.SetCaretPosition(1, 1);

            DirPath = GetDirPath(id);

            FilePath = FileSaveHelper.SaveFile(DirPath, name, id, content, newFile);

            return newFile;
        }

        public static string GetDirPath(string id)
        {
            return System.IO.Path.Combine(Config.PowerGistFolder, id);
        }

        private string GetFilePath(string name, string directoryPath)
        {
            return System.IO.Path.Combine(directoryPath, name);
        }

        private ISEFile CreateNewFileInISE()
        {
            var newFile = host.CurrentPowerShellTab.Files.Add();
            host.CurrentPowerShellTab.Files.SelectedFile = newFile;
            return newFile;
        }
    }
}
