using GistsApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GripDev.PowerGist.Addin
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private GistRepository repo;
        public MainViewModel(GistRepository repo)
        {
            this.repo = repo;
        }

        public async Task Load()
        {
            myGists = new ObservableCollection<GistObject>(await repo.GetMyGists());
            starredGists = new ObservableCollection<GistObject>(await repo.GetStarredGists());

            ExecuteScript = new DelegateCommand<string>(x =>
            {
                
            });
        }

        public ICommand ExecuteScript { get; set; }

        private ObservableCollection<GistObject> myGists;

        public ObservableCollection<GistObject> MyGists
        {
            get { return myGists; }
            set { myGists = value; }
        }

        private ObservableCollection<GistObject> starredGists;

        public ObservableCollection<GistObject> StarredGists
        {
            get { return starredGists; }
            set { starredGists = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
