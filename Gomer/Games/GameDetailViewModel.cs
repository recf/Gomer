using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        private GameLists _list;
        public GameLists List
        {
            get { return _list; }
            set
            {
                Set(() => List, ref _list, value);
                Game.List = value;
            }
        }

        #endregion

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public GameDetailViewModel(Repository<GameModel, Guid> repository)
        {
            _repository = repository;

            Id = Guid.Empty;
            List = GameLists.Pile;

            SaveCommand = new RelayCommand(SaveCommandImpl);
            RemoveCommand = new RelayCommand(RemoveCommandImpl);
            CancelCommand = new RelayCommand(OnCanceled);
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

        public event EventHandler Saved;

        private void OnSaved()
        {
            if (Saved != null)
            {
                Saved(this, new EventArgs());
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
    }
}
