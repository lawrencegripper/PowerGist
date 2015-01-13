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

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }


        //ctor for design time 
        public MainViewModel()
        {
            myGists = new ObservableCollection<GistObject>()
            {
                new GistObject
                {
                    id = "something", description = "something here"
                }
            };
        }

        public async Task Load()
        {
            var myGistsTemp = await repo.GetUsers();
            MyGists = new ObservableCollection<GistObject>(myGistsTemp);

            StarredGists = new ObservableCollection<GistObject>(await repo.GetStarred());

            LoadScript = new DelegateCommand<GistObject>(async q =>
            {
                foreach(var file in q.files)
                {
                    var CreateNewFile = new ISEInterop.CreateNewFile();
                    var content = await repo.GetFileContentByUri(file.raw_url);
                    CreateNewFile.Invoke(file.filename, content); 
                }
            });
        }

        private ICommand loadScript;

        public ICommand LoadScript
        {
            get { return loadScript; }
            set { loadScript = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<GistObject> myGists;

        public ObservableCollection<GistObject> MyGists
        {
            get { return myGists; }
            set { myGists = value; NotifyPropertyChanged();  }
        }

        private ObservableCollection<GistObject> starredGists;

        public ObservableCollection<GistObject> StarredGists
        {
            get { return starredGists; }
            set { starredGists = value; NotifyPropertyChanged(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
