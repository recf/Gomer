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

        #region Status & Dates

        public StatusCodes StatusCode
        {
            get
            {
                if (FinishedOn.HasValue) { return StatusCodes.Finished; }
                if (RetiredOn.HasValue) { return StatusCodes.Retired; }
                if (StartedOn.HasValue) { return StatusCodes.InProgress; }

                return StatusCodes.NotStarted;
            }
        }

        private DateTime _addedOn;
        public DateTime AddedOn
        {
            get { return _addedOn; }
            set
            {
                SetProperty(ref _addedOn, value);
                OnPropertyChanged("StatusCode");
            }
        }

        private DateTime? _startedOn;
        public DateTime? StartedOn
        {
            get { return _startedOn; }
            set
            {
                SetProperty(ref _startedOn, value);
                OnPropertyChanged("StatusCode");
            }
        }

        private DateTime? _finishedOn;
        public DateTime? FinishedOn
        {
            get { return _finishedOn; }
            set
            {
                SetProperty(ref _finishedOn, value);
                OnPropertyChanged("StatusCode");
            }
        }

        private DateTime? _retiredOn;
        public DateTime? RetiredOn
        {
            get { return _retiredOn; }
            set
            {
                SetProperty(ref _retiredOn, value);
                OnPropertyChanged("StatusCode");
            }
        }

        #endregion

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

        private bool _isDigital;
        public bool IsDigital
        {
            get { return _isDigital; }
            set { SetProperty(ref _isDigital, value); }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { SetProperty(ref _note, value); }
        }

        public GameModel()
        {
            Platforms = new ObservableCollection<PlatformModel>();

            AddedOn = DateTime.Today;
        }

        public override void SetFrom(GameModel other)
        {
            Id = other.Id;
            Name = other.Name;

            List = other.List;

            AddedOn = other.AddedOn;
            StartedOn = other.StartedOn;
            FinishedOn = other.FinishedOn;
            RetiredOn = other.RetiredOn;

            Platforms.Clear();
            foreach (var platform in other.Platforms)
            {
                Platforms.Add(platform);
            }

            EstimatedHours = other.EstimatedHours;
            PlayedHours = other.PlayedHours;

            IsDigital = other.IsDigital;
            Note = other.Note;
        }
    }
}