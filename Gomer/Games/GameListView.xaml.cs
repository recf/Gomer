using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gomer.Models;

namespace Gomer.Games
{
    /// <summary>
    /// Interaction logic for PileGameListView.xaml
    /// </summary>
    public partial class GameListView : UserControl
    {
        public GameListView()
        {
            InitializeComponent();
        }

        private void ModelsView_OnFilter(object sender, FilterEventArgs e)
        {
            var vm = (GameListViewModel) DataContext;
            var model = (GameModel) e.Item;

            e.Accepted = vm.Filter(model);
        }
    }
}
