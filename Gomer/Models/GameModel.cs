using System;
using System.Collections.ObjectModel;
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

        private DateTime _addedOn;
        public DateTime AddedOn
        {
            get { return _addedOn; }
            set
            {
                SetProperty(ref _addedOn, value);
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

        public ObservableCollection<PlatformModel> Platforms { get; private set; }

        private StatusModel _status;
        [Required]
        public StatusModel Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public ObservableCollection<StatusHistoryModel> StatusHistory { get; private set; }
        
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
                var played = PlayedHours ?? 0;

                return est - played;
            }
        }

        public GameModel()
        {
            Platforms = new ObservableCollection<PlatformModel>();
            StatusHistory = new ObservableCollection<StatusHistoryModel>();

            AddedOn = DateTime.Today;
        }

        public override void SetFrom(GameModel other)
        {
            Id = other.Id;
            Name = other.Name;

            List = other.List;
            Status = other.Status;

            AddedOn = other.AddedOn;

            Platforms.Clear();
            foreach (var platform in other.Platforms)
            {
                Platforms.Add(platform);
            }

            EstimatedHours = other.EstimatedHours;
            PlayedHours = other.PlayedHours;

            StatusHistory.Clear();
            foreach (var otherHx in other.StatusHistory)
            {
                var statusHx = new StatusHistoryModel();
                statusHx.SetFrom(otherHx);
                StatusHistory.Add(statusHx);
            }
        }
    }
}