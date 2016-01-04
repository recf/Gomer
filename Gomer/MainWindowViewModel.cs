using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using Gomer.Dto;
using Gomer.Models;
using Gomer.PileGames;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Recfab.Infrastructure;

namespace Gomer
{
    public class MainWindowViewModel : ReactiveObject
    {
        private JsonStructuredFileManager<PileDto> _manager;

        private Repository<GameModel, Guid> _repository;

        private ReactiveList<GameModel> _games;

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public ReactiveObject CurrentViewModel { get; set; }

        public ReactiveCommand<object> OpenCommand { get; protected set; }

        public ReactiveCommand<object> SaveCommand { get; protected set; }

        public MainWindowViewModel()
        {
            _repository = new MemoryRepository<GameModel, Guid>(x => x.Id);

            OpenCommand = ReactiveCommand.Create();
            OpenCommand.Subscribe(_ => OpenCommandImpl());

            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ => SaveCommandImpl());

            _games = new ReactiveList<GameModel>();

            Navigate(new PileGameListViewModel(_games), "Games");
        }

        private void Navigate(ViewModel viewModel, string title)
        {
            viewModel.Title = title;
            Title = string.Format("Gomer: {0}", viewModel.Title);

            CurrentViewModel = viewModel;
        }

        private void OpenCommandImpl()
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".pile";
            dialog.Filter = "Gomer Pile File (*.pile)|*.pile";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _manager = new JsonStructuredFileManager<PileDto>(dialog.FileName);
            var pile = _manager.Read();

            var gameModels = Mapper.Map<IList<GameModel>>(pile.Games);

            _games.Clear();
            foreach (var model in gameModels)
            {
                _games.Add(model);
            }
        }

        private async void SaveCommandImpl()
        {
            if (_manager == null)
            {
                var dialog = new SaveFileDialog();
                dialog.DefaultExt = ".pile";
                dialog.Filter = "Gomer Pile File (*.pile)|*.pile";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _manager = new JsonStructuredFileManager<PileDto>(dialog.FileName);
            }

            var gameDtos = _games.Select(Mapper.Map<GameDto>).ToArray();

            var pile = new PileDto();
            pile.Games = gameDtos;

            _manager.Write(pile);
        }
    }
}
