using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Gomer.Models
{
    public class PileGameModel : ReactiveObject
    {
        [Reactive]
        public Guid Id { get; set; }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public PlatformModel Platform { get; set; }

        public PileGameModel()
        {
            Id = Guid.NewGuid();
        }
    }
}