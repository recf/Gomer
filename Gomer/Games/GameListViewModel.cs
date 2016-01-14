using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;
using Gomer.Models;
using Recfab.Infrastructure;

namespace Gomer.Games
{
    public class GameListViewModel : ViewModelBase
    {
        private ISet<string> _platforms;
        private ISet<string> _lists;

        private Repository<GameModel, Guid> _repository;

        public ObservableCollection<GameModel> Games { get; private set; }

        private GameDetailViewModel _selectedGameDetails;
        public GameDetailViewModel SelectedGameDetails
        {
            get { return _selectedGameDetails; }
            set
            {
                Set(() => SelectedGameDetails, ref _selectedGameDetails, value);
            }
        }
        
        public RelayCommand AddCommand { get; set; }
        public RelayCommand<GameModel> EditCommand { get; set; }

        public GameListViewModel(Repository<GameModel, Guid> repository)
        {
            _repository = repository;

            Games = new ObservableCollection<GameModel>();

            AddCommand = new RelayCommand(AddCommandImpl);
            EditCommand = new RelayCommand<GameModel>(EditCommandImpl);

            Refresh();
        }

        public async void Refresh()
        {
            _platforms = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            _lists = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);

            var games = await _repository.ListItemsAsync();

            Games.Clear();
            foreach (var item in games)
            {
                Games.Add(item);
                _platforms.Add(item.Platform);
                _lists.Add(item.List);
            }
        }

        #region Command Implementations

        private void AddCommandImpl()
        {
            SelectedGameDetails = new GameDetailViewModel(_repository, _platforms, _lists);
            SelectedGameDetails.Saved += SelectedGameDetails_OnSaved;
            SelectedGameDetails.Canceled += SelectedGameDetails_OnCanceled;
        }

        private void EditCommandImpl(GameModel game)
        {
            SelectedGameDetails = new GameDetailViewModel(_repository, _platforms, _lists);
            SelectedGameDetails.Id = game.Id;
            SelectedGameDetails.Saved += SelectedGameDetails_OnSaved;
            SelectedGameDetails.Canceled += SelectedGameDetails_OnCanceled;
        }

        private void SelectedGameDetails_OnSaved(object sender, GameSavedEventArgs e)
        {
            SelectedGameDetails = null;
            Refresh();
        }

        private void SelectedGameDetails_OnCanceled(object sender, EventArgs e)
        {
            SelectedGameDetails = null;
        }

        #endregion
    }
}
