using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GripDev.PowerGist.Addin.ISEInterop
{
    public class SaveAllItems
    {
        private ObjectModelRoot host;
        public SaveAllItems()
        {
            host = Config.ObjectModelRoot;
            if (host == null)
            {
                throw new ArgumentNullException("ObjectModelRoot not set in config");
            }
        }

        public void Invoke()
        {
			if (host.CurrentPowerShellTab.Files.Any(x => !x.IsSaved))
            {
				MessageBox.Show("You have unsaved files open, these will be ignored");
			}

            foreach (var f in host.CurrentPowerShellTab.Files.Where(x=>x.IsSaved))
            {
                f.Save();
            }


        }
    }
}
