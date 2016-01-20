﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Gomer.Models
{
    public class ListModel : ModelBase<ListModel>
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        public override void SetFrom(ListModel other)
        {
            Id = other.Id;
            Name = other.Name;
        }
    }
}
