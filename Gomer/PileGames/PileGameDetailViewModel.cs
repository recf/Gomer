using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Gomer.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Recfab.Infrastructure;

namespace Gomer.PileGames
{
    public class PileGameDetailViewModel : ViewModel
    {
        private Repository<PileGameModel, Guid> _repository;

        [Reactive]
        public Guid Id { get; set; }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string Platform { get; set; }

        public ReactiveCommand<object> OkCommand { get; private set; }

        public ReactiveCommand<object> CancelCommand { get; private set; }

        private Subject<bool> _done;

        public IObservable<bool> Done
        {
            get { return _done; } 
        }

        public PileGameDetailViewModel(Repository<PileGameModel, Guid> repository)
        {
            _repository = repository;

            OkCommand = ReactiveCommand.Create();
            CancelCommand = ReactiveCommand.Create();

            OkCommand.Subscribe(_ => OkCommandImpl());
            CancelCommand.Subscribe(_ => _done.OnNext(false));

            _done = new Subject<bool>();
        }

        private async void OkCommandImpl()
        {
            var model = Mapper.Map<PileGameModel>(this);

            await _repository.AddItemAsync(model);

            _done.OnNext(true);
        }
    }
}
