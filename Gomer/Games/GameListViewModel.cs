using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Models;
using Recfab.Infrastructure;

namespace Gomer.Games
{
    public class GameListViewModel : ViewModelBase
    {
        private Repository<GameModel, Guid> _repository;

        public ObservableCollection<GameModel> Games { get; private set; }

        private GameModel _selectedGame;
        public GameModel SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                Set(() => SelectedGame, ref _selectedGame, value);
            }
        }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand<GameModel> RemoveCommand { get; set; }

        public GameListViewModel(Repository<GameModel, Guid> repository)
        {
            _repository = repository;

            Games = new ObservableCollection<GameModel>();
            Refresh();

            AddCommand = new RelayCommand(AddCommandImpl);
            RemoveCommand = new RelayCommand<GameModel>(RemoveCommandImpl);
        }

        public async void Refresh()
        {
            var games = await _repository.ListItemsAsync();

            Games.Clear();
            foreach (var item in games)
            {
                Games.Add(item);
            }

            if (games.Any())
            {
                SelectedGame = games.First();
            }
        }

        private async void AddCommandImpl()
        {
            var game = new GameModel()
            {
                Name = "{Name}",
                Platform = "{Platform}"
            };
            await _repository.AddItemAsync(game);
            Games.Add(game);
            SelectedGame = game;
        }

        private async void RemoveCommandImpl(GameModel game)
        {
            await _repository.RemoveItemAsync(game.Id);
            Games.Remove(game);
        }
    }
}
