using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using Gomer.Dto;
using Gomer.Games;
using Gomer.Lists;
using Gomer.Models;
using Gomer.Platforms;

namespace Gomer.Piles
{
    // TODO: Derive from DetailViewModelBase
    public class PileDetailViewModel : BindableBase
    {
        private PileModel _pile;

        public PileModel Pile
        {
            get { return _pile; }
            set
            {
                SetProperty(ref _pile, value);

                ListsIndex = new ListIndexViewModel(Pile.Lists);
                PlatformsIndex = new PlatformIndexViewModel(Pile.Platforms);

                GamesIndex = new GameIndexViewModel(Pile.Games, Pile.Lists, Pile.Platforms);
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

                SetProperty(ref _listsIndex, value);

                _listsIndex.DataChanged += SubData_OnDataChanged;
            }
        }


        private PlatformIndexViewModel _platformsIndex;
        public PlatformIndexViewModel PlatformsIndex
        {
            get { return _platformsIndex; }
            set
            {
                if (_platformsIndex != null)
                {
                    _platformsIndex.DataChanged -= SubData_OnDataChanged;
                }

                SetProperty(ref _platformsIndex, value);

                _platformsIndex.DataChanged += SubData_OnDataChanged;
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

                SetProperty(ref _gamesIndex, value);

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
