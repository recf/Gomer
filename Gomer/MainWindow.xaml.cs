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

            DataContext = new MainWindowViewModel(
                new DataService(new OpenFileService(), new SaveFileService()), 
                new ConfirmationService());
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var vm = (MainWindowViewModel) DataContext;

            e.Cancel = !vm.ConfirmCloseFile();
        }
    }
}
