using GistsApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public MainViewModel(GistRepository repo) : this()
        {
            this.repo = repo;
            AddFileName = "newFile.ps1";
        }
 
        public MainViewModel()
        {
            Loading = Visibility.Visible;
            PendingChangesInCurrentGist = false;
            SetupCommands();
        }

        public async Task Load()
        {
            Loading = Visibility.Visible;
            MyGists = new ObservableCollection<GistObject>(await repo.GetUsers());

            //StarredGists = new ObservableCollection<GistObject>(await repo.GetStarred());

            Loading = Visibility.Collapsed;

        }

        private void SetupCommands()
        {
            LoadGist = new DelegateCommand<GistObject>(async selectGist =>
            {
                if (CurrentGist != null)
                {
                    var closeItems = new ISEInterop.CloseItems();
                    closeItems.Invoke();
                }

                CurrentGist = selectGist;

                foreach (var file in selectGist.files)
                {
					var content = await repo.GetFileContentByUri(file.raw_url);
                    var createNewFile = new ISEInterop.CreateNewFile();

					var iseFile = createNewFile.Invoke(file.filename, selectGist.id, content);
                }

                var subscribeForChanges = new ISEInterop
                    .SubscribeToChanges(ISEInterop.CreateNewFile.GetDirPath(selectGist.id), () =>
                    {
                        PendingChangesInCurrentGist = true;
                    });
            });

            CloseGist = new DelegateCommand(async () =>
            {
                if (CurrentGist == null)
                {
                    return;
                }

                var closeItems = new ISEInterop.CloseItems();
                closeItems.Invoke();

                CurrentGist = null;
                CurrentGistFiles = null;

                await Load();
            });

            SaveGist = new DelegateCommand(async () =>
            {
                var saveAll = new ISEInterop.SaveAllItems();
                saveAll.Invoke();

                foreach (var file in CurrentGist.files)
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

            AddFileToCurrentGist = new DelegateCommand(() =>
            {
                if (String.IsNullOrEmpty(AddFileName))
                {
                    Debug.WriteLine("No file name specified.");
                }

                var CreateNewFile = new ISEInterop.CreateNewFile();
                var newFile = CreateNewFile.Invoke(AddFileName, CurrentGist.id, "#"+AddFileName);
                UpdateCurrentGistsFiles();
            });

            CreateNewGist = new DelegateCommand<string>(async x =>
            {
                var gist = await repo.Create(x, AddFileName, "Write-host 'hello'");
                LoadGist.Execute(gist);
            });
        }

        private void UpdateCurrentGistsFiles()
        {
            //update the gui
            CurrentGistFiles.Add(new File() { filename = AddFileName });

            //update the current gist, need to create new list as its immutable
            var currentFiles = new List<File>(CurrentGist.files);
            var updatedFiles = new Files(currentFiles);
            updatedFiles.Add(new File() { filename = AddFileName });
            CurrentGist.files = updatedFiles;
        }

        private ICommand createNewGist;

        public ICommand CreateNewGist
        {
            get { return createNewGist; }
            set { createNewGist = value; NotifyPropertyChanged(); }
        }


        private ICommand loadGist;

        public ICommand LoadGist
        {
            get { return loadGist; }
            set { loadGist = value; NotifyPropertyChanged(); }
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


        private ICommand addFileToCurrentGist;

        public ICommand AddFileToCurrentGist
        {
            get { return addFileToCurrentGist; }
            set { addFileToCurrentGist = value; NotifyPropertyChanged(); }
        }

        private string addFileName;

        public string AddFileName
        {
            get { return addFileName; }
            set { addFileName = value; NotifyPropertyChanged(); }
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
                    return;
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

        //private ObservableCollection<GistObject> starredGists;

        //public ObservableCollection<GistObject> StarredGists
        //{
        //    get { return starredGists; }
        //    set { starredGists = value; NotifyPropertyChanged(); }
        //}

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
