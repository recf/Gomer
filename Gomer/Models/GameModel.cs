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

        public GameModel()
        {
            Id = Guid.NewGuid();
        }
    }
}