using System;
using System.Collections.Generic;
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

        #region Notify Properties

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set
            {
                Set(() => Id, ref _id, value);
                RaisePropertyChanged(() => UsePileCommands);
                RaisePropertyChanged(() => UseWishlistCommands);
                RaisePropertyChanged(() => UseIgnoreListCommands);
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

        private GameLists? _movedToList = null;
        private GameLists _list;
        public GameLists List
        {
            get { return _list; }
            set
            {
                Set(() => List, ref _list, value);
                RaisePropertyChanged(() => UsePileCommands);
                RaisePropertyChanged(() => UseWishlistCommands);
                RaisePropertyChanged(() => UseIgnoreListCommands);
            }
        }

        public bool UsePileCommands
        {
            get
            {
                return Id != Guid.Empty && Game.List == GameLists.Pile;
            }
        }

        public bool UseWishlistCommands
        {
            get
            {
                return Id != Guid.Empty && Game.List == GameLists.Wishlist;
            }
        }

        public bool UseIgnoreListCommands
        {
            get
            {
                return Id != Guid.Empty && Game.List == GameLists.Ignored;
            }
        }

        #endregion

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }
        public RelayCommand<GameLists> MoveToListCommand { get; private set; }

        public GameDetailViewModel(Repository<GameModel, Guid> repository)
        {
            _repository = repository;

            Id = Guid.Empty;
            List = GameLists.Pile;

            CancelCommand = new RelayCommand(OnCanceled);
            SaveCommand = new RelayCommand(SaveCommandImpl);
            RemoveCommand = new RelayCommand(RemoveCommandImpl);
            MoveToListCommand = new RelayCommand<GameLists>(MoveToListCommandImpl);

            Refresh();
        }

        private async void Refresh()
        {
            if (Id == Guid.Empty)
            {
                Game = new GameModel()
                {
                    List = List
                };
                return;
            }

            Game = await _repository.GetItemAsync(Id);
            List = Game.List;
        }

        #region Events

        public event EventHandler<GameSavedEventArgs> Saved;

        private void OnSaved()
        {
            if (Saved != null)
            {
                var args = new GameSavedEventArgs()
                {
                    MovedToList = _movedToList
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
            Game.List = List;
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

        private void MoveToListCommandImpl(GameLists newList)
        {
            _movedToList = newList;
            List = newList;
            SaveCommand.Execute(null);
        }

        #endregion
    }
}
