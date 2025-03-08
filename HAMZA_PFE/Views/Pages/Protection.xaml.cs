using HAMZA_PFE.ViewModels.Pages;
using Wpf.Ui.Controls;
using System.Windows;

namespace HAMZA_PFE.Views.Pages
{
    public partial class Protection : INavigableView<ProtectionViewModel>
    {
        public ProtectionViewModel ViewModel { get; }

        public Protection(ProtectionViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnQuickScan(); // Opens the scan dialog
        }
    }
}


