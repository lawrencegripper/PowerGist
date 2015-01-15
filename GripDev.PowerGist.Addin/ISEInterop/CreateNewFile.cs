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

        private ISEFile CreateNewFileInISE()
        {
            var newFile = host.CurrentPowerShellTab.Files.Add();
            host.CurrentPowerShellTab.Files.SelectedFile = newFile;
            return newFile;
        }


        public void Invoke(string name, string id, string content)
        {
            var newFile = CreateNewFileInISE();

            newFile.Editor.InsertText(content);
            newFile.Editor.SetCaretPosition(1, 1);

            FileSaveHelper.SaveFile(name, id, content, newFile);

        }
    }
}
