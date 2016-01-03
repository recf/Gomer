using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Gomer.Dto;
using Gomer.Models;
using Gomer.PileGames;
using Microsoft.Practices.Unity;
using Recfab.Infrastructure;

namespace Gomer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            ConfigureUnity(container);
            ConfigureAutoMapper();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.DataContext = container.Resolve<MainWindowViewModel>();
            mainWindow.Show();
        }

        private void ConfigureUnity(IUnityContainer container)
        {
            container.RegisterType<MainWindow>();

            // Models
            container.RegisterType<PileGameModel>();
            container.RegisterType<PlatformModel>();

            // View Models
            container.RegisterType<MainWindowViewModel>();

            var repo = new MemoryRepository<PileGameModel, Guid>(x => x.Id);
            container.RegisterInstance<Repository<PileGameModel, Guid>>(repo);
        }

        private void ConfigureAutoMapper()
        {
            // View Model -> Model
            Mapper.CreateMap<PileGameDetailViewModel, PileGameModel>();

            // Model -> View Model
            Mapper.CreateMap<PileGameModel, PileGameDetailViewModel>()
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.OkCommand, opt => opt.Ignore())
                .ForMember(dest => dest.CancelCommand, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}
