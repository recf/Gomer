using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Models
{
    public abstract class ModelBase<TSelf> : ValidatableBindableBase
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        protected ModelBase()
        {
            Id = Guid.NewGuid();
        }

        public abstract void SetFrom(TSelf other);
    }
}
