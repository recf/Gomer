using System;
using System.Collections.ObjectModel;
using Gomer.DataAccess;
using Gomer.Models;
using Gomer.Areas.Piles;
using Gomer.Services;

namespace Gomer
{
    public class MainWindowViewModel : BindableBase
    {
        private IOpenFileService _openFileService;
        private readonly IDataService _dataService;
        private readonly IConfirmationService _confirmationService;

        public ObservableCollection<string> RecentFiles { get; private set; }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                SetProperty(ref _fileName, value);
                OnPropertyChanged("Title");
            }
        }

        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                SetProperty(ref _isDirty, value);
                OnPropertyChanged("Title");
            }
        }
        
        public string Title
        {
            get
            {
                var fileName = FileName ?? "New";
                var dirtyMarker = IsDirty ? " *" : string.Empty;

                return string.Format("{0}{1} - Gomer", fileName, dirtyMarker);
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                SetProperty(ref _statusMessage, value);
            }
        }

        private PileDetailViewModel _pileDetail;
        public PileDetailViewModel PileDetail
        {
            get { return _pileDetail; }
            set
            {
                SetProperty(ref _pileDetail, value);
            }
        }

        public RelayCommand OpenCommand { get; private set; }
        public RelayCommand<string> OpenRecentCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SaveAsCommand { get; private set; }

        public RelayCommand ImportGrouveeCsvCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel(IOpenFileService openFileService, IDataService dataService, IConfirmationService confirmationService)
        {
            _openFileService = openFileService;
            _dataService = dataService;
            _confirmationService = confirmationService;

            RecentFiles = _dataService.RecentFiles;

            OpenCommand = new RelayCommand(OpenCommandImpl);
            OpenRecentCommand = new RelayCommand<string>(OpenRecentCommandImpl);
            SaveCommand = new RelayCommand(SaveCommandImpl);
            SaveAsCommand = new RelayCommand(SaveAsCommandImpl);

            ImportGrouveeCsvCommand = new RelayCommand(ImportGrouveeCsvImpl);

            _dataService.PopulateDefaultData();
            ShowPile(null);
        }

        private void ShowPile(string fileName)
        {
            if (PileDetail != null)
            {
                PileDetail.DataChanged -= PileDetail_DataChanged;
            }

            PileDetail = new PileDetailViewModel(_dataService);
            PileDetail.DataChanged += PileDetail_DataChanged;
            PileDetail.Refresh();
            
            IsDirty = false;

            if (string.IsNullOrEmpty(fileName))
            {
                FileName = "New";
            }
            else
            {
                FileName = fileName;
                StatusMessage = string.Format("Opened {0}.", DateTime.Now);
            }
        }

        private void PileDetail_DataChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private delegate bool TrySavePile(out string fileName);

        private void SavePile(TrySavePile trySavePile)
        {
            string fileName;
            if (trySavePile(out fileName))
            {
                FileName = fileName;
                IsDirty = false;
                StatusMessage = string.Format("Last saved {0}.", DateTime.Now);
            }
        }

        public bool ConfirmCloseFile()
        {
            if (!IsDirty)
            {
                return true;
            }

            return _confirmationService.Confirm(
                    "Data has changed since you last saved. Unsaved work will be lost. Do you want to continue?",
                    "Unsaved changes");
        }

        #region Command Implementations

        private void OpenCommandImpl()
        {
            if (!ConfirmCloseFile())
            {
                return;
            }

            string fileName;
            if (_dataService.TryOpen(out fileName))
            {
                ShowPile(fileName);
            }
        }
        
        private void OpenRecentCommandImpl(string fileName)
        {
            if (!ConfirmCloseFile())
            {
                return;
            }

            _dataService.OpenFile(fileName);

            ShowPile(fileName);
        }

        private void SaveCommandImpl()
        {
            SavePile(_dataService.TrySave);
        }

        private void SaveAsCommandImpl()
        {
            SavePile(_dataService.TrySaveAs);
        }
        
        private void ImportGrouveeCsvImpl()
        {
            var fileName = _openFileService.GetFileName("*.csv", "Comma Separated Values (*.csv)|*.csv");

            _dataService.ImportGrouveeCsv(fileName);
            PileDetail.Refresh();
            IsDirty = true;
        }

        #endregion
    }
}