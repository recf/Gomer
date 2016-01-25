using System;
using System.ComponentModel.DataAnnotations;


namespace Gomer.Models
{
    public class GameModel : ModelBase<GameModel>
    {
        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private ListModel _list;
        [Required]
        public ListModel List
        {
            get { return _list; }
            set
            {
                SetProperty(ref _list, value);
            }
        }

        private PlatformModel _platform;
        [Required]
        public PlatformModel Platform
        {
            get { return _platform; }
            set
            {
                SetProperty(ref _platform, value);
            }
        }
        
        private DateTime? _addedOn;
        public DateTime? AddedOn
        {
            get { return _addedOn; }
            set
            {
                SetProperty(ref _addedOn, value);
            }
        }

        private DateTime? _startedOn;
        public DateTime? StartedOn
        {
            get { return _startedOn; }
            set
            {
                SetProperty(ref _startedOn, value);
            }
        }

        private DateTime? _finishedOn;
        public DateTime? FinishedOn
        {
            get { return _finishedOn; }
            set
            {
                SetProperty(ref _finishedOn, value);
            }
        }

        private decimal? _estimatedHours;
        public decimal? EstimatedHours
        {
            get { return _estimatedHours; }
            set
            {
                SetProperty(ref _estimatedHours, value);
                OnPropertyChanged("RemainingHours");
            }
        }

        private decimal? _playedHours;
        public decimal? PlayedHours
        {
            get { return _playedHours; }
            set
            {
                SetProperty(ref _playedHours, value);
                OnPropertyChanged("RemainingHours");
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
            AddedOn = DateTime.Today;
        }

        public override void SetFrom(GameModel other)
        {
            Id = other.Id;
            Name = other.Name;

            List = other.List;
            Platform = other.Platform;
            AddedOn = other.AddedOn;
            StartedOn = other.StartedOn;
            FinishedOn = other.FinishedOn;
            EstimatedHours = other.EstimatedHours;
            PlayedHours = other.PlayedHours;
        }
    }
}