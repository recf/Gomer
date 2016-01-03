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
using Microsoft.Practices.Unity;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Recfab.Infrastructure;

namespace Gomer
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly IUnityContainer _container;

        private Repository<PileGameModel, Guid> _repository;

        private Stack<ViewModel> _navigationStack;

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public ReactiveObject CurrentViewModel { get; set; }

        public ReactiveCommand<object> OpenCommand { get; protected set; }

        public ReactiveCommand<object> SaveCommand { get; protected set; }

        public ReactiveCommand<object> AddCommand { get; protected set; }

        public MainWindowViewModel(IUnityContainer container, Repository<PileGameModel, Guid> repository)
        {
            _container = container;
            _repository = repository;
            _navigationStack = new Stack<ViewModel>();

            OpenCommand = ReactiveCommand.Create();
            OpenCommand.Subscribe(_ => OpenCommandImpl());

            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ => SaveCommandImpl());

            AddCommand = ReactiveCommand.Create();
            AddCommand.Subscribe(_ => AddCommandImpl());
            
            Navigate(_container.Resolve<PileGameListViewModel>(), "Games");
        }

        private void Navigate(ViewModel viewModel, string title)
        {
            viewModel.Title = title;
            Title = string.Format("Gomer: {0}", viewModel.Title);

            _navigationStack.Push(viewModel);
            CurrentViewModel = viewModel;
            viewModel.Refresh();
        }

        private void NavigateBack(bool changed)
        {
            _navigationStack.Pop();
            var viewModel = _navigationStack.Peek();
            Title = string.Format("Gomer: {0}", viewModel.Title);

            CurrentViewModel = viewModel;

            if (changed)
            {
                viewModel.Refresh();
            }
        }

        private void OpenCommandImpl()
        {
            throw new NotImplementedException();
        }

        private async void SaveCommandImpl()
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".pile";
            dialog.Filter = "Gomer Pile File (*.pile)|*.pile";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var manager = new JsonStructuredFileManager<Pile>(dialog.FileName);

                var gameModels = await _repository.ListItemsAsync();
                var gameDtos = gameModels.Select(Mapper.Map<PileGame>).ToArray();

                var pile = new Pile();
                pile.Games = gameDtos;

                manager.Write(pile);
            }
        }

        private void AddCommandImpl()
        {
            var vm = _container.Resolve<PileGameDetailViewModel>();
            vm.Done.Subscribe(NavigateBack);

            Navigate(vm, "Add Game");
        }
    }
}
