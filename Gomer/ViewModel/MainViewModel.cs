using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Dto;
using Gomer.Models;
using Gomer.Games;
using Gomer.Piles;
using Gomer.Services;

namespace Gomer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
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
                Set(() => FileName, ref _fileName, value);
                RaisePropertyChanged(() => Title);
            }
        }

        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                Set(() => IsDirty, ref _isDirty, value);
                RaisePropertyChanged(() => Title);
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
                Set(() => StatusMessage, ref _statusMessage, value);
            }
        }

        private PileDetailViewModel _pileDetail;
        public PileDetailViewModel PileDetail
        {
            get { return _pileDetail; }
            set
            {
                Set(() => PileDetail, ref _pileDetail, value);
            }
        }

        public RelayCommand OpenCommand { get; private set; }
        public RelayCommand<string> OpenRecentCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SaveAsCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService, IConfirmationService confirmationService)
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }

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

        private void ShowPile(PileDto pileDto, string fileName)
        {
            if (PileDetail != null)
            {
                PileDetail.DataChanged -= PileDetail_DataChanged;
            }

            var pile = Mapper.Map<PileModel>(pileDto);

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

        private delegate bool TrySavePile(PileDto pile, out string fileName);

        private void SavePile(TrySavePile trySavePile)
        {
            var pile = Mapper.Map<PileDto>(PileDetail.Pile);

            string fileName;
            if (trySavePile(pile, out fileName))
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

            PileDto pile;
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