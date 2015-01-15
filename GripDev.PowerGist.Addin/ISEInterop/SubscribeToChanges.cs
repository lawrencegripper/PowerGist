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
        public SubscribeToChanges(Action Callback, ISEFile file, string propertyName)
        {
            host = Config.ObjectModelRoot;
            if (host == null)
            {
                throw new ArgumentNullException("ObjectModelRoot not set in config");
            }

            callbackAction = Callback;
            propertyNameFilter = propertyName;
            file.Editor.PropertyChanged += Editor_PropertyChanged;
        }

        private void Editor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyNameFilter)
            {
                callbackAction.Invoke();
            }
        }
    }
}

