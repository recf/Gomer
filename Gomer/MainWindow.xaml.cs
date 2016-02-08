using System.ComponentModel;
using System.Windows;
using Gomer.DataAccess;
using Gomer.DataAccess.Implementation;
using Gomer.Services;

namespace Gomer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AutoMapperConfiguration.Configure();

            var openFileService = new OpenFileService();

            DataContext = new MainWindowViewModel(
                openFileService,
                new DataService(openFileService, new SaveFileService()), 
                new ConfirmationService());
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var vm = (MainWindowViewModel) DataContext;

            e.Cancel = !vm.ConfirmCloseFile();
        }
    }
}
