using GistsApi;
using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GripDev.PowerGist.Addin
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PowerGistPanel : IAddOnToolHostObject
    {
        private GistClient gistClient;
        private MainViewModel viewModel;
        public ObjectModelRoot HostObject { get; set; }

        public PowerGistPanel()
        {
            InitializeComponent();

            gistClient = new GistClient("1eb530bea98d9f863c57", "1e55daaec72d64581f8688e7bbb3e779c83b3262", "powershellISEAddin");
            viewModel = new MainViewModel();
            viewModel.Loading = Visibility.Collapsed;

            //navigate to "https://github.com/login/oauth/authorize" 
            webBrowser.Visibility = Visibility.Visible;
            webBrowser.Navigate(gistClient.AuthorizeUrl);
        }

        private async void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri.AbsoluteUri.Contains("code="))
            {
                e.Cancel = true;
                webBrowser.Visibility = Visibility.Collapsed;

                var authCode = Regex.Split(e.Uri.AbsoluteUri, "code=")[1];

                //get access token
                await gistClient.Authorize(authCode);
                ISEInterop.Config.ObjectModelRoot = HostObject;
                var gistRepo = new GistRepository(gistClient);
                viewModel = new MainViewModel(gistRepo);
                this.DataContext = viewModel;
                await viewModel.Load();
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems == null || e.AddedItems.Count < 1)
            {
                return;
            }

            viewModel.LoadGist.Execute(e.AddedItems[0]);
        }
    }
}
