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

namespace Gomer.Games
{
    // TODO: Game list being in charge of the detail feels wrong. Push up to pile detail vm?
    public class GameListViewModel : ViewModelBase
    {
        private readonly ICollection<GameModel> _games;
        private ISet<string> _platforms;
        private ISet<string> _lists;
        
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

        public GameListViewModel(ICollection<GameModel> games)
        {
            _games = games;

            Games = new ObservableCollection<GameModel>();

            AddCommand = new RelayCommand(AddCommandImpl);
            EditCommand = new RelayCommand<GameModel>(EditCommandImpl);

            Refresh();
        }

        public void Refresh()
        {
            _platforms = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            _lists = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            
            Games.Clear();
            foreach (var game in _games)
            {
                Games.Add(game);
                _platforms.Add(game.Platform);
                _lists.Add(game.List);
            }
        }

        #region Events

        public event EventHandler DataChanged;

        private void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Command Implementations

        private void AddCommandImpl()
        {
            SelectedGameDetails = new GameDetailViewModel(_platforms, _lists)
            {
                Game = new GameModel()
            };
            SelectedGameDetails.Updated += SelectedGameDetails_OnUpdated;
            SelectedGameDetails.Canceled += SelectedGameDetails_OnCanceled;
            SelectedGameDetails.Removed += SelectedGameDetails_Removed;
        }

        private void EditCommandImpl(GameModel game)
        {
            SelectedGameDetails = new GameDetailViewModel(_platforms, _lists)
            {
                Game = game
            };
            SelectedGameDetails.Updated += SelectedGameDetails_OnUpdated;
            SelectedGameDetails.Canceled += SelectedGameDetails_OnCanceled;
            SelectedGameDetails.Removed += SelectedGameDetails_Removed;
        }

        private void SelectedGameDetails_OnUpdated(object sender, GameEventArgs e)
        {
            SelectedGameDetails = null;

            var existing = _games.FirstOrDefault(g => g.Id == e.Game.Id);
            if (existing != null)
            {
                _games.Remove(existing);
            }

            _games.Add(e.Game);
            
            OnDataChanged();
            Refresh();
        }

        void SelectedGameDetails_Removed(object sender, GameEventArgs e)
        {
            SelectedGameDetails = null;

            var existing = _games.FirstOrDefault(g => g.Id == e.Game.Id);
            if (existing != null)
            {
                _games.Remove(existing);
            }

            OnDataChanged();
            Refresh();
        }

        private void SelectedGameDetails_OnCanceled(object sender, EventArgs e)
        {
            SelectedGameDetails = null;
        }

        #endregion
    }
}
