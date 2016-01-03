using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using AutoMapper;
using Gomer.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Recfab.Infrastructure;

namespace Gomer.PileGames
{
    public class PileGameDetailViewModel : ViewModel
    {
        private readonly Repository<PileGameModel, Guid> _repository;
        private readonly Repository<PlatformModel, string> _platformRepository;

        [Reactive]
        public Guid Id { get; set; }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public PlatformModel Platform { get; set; }

        public ReactiveList<PlatformModel> Platforms { get; set; }

        public ReactiveCommand<object> OkCommand { get; private set; }

        public ReactiveCommand<object> CancelCommand { get; private set; }

        private readonly Subject<bool> _done;

        public IObservable<bool> Done
        {
            get { return _done; } 
        }

        public PileGameDetailViewModel(Repository<PileGameModel, Guid> repository, Repository<PlatformModel, string> platformRepository)
        {
            _repository = repository;
            _platformRepository = platformRepository;
            
            Id = Guid.NewGuid();
            Platforms = new ReactiveList<PlatformModel>();

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

        public override async void Refresh()
        {
            var refresh = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var platforms = await _platformRepository.ListItemsAsync();

                foreach (var game in platforms)
                {
                    Platforms.Add(game);
                }
            });

            await refresh.ExecuteAsync();
        }
    }
}
