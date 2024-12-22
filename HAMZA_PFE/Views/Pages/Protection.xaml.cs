
using HAMZA_PFE.ViewModels.Pages;
using Wpf.Ui.Controls;
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

    }
}
