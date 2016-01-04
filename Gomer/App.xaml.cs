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
            ConfigureAutoMapper();

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel();
            mainWindow.Show();
        }

        private void ConfigureAutoMapper()
        {
            // Model -> DTO
            Mapper.CreateMap<GameModel, GameDto>();

            // DTO -> Model
            Mapper.CreateMap<GameDto, GameModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // View Model -> Model

            // Model -> View Model

            Mapper.AssertConfigurationIsValid();
        }
    }
}
