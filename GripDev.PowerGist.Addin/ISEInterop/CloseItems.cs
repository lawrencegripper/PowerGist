using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GripDev.PowerGist.Addin.ISEInterop
{

    public class CloseItems
    {
        private ObjectModelRoot host;
        public CloseItems()
        {
            host = Config.ObjectModelRoot;
            if (host == null)
            {
                throw new ArgumentNullException("ObjectModelRoot not set in config");
            }
        }
        public void Invoke()
        {
            if (!ProceedSaveAndClose())
            {
                return;
            }

            SaveAndCloseFiles();
        }

        private void SaveAndCloseFiles()
        {
            foreach (var f in host.CurrentPowerShellTab.Files)
            {
                MessageBox.Show("//Todo - updateGist");
                f.Save();
                host.CurrentPowerShellTab.Files.Remove(f);
            }
        }

        private static bool ProceedSaveAndClose()
        {
            return MessageBox.Show("Save & Close current files then open new gist?", "Overwrite?", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
