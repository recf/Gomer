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
using Gomer.Services;
using Recfab.Infrastructure;

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

        private GameListViewModel _pileGames;
        public GameListViewModel PileGames
        {
            get { return _pileGames; }
            set
            {
                Set(() => PileGames, ref _pileGames, value);
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
                var docsdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
            var gameModels = Mapper.Map<ICollection<GameModel>>(pile.Games);
            PileGames = new GameListViewModel(gameModels);
        }

        #region Command Implementations
        private void OpenCommandImpl()
        {
            PileDto pile;
            if (_dataService.TryOpen(out pile))
            {
                ShowPile(pile);
                StatusMessage = string.Format("Opened {0}.", DateTime.Now);
            }
        }

        private void SaveCommandImpl()
        {
            var pile = new PileDto();
            pile.Games = Mapper.Map<ICollection<GameDto>>(PileGames.Games);

            if (_dataService.TrySave(pile))
            {
                StatusMessage = string.Format("Last saved {0}.", DateTime.Now);
            }
        }

        private void SaveAsCommandImpl()
        {
            var pile = new PileDto();
            pile.Games = Mapper.Map<ICollection<GameDto>>(PileGames.Games);

            if (_dataService.TrySaveAs(pile))
            {
                StatusMessage = string.Format("Last saved {0}.", DateTime.Now);
            }
        }

        #endregion
    }
}