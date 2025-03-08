using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HAMZA_PFE.Dialogs;
using System.IO;

namespace HAMZA_PFE.ViewModels.Pages
{
    public partial class ProtectionViewModel : ObservableObject
    {
        [RelayCommand]
        public void OnQuickScan()
        {
            // Provide the correct path to your dataset (full.csv)
            string datasetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dataset", "full.csv");

            // Log the dataset path to the console for debugging
            Console.WriteLine($"Dataset Path: {datasetPath}");

            // Check if the dataset file exists
            if (!File.Exists(datasetPath))   
            {
                System.Windows.MessageBox.Show("Dataset file (full.csv) not found! Please ensure the file is in the correct directory.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            // Open the scan dialog and pass the dataset path
            QuickScanDialog scanDialog = new QuickScanDialog(datasetPath);
            scanDialog.ShowDialog(); // Show the dialog
        }
    }
}
