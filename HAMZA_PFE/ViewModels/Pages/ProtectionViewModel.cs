using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAMZA_PFE.ViewModels.Pages
{
    public partial class ProtectionViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isProtected = true;

        [ObservableProperty]
        private bool _isProtectedByIa = true;

        [ObservableProperty]
        private bool _isRealTimeOn = true;

        [RelayCommand]
        private void OnProtectionToggle()
        {
            IsProtected = !IsProtected;
        }

        [RelayCommand]
        private void OnProtectionByIaToggle()
        {
            IsProtectedByIa = !IsProtectedByIa;
        }

        [RelayCommand]
        private void OnRealTimeToggle()
        {
            IsRealTimeOn = !IsRealTimeOn;
        }

    }
}
