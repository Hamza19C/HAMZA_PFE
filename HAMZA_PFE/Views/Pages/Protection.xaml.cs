
using HAMZA_PFE.ViewModels.Pages;
using Wpf.Ui.Controls;
using HAMZA_PFE.Dialogs;

namespace HAMZA_PFE.Views.Pages
{
    
    public partial class Protection : INavigableView<ProtectionViewModel>
    {
        public ProtectionViewModel ViewModel { get; }

        public Protection(ProtectionViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window quick_scan = new QuickScanDialog();
            quick_scan.Show();

        }
    }
}
