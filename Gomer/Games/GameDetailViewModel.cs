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
        public ObservableCollection<ListModel> Lists { get; private set; }
        public ObservableCollection<PlatformModel> Platforms { get; private set; }

        public override GameModel Model
        {
            get { return base.Model; }
            set
            {
                base.Model = value;
                if (Model != null)
                {
                    Model.List = Lists.FirstOrDefault();
                    Model.Platform = Platforms.FirstOrDefault();
                }
            } 
        }

        public GameDetailViewModel(
            ObservableCollection<ListModel> lists, 
            ObservableCollection<PlatformModel> platforms)
        {
            Lists = lists;
            Platforms = platforms;
        }
    }
}
