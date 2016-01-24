﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Generic;
using Gomer.Models;
using Gomer.ViewModel;

namespace Gomer.Games
{
    public class GameIndexViewModel : IndexViewModelBase<GameModel, GameListViewModel, GameDetailViewModel>
    {
        private ICollection<ListModel> _lists;
        public ICollection<ListModel> Lists
        {
            get { return _lists; }
            set
            {
                Set(() => Lists, ref _lists, value);
            }
        }

        public GameIndexViewModel(ObservableCollection<GameModel> models, ICollection<ListModel> lists) :
            base(models,
            new GameListViewModel(models, lists),
            new GameDetailViewModel(lists))
        {
        }
    }
}