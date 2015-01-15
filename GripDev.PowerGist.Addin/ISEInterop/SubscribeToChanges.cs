using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripDev.PowerGist.Addin.ISEInterop
{
    public class SubscribeToChanges
    {
        private ObjectModelRoot host;
        private Action callbackAction;
        private string propertyNameFilter;
        public SubscribeToChanges(string filePath, Action Callback)
        {
            host = Config.ObjectModelRoot;
            if (host == null)
            {
                throw new ArgumentNullException("ObjectModelRoot not set in config");
            }

            callbackAction = Callback;
            System.IO.FileSystemWatcher watcher = new System.IO.FileSystemWatcher(filePath);
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            callbackAction.Invoke();
        }
    }
}

