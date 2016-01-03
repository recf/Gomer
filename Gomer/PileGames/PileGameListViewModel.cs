using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Gomer.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Recfab.Infrastructure;

namespace Gomer.PileGames
{
    public class PileGameListViewModel : ViewModel
    {
        private readonly Repository<PileGameModel, Guid> _repository;

        public ReactiveList<PileGameModel> Games { get; private set; }

        public PileGameListViewModel(Repository<PileGameModel, Guid> repository)
        {
            Games = new ReactiveList<PileGameModel>();

            _repository = repository;
        }

        public override async void Refresh()
        {
            var refresh = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var games = await _repository.ListItemsAsync();

                foreach (var game in games)
                {
                    Games.Add(game);
                }
            });

            await refresh.ExecuteAsync();
        }
    }
}
