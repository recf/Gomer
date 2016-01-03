﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Gomer.Models
{
    public class PlatformModel : ReactiveObject
    {
        [Reactive]
        public Guid Id { get; set; }

        [Reactive]
        public string Abbreviation { get; set; }

        [Reactive]
        public string Name { get; set; }
    }
}
