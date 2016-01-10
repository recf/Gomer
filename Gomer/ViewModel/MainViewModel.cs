using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Dto;
using Gomer.Models;
using Gomer.PileGames;
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
        private JsonStructuredFileManager<PileDto> _manager;

        private Repository<GameModel, Guid> _repository;

        private ObservableCollection<GameModel> _pileGames;

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                Set(() => Title, ref _title, value);
            }
        }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                Set(() => CurrentViewModel, ref _currentViewModel, value);
            }
        }

        public RelayCommand OpenCommand { get; protected set; }

        public RelayCommand SaveCommand { get; protected set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            _repository = new MemoryRepository<GameModel, Guid>(x => x.Id);

            OpenCommand = new RelayCommand(OpenCommandImpl);

            SaveCommand = new RelayCommand(SaveCommandImpl);

            _pileGames = new ObservableCollection<GameModel>();

            Navigate(new PileGameListViewModel(_pileGames), "Pile");
        }

        private void Navigate(ViewModelBase viewModel, string title)
        {
            CurrentViewModel = viewModel;
        }

        private void OpenCommandImpl()
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".pile";
            dialog.Filter = "Gomer Pile File (*.pile)|*.pile";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _manager = new JsonStructuredFileManager<PileDto>(dialog.FileName);
            var pile = _manager.Read();

            var gameModels = Mapper.Map<IList<GameModel>>(pile.PileGames);

            _pileGames.Clear();
            foreach (var model in gameModels)
            {
                _pileGames.Add(model);
            }
        }

        private void SaveCommandImpl()
        {
            if (_manager == null)
            {
                var dialog = new SaveFileDialog();
                dialog.DefaultExt = ".pile";
                dialog.Filter = "Gomer Pile File (*.pile)|*.pile";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _manager = new JsonStructuredFileManager<PileDto>(dialog.FileName);
            }

            var gameDtos = _pileGames.Select(Mapper.Map<GameDto>).ToArray();

            var pile = new PileDto();
            pile.PileGames = gameDtos;
            pile.WishlistGames = new List<GameDto>();
            pile.IgnoredGames = new List<GameDto>();

            _manager.Write(pile);
        }
    }
}