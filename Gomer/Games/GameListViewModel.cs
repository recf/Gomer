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
        private Repository<GameModel, Guid> _repository;
        private GameLists _list;

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

        public GameListViewModel(Repository<GameModel, Guid> repository, GameLists list)
        {
            _repository = repository;
            _list = list;

            Games = new ObservableCollection<GameModel>();

            AddCommand = new RelayCommand(AddCommandImpl);
            EditCommand = new RelayCommand<GameModel>(EditCommandImpl);

            Refresh();
        }

        public async void Refresh()
        {
            var games = await _repository.ListItemsAsync();

            Games.Clear();
            foreach (var item in games.Where(g => g.List == _list))
            {
                Games.Add(item);
            }
        }

        #region Events

        public event EventHandler<GameListChangedEventArgs> OtherListChanged;

        private void OnOtherListChanged(GameLists list)
        {
            if (OtherListChanged != null)
            {
                var args = new GameListChangedEventArgs
                {
                    List = list
                };
                OtherListChanged(this, args);
            }
        }

        #endregion

        #region Command Implementations

        private void AddCommandImpl()
        {
            SelectedGameDetails = new GameDetailViewModel(_repository);
            SelectedGameDetails.List = _list;
            SelectedGameDetails.Saved += SelectedGameDetails_OnSaved;
            SelectedGameDetails.Canceled += SelectedGameDetails_OnCanceled;
        }

        private void EditCommandImpl(GameModel game)
        {
            SelectedGameDetails = new GameDetailViewModel(_repository);
            SelectedGameDetails.Id = game.Id;
            SelectedGameDetails.Saved += SelectedGameDetails_OnSaved;
            SelectedGameDetails.Canceled += SelectedGameDetails_OnCanceled;
        }

        private void SelectedGameDetails_OnSaved(object sender, GameSavedEventArgs e)
        {
            SelectedGameDetails = null;
            Refresh();
            if (e.MovedToList.HasValue)
            {
                OnOtherListChanged(e.MovedToList.Value);
            }
        }

        private void SelectedGameDetails_OnCanceled(object sender, EventArgs e)
        {
            SelectedGameDetails = null;
        }

        #endregion
    }
}
