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
        private bool _isNew;
        private Repository<GameModel, Guid> _repository;

        private GameModel _game;
        public GameModel Game
        {
            get { return _game; }
            private set
            {
                Set(() => Game, ref _game, value);
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        
        public GameDetailViewModel(Repository<GameModel, Guid> repository)
            : this(Guid.Empty, repository)
        {
        }

        public GameDetailViewModel(Guid id, Repository<GameModel, Guid> repository)
        {
            _repository = repository;

            SaveCommand = new RelayCommand(SaveCommandImpl);
            RemoveCommand = new RelayCommand(RemoveCommandImpl);
            CancelCommand = new RelayCommand(OnCanceled);

            Refresh(id);
        }

        private async void Refresh(Guid id)
        {
            if (id == Guid.Empty)
            {
                _isNew = true;
                Game = new GameModel();
                return;
            }

            Game = await _repository.GetItemAsync(id);
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
            if (_isNew)
            {
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
