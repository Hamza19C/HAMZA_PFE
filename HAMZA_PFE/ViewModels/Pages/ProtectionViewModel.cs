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
            string path1 = Path.GetDirectoryName(@"C:\Users\IdeaPad S340\Desktop\HAMZA_PFE\HAMZA_PFE\");
            System.Diagnostics.Debug.WriteLine($"Start quick scan");

            string datasetPath = Path.Combine(path1, "dataset", "full.csv");
            System.Diagnostics.Debug.WriteLine($"Dataset Path: {datasetPath}");

            if (!File.Exists(datasetPath))
            {
                System.Windows.MessageBox.Show($"Dataset file (full.csv) not found at: {datasetPath}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            QuickScanDialog scanDialog = new QuickScanDialog(datasetPath);
            scanDialog.ShowDialog();
        }

        [RelayCommand]
        public void OnFullScan()
        {
            string path1 = Path.GetDirectoryName(@"C:\Users\IdeaPad S340\Desktop\HAMZA_PFE\HAMZA_PFE\");
            string datasetPath = Path.Combine(path1, "dataset", "full.csv");
            System.Diagnostics.Debug.WriteLine($"Dataset Path: {datasetPath}");

            if (!File.Exists(datasetPath))
            {
                System.Windows.MessageBox.Show($"Dataset file (full.csv) not found at: {datasetPath}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            FullScanDialog scanDialog = new FullScanDialog(datasetPath);
            scanDialog.ShowDialog();
        }

        public void OnSelectiveScan()
        {
            string path1 = Path.GetDirectoryName(@"C:\Users\IdeaPad S340\Desktop\HAMZA_PFE\HAMZA_PFE\");
            string datasetPath = Path.Combine(path1, "dataset", "full.csv");
            System.Diagnostics.Debug.WriteLine($"Dataset Path: {datasetPath}");

            if (!File.Exists(datasetPath))
            {
                System.Windows.MessageBox.Show($"Dataset file (full.csv) not found at: {datasetPath}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            SelectiveScanDialog scanDialog = new SelectiveScanDialog(datasetPath);
            scanDialog.ShowDialog();
        }

    }
}

