using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Events;
using Gomer.Models;
using Recfab.Infrastructure;

namespace Gomer.Games
{
    public class GameDetailViewModel : ViewModelBase
    {
        private Repository<GameModel, Guid> _repository;
        private GameLists _originalList;

        public ObservableCollection<GameLists> Lists { get; private set; }

        #region INotifyPropertyChanged Properties

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set
            {
                Set(() => Id, ref _id, value);
                Refresh();
            }
        }

        private GameModel _game;
        public GameModel Game
        {
            get { return _game; }
            private set
            {
                Set(() => Game, ref _game, value);
            }
        }

        #endregion

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }

        public GameDetailViewModel(Repository<GameModel, Guid> repository, GameLists originalList)
        {
            _repository = repository;
            _originalList = originalList;

            Id = Guid.Empty;
            Lists = new ObservableCollection<GameLists>();
            foreach (var list in Enum.GetValues(typeof(GameLists)))
            {
                Lists.Add((GameLists)list);
            }

            CancelCommand = new RelayCommand(OnCanceled);
            SaveCommand = new RelayCommand(SaveCommandImpl);
            RemoveCommand = new RelayCommand(RemoveCommandImpl);

            Refresh();
        }

        private async void Refresh()
        {
            if (Id == Guid.Empty)
            {
                Game = new GameModel()
                {
                    List = _originalList,
                    AddedOn = DateTime.Today
                };
                return;
            }

            Game = await _repository.GetItemAsync(Id);
        }

        #region Events

        public event EventHandler<GameSavedEventArgs> Saved;

        private void OnSaved()
        {
            if (Saved != null)
            {
                var args = new GameSavedEventArgs()
                {
                    OriginalList = _originalList,
                    Game = Game
                };

                Saved(this, args);
            }
        }

        public event EventHandler Canceled;

        private void OnCanceled()
        {
            if (Canceled != null)
            {
                Canceled(this, new EventArgs());
            }
        }

        #endregion

        #region Command Implementations

        private async void SaveCommandImpl()
        {
            if (Id == Guid.Empty)
            {
                Game.Id = Guid.NewGuid();
                await _repository.AddItemAsync(Game);
            }
            else
            {
                await _repository.UpdateItemAsync(Game);
            }
            OnSaved();
        }

        private async void RemoveCommandImpl()
        {
            await _repository.RemoveItemAsync(Game.Id);
            OnSaved();
        }

        #endregion
    }
}
