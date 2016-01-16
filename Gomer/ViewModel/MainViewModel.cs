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
        private IDataService _dataService;

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
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SaveAsCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
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

            OpenCommand = new RelayCommand(OpenCommandImpl);
            SaveCommand = new RelayCommand(SaveCommandImpl);
            SaveAsCommand = new RelayCommand(SaveAsCommandImpl);

            var pile = _dataService.GetNew();
            ShowPile(pile);
        }

        private void ShowPile(PileDto pile)
        {
            if (PileDetail != null)
            {
                PileDetail.DataChanged -= PileDetail_DataChanged;
            }

            var model = Mapper.Map<PileModel>(pile);

            PileDetail = new PileDetailViewModel(model);
            PileDetail.DataChanged += PileDetail_DataChanged;
        }

        void PileDetail_DataChanged(object sender, EventArgs e)
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

        #region Command Implementations

        private void OpenCommandImpl()
        {
            PileDto pile;
            string fileName;
            if (_dataService.TryOpen(out pile, out fileName))
            {
                ShowPile(pile);
                FileName = fileName;
                IsDirty = false;
                StatusMessage = string.Format("Opened {0}.", DateTime.Now);
            }
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