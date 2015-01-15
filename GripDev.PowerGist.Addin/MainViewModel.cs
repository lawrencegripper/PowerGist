using GistsApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            Loading = Visibility.Visible;
        }

        public async Task Load()
        {
            Loading = Visibility.Visible;
            var myGistsTemp = await repo.GetUsers();
            MyGists = new ObservableCollection<GistObject>(myGistsTemp);

            StarredGists = new ObservableCollection<GistObject>(await repo.GetStarred());

            Loading = Visibility.Collapsed;

            LoadScript = new DelegateCommand<GistObject>(async selectGist =>
            {
                if (CurrentGist != null)
                {
                    var closeItems = new ISEInterop.CloseItems();
                    closeItems.Invoke();
                }

                CurrentGist = selectGist;

                foreach(var file in selectGist.files)
                {
                    var CreateNewFile = new ISEInterop.CreateNewFile();
                    var content = await repo.GetFileContentByUri(file.raw_url);
                    var iseFile = CreateNewFile.Invoke(file.filename, selectGist.id, content); 

                    //var subscribe = new ISEInterop.SubscribeToChanges()
                }
            });

            CloseGist = new DelegateCommand(() =>
            {
                if (CurrentGist == null)
                {
                    return;
                }

                var closeItems = new ISEInterop.CloseItems();
                closeItems.Invoke();
            });

            SaveGist = new DelegateCommand(async () =>
            {
                var saveAll = new ISEInterop.SaveAllItems();
                saveAll.Invoke();

                foreach(var file in CurrentGist.files)
                {
                    var contentFromEditor = new ISEInterop.GetCurrentFileContent().Invoke(file.filename);
                    if (contentFromEditor == null)
                    {
                        continue;
                    }
                    await repo.Update(CurrentGist, file.filename, contentFromEditor);

                    PendingChangesInCurrentGist = false;
                }
            });
        }

        private ICommand loadScript;

        public ICommand LoadScript
        {
            get { return loadScript; }
            set { loadScript = value; NotifyPropertyChanged(); }
        }

        private ICommand closeGist;

        public ICommand CloseGist
        {
            get { return closeGist; }
            set { closeGist = value; NotifyPropertyChanged(); }
        }

        private ICommand saveGist;

        public ICommand SaveGist
        {
            get { return saveGist; }
            set { saveGist = value; NotifyPropertyChanged(); }
        }



        private GistObject currentGist;

        public GistObject CurrentGist
        {
            get { return currentGist; }
            set {
                currentGist = value;
                NotifyPropertyChanged();
                if (value == null)
                {
                    PendingChangesInCurrentGist = false;
                }
                //update possible files
                CurrentGistFiles = new ObservableCollection<File>(currentGist.files);
            }
        }

        private bool pendingChangesInCurrentGist;

        public bool PendingChangesInCurrentGist
        {
            get { return pendingChangesInCurrentGist; }
            set { pendingChangesInCurrentGist = value; NotifyPropertyChanged(); }
        }


        private File currentFile;

        public File CurrentFile
        {
            get { return currentFile; }
            set { currentFile = value; NotifyPropertyChanged(); }
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

        private ObservableCollection<File> currentGistFiles;

        public ObservableCollection<File> CurrentGistFiles
        {
            get { return currentGistFiles; }
            set { currentGistFiles = value; NotifyPropertyChanged(); }
        }


        private Visibility loading;
        public Visibility Loading
        {
            get
            {
                return loading;
            }
            set
            {
                loading = value;
                NotifyPropertyChanged();
            }
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
