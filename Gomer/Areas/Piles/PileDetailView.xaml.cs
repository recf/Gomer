using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gomer.Areas.Piles
{
    /// <summary>
    /// Interaction logic for PileDetailView.xaml
    /// </summary>
    public partial class PileDetailView : UserControl
    {
        public PileDetailView()
        {
            InitializeComponent();
        }

        private ViewModelBase _selectedViewModel;

        private void TabItem_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var tabItem = (TabItem)sender;
            var vm = (ViewModelBase)tabItem.Content;

            if (vm == null || vm == _selectedViewModel)
            {
                return;
            }

            _selectedViewModel = vm;

            vm.Refresh();
        }
    }
}
