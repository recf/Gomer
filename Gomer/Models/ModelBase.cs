using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Models
{
    public abstract class ModelBase<TSelf> : ValidatableBindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }
        
        public bool IsNew
        {
            get { return Id == 0; }
        }

        protected ModelBase()
        {
            Id = 0;
        }

        public abstract void SetFrom(TSelf other);
    }
}
