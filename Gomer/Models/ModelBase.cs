using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Gomer.Models
{
    public abstract class ModelBase<TSelf> : ObservableObject
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

        protected ModelBase()
        {
            Id = Guid.NewGuid();
        }

        public abstract void SetFrom(TSelf other);
    }
}
