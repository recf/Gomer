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

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel(IDataService dataService, IConfirmationService confirmationService)
        {
            _dataService = dataService;
            _confirmationService = confirmationService;

            RecentFiles = _dataService.RecentFiles;

            OpenCommand = new RelayCommand(OpenCommandImpl);
            OpenRecentCommand = new RelayCommand<string>(OpenRecentCommandImpl);
            SaveCommand = new RelayCommand(SaveCommandImpl);
            SaveAsCommand = new RelayCommand(SaveAsCommandImpl);

            var pile = _dataService.GetNew();
            ShowPile(pile, null);
        }
        
        private void ShowPile(PileModel pile, string fileName)
        {
            if (PileDetail != null)
            {
                PileDetail.DataChanged -= PileDetail_DataChanged;
            }

            PileDetail = new PileDetailViewModel(pile);
            PileDetail.DataChanged += PileDetail_DataChanged;
            
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

        private delegate bool TrySavePile(PileModel pile, out string fileName);

        private void SavePile(TrySavePile trySavePile)
        {
            string fileName;
            if (trySavePile(PileDetail.Pile, out fileName))
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

            PileModel pile;
            string fileName;
            if (_dataService.TryOpen(out pile, out fileName))
            {
                ShowPile(pile, fileName);
            }
        }
        
        private void OpenRecentCommandImpl(string fileName)
        {
            if (!ConfirmCloseFile())
            {
                return;
            }

            var pile = _dataService.OpenFile(fileName);

            ShowPile(pile, fileName);
        }

        private void SaveCommandImpl()
        {
            SavePile(_dataService.TrySave);
        }

        private void SaveAsCommandImpl()
        {
            SavePile(_dataService.TrySaveAs);
        }

        #endregion
    }
}