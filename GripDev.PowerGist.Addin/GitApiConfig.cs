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
        public static string ClientId = "1eb530bea98d9f863c57";
        public static string ClientSecret = "1bd1d3ae6d18c42339e4f98f7e705ef63de7bfdc";

        internal static void AlertIfConfigRequired()
        {
            if (ClientSecret == null)
            {
                MessageBox.Show("Configure Client Secret");
            }
        }
    }
}
