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
    // TODO: Derive from DetailViewModelBase
    public class PileDetailViewModel : ViewModelBase
    {
        private PileModel _pile;

        public PileModel Pile
        {
            get { return _pile; }
            set
            {
                Set(() => Pile, ref _pile, value);

                ListsIndex = new ListIndexViewModel(Pile.Lists);
                ListsIndex.DataChanged += SubData_OnDataChanged;

                GamesIndex = new GameIndexViewModel(Pile.Games, Pile.Lists);
            }
        }

        private ListIndexViewModel _listsIndex;
        public ListIndexViewModel ListsIndex
        {
            get { return _listsIndex; }
            set
            {
                if (_listsIndex != null)
                {
                    _listsIndex.DataChanged -= SubData_OnDataChanged;
                }

                Set(() => ListsIndex, ref _listsIndex, value);

                _listsIndex.DataChanged += SubData_OnDataChanged;
            }
        }

        private GameIndexViewModel _gamesIndex;
        public GameIndexViewModel GamesIndex
        {
            get { return _gamesIndex; }
            set
            {
                if (_gamesIndex != null)
                {
                    _gamesIndex.DataChanged -= SubData_OnDataChanged;
                }

                Set(() => GamesIndex, ref _gamesIndex, value);

                _gamesIndex.DataChanged += SubData_OnDataChanged;
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
