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
        #region INotifyPropertyChanged Properties

        private GameModel _game;
        public GameModel Game
        {
            get { return _game; }
            set
            {
                var workingCopy = Mapper.Map<GameModel>(value);
                Set(() => Game, ref _game, workingCopy);
            }
        }

        public ObservableCollection<string> Platforms { get; private set; }
        public ObservableCollection<string> Lists { get; private set; }

        #endregion

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }

        public GameDetailViewModel(ISet<string> platforms, ISet<string> lists)
        {
            Platforms = new ObservableCollection<string>(platforms);
            Lists = new ObservableCollection<string>(lists);

            CancelCommand = new RelayCommand(OnCanceled);
            UpdateCommand = new RelayCommand(SaveCommandImpl);
            RemoveCommand = new RelayCommand(RemoveCommandImpl);
        }

        #region Events

        public event EventHandler<GameEventArgs> Updated;

        private void OnSaved()
        {
            if (Updated != null)
            {
                var args = new GameEventArgs()
                {
                    Game = Game
                };

                Updated(this, args);
            }
        }

        public event EventHandler<GameEventArgs> Removed;

        private void OnRemoved()
        {
            if (Removed != null)
            {
                var args = new GameEventArgs()
                {
                    Game = Game
                };

                Removed(this, args);
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

        private void SaveCommandImpl()
        {
            OnSaved();
        }

        private void RemoveCommandImpl()
        {
            OnRemoved();
        }

        #endregion
    }
}
