using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight;
using Gomer.Dto;
using Gomer.Games;
using Gomer.Lists;
using Gomer.Models;

namespace Gomer.Piles
{
    public class PileDetailViewModel : ViewModelBase
    {
        private PileModel _pile;

        public PileModel Pile
        {
            get { return _pile; }
            set
            {
                Set(() => Pile, ref _pile, value);

                ListsList = new ListListViewModel(Pile.Lists);
                GamesList = new GameListViewModel(Pile.Games, Pile.Lists);
            }
        }

        private ListListViewModel _listsList;
        public ListListViewModel ListsList
        {
            get { return _listsList; }
            set
            {
                if (_listsList != null)
                {
                    _listsList.DataChanged -= SubData_OnDataChanged;
                }

                Set(() => ListsList, ref _listsList, value);

                _listsList.DataChanged += SubData_OnDataChanged;
            }
        }

        private GameListViewModel _gamesList;
        public GameListViewModel GamesList
        {
            get { return _gamesList; }
            set
            {
                if (_gamesList != null)
                {
                    _gamesList.DataChanged -= SubData_OnDataChanged;
                }

                Set(() => GamesList, ref _gamesList, value);

                _gamesList.DataChanged += SubData_OnDataChanged;
            }
        }
        
        public PileDetailViewModel(PileModel pile)
        {
            Pile = pile;
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

        private void SubData_OnDataChanged(object sender, EventArgs e)
        {
            OnDataChanged();
        }
    }
}
