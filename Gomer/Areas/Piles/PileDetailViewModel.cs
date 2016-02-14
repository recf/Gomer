using System;
using Gomer.Areas.Games;
using Gomer.Areas.Lists;
using Gomer.Models;
using Gomer.Areas.Platforms;
using Gomer.Areas.Reports;
using Gomer.DataAccess;

namespace Gomer.Areas.Piles
{
    // TODO: Move in MainViewModel?
    public class PileDetailViewModel : ViewModelBase
    {
        private ListIndexViewModel _listsIndex;
        public ListIndexViewModel ListsIndex
        {
            get { return _listsIndex; }
            set
            {
                if (_listsIndex != null)
                {
                    _listsIndex.DataChanged -= LookupData_OnDataChanged;
                }

                SetProperty(ref _listsIndex, value);

                _listsIndex.DataChanged += LookupData_OnDataChanged;
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
                    _platformsIndex.DataChanged -= LookupData_OnDataChanged;
                }

                SetProperty(ref _platformsIndex, value);

                _platformsIndex.DataChanged += LookupData_OnDataChanged;
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
                    _gamesIndex.DataChanged -= GameData_OnDataChanged;
                }

                SetProperty(ref _gamesIndex, value);

                _gamesIndex.DataChanged += GameData_OnDataChanged;
            }
        }

        private ReportViewModel _report;
        public ReportViewModel Report
        {
            get { return _report; }
            set
            {
                SetProperty(ref _report, value);
            }
        }

        public PileDetailViewModel(IDataService dataService)
        {
            ListsIndex = new ListIndexViewModel(dataService.Lists);
            PlatformsIndex = new PlatformIndexViewModel(dataService.Platforms);

            GamesIndex = new GameIndexViewModel(dataService.Games, dataService.Lists, dataService.Platforms);

            Report = new ReportViewModel(dataService.Games);
        }

        public override void Refresh()
        {
            ListsIndex.Refresh();
            PlatformsIndex.Refresh();

            GamesIndex.Refresh();
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

        private void LookupData_OnDataChanged(object sender, EventArgs e)
        {
            OnDataChanged();
        }

        private void GameData_OnDataChanged(object sender, EventArgs e)
        {
            OnDataChanged();
        }
    }
}
