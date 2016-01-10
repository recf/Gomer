using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gomer.Dto;
using Gomer.Models;
using Gomer.Games;
using Recfab.Infrastructure;

namespace Gomer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Repository<GameModel, Guid> _repository;

        private GameListViewModel _pileGames;
        public GameListViewModel PileGames
        {
            get { return _pileGames; }
            set
            {
                Set(() => PileGames, ref _pileGames, value);
            }
        }

        private GameListViewModel _playedGames;
        public GameListViewModel PlayedGames
        {
            get { return _playedGames; }
            set
            {
                Set(() => PlayedGames, ref _playedGames, value);
            }
        }

        private GameListViewModel _wishlistGames;
        public GameListViewModel WishlistGames
        {
            get { return _wishlistGames; }
            set
            {
                Set(() => WishlistGames, ref _wishlistGames, value);
            }
        }

        private GameListViewModel _ignoredGames;
        public GameListViewModel IgnoredGames
        {
            get { return _ignoredGames; }
            set
            {
                Set(() => IgnoredGames, ref _ignoredGames, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                _repository = new MemoryRepository<GameModel, Guid>(x => x.Id);
            }
            else
            {
                // Code runs "for real"
                var docsdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var path = Path.Combine(docsdir, "gomer.pile");
                var innerRepo = new FileBackedRepository<GameDto, Guid>(path, x => x.Id);
                _repository = new MappingRepository<GameModel, Guid, GameDto, Guid>(
                    model => Mapper.Map<GameDto>(model),
                    dto => Mapper.Map<GameModel>(dto),
                    id => id,
                    innerRepo,
                    model => model.Id);
            }

            PileGames = new GameListViewModel(_repository, GameLists.Pile);
            PlayedGames = new GameListViewModel(_repository, GameLists.Played);
            WishlistGames = new GameListViewModel(_repository, GameLists.Wishlist);
            IgnoredGames = new GameListViewModel(_repository, GameLists.Ignored);

            PileGames.OtherListChanged += GameList_OtherListChanged;
            PlayedGames.OtherListChanged += GameList_OtherListChanged;
            WishlistGames.OtherListChanged += GameList_OtherListChanged;
            IgnoredGames.OtherListChanged += GameList_OtherListChanged;
        }

        private void GameList_OtherListChanged(object sender, Events.GameListChangedEventArgs e)
        {
            switch (e.List)
            {
                case GameLists.Pile:
                    PileGames.Refresh();
                    break;
                case GameLists.Played:
                    PlayedGames.Refresh();
                    break;
                case GameLists.Wishlist:
                    WishlistGames.Refresh();
                    break;
                case GameLists.Ignored:
                    IgnoredGames.Refresh();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("e.List", string.Format("Unhandled GameList value {0}", e.List));
            }
        }
    }
}