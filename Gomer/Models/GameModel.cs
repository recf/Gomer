using System;
using GalaSoft.MvvmLight;

namespace Gomer.Models
{
    public class GameModel : ObservableObject
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set
            {
                Set(() => Id, ref _id, value);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        private string _platform;
        public string Platform
        {
            get { return _platform; }
            set
            {
                Set(() => Platform, ref _platform, value);
            }
        }

        private GameLists _list;
        public GameLists List
        {
            get { return _list; }
            set
            {
                Set(() => List, ref _list, value);
            }
        }

        private DateTime _addedOn;
        public DateTime AddedOn
        {
            get { return _addedOn; }
            set
            {
                Set(() => AddedOn, ref _addedOn, value);
            }
        }

        private DateTime? _startedOn;
        public DateTime? StartedOn
        {
            get { return _startedOn; }
            set
            {
                Set(() => StartedOn, ref _startedOn, value);
            }
        }

        private DateTime? _finishedOn;
        public DateTime? FinishedOn
        {
            get { return _finishedOn; }
            set
            {
                Set(() => FinishedOn, ref _finishedOn, value);
                if (value.HasValue)
                {
                    List = GameLists.Played;
                }
            }
        }

        private decimal? _estimatedHours;
        public decimal? EstimatedHours
        {
            get { return _estimatedHours; }
            set
            {
                Set(() => EstimatedHours, ref _estimatedHours, value);
                RaisePropertyChanged(() => RemainingHours);
            }
        }

        private decimal? _playedHours;
        public decimal? PlayedHours
        {
            get { return _playedHours; }
            set
            {
                Set(() => PlayedHours, ref _playedHours, value);
                RaisePropertyChanged(() => RemainingHours);
            }
        }

        public decimal? RemainingHours
        {
            get
            {
                if (!EstimatedHours.HasValue)
                {
                    return null;
                }

                var est = EstimatedHours.Value;
                var played = PlayedHours.HasValue ? PlayedHours.Value : 0;

                return est - played;
            }
        }

        public GameModel()
        {
            Id = Guid.NewGuid();
        }
    }
}