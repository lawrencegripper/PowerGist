using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GripDev.PowerGist.Addin
{
    class GitApiConfig
    {
        private const string unconfiguredConstant = "ReplaceMe";
        static GitApiConfig()
        {

        }

        public static void AlertIfConfigRequired()
        {
            if (ClientId == unconfiguredConstant)
            {
                MessageBox.Show("ClientID and Secret require configuration, see GitApiConfig class");
            }
        }

        public static string ClientId = unconfiguredConstant;
        public static string ClientSecret = unconfiguredConstant;

    }
}
