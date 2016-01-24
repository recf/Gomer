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
using Gomer.Generic;
using Gomer.Models;
using Gomer.ViewModel;

namespace Gomer.Games
{
    public class GameDetailViewModel : DetailViewModelBase<GameModel>
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

        public override GameModel Model
        {
            get { return base.Model; }
            set
            {
                base.Model = value;
                if (Model.List == null)
                {
                    Model.List = Lists.FirstOrDefault();
                }
            } 
        }

        public GameDetailViewModel(ICollection<ListModel> lists)
        {
            Lists = lists;
        }
    }
}
