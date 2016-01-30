using System.Windows.Controls;
using System.Windows.Data;
using Gomer.Models;

namespace Gomer.Areas.Games
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
