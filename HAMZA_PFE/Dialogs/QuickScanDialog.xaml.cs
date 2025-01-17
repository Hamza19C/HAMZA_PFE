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
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics;

namespace HAMZA_PFE.Dialogs
{
    /// <summary>
    /// Interaction logic for QuickScanDialog.xaml
    /// </summary>
    public partial class QuickScanDialog : FluentWindow
    {
        public QuickScanDialog()
        {
            InitializeComponent();
            FileEnScan.Content = ".....";
            TimeEstimation.Content = ".....";
            Progress.Value = 0;

        }

        public async void StartScan(object sender, RoutedEventArgs e)
        {
            FileEnScan.Content = "Scanning: C:\\";
            await RunScanAsync("C:\\", quickScan: false);
            FileEnScan.Content = "\nFull Scan Completed!";

            

        }

        private async Task RunScanAsync(string rootDirectory, bool quickScan)
        {
            var directories = Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories).ToList();
            int totalDirs = directories.Count;
            int progressStep = totalDirs > 0 ? 100 / totalDirs : 0;

            await Task.Run(() =>
            {
                for (int i = 0; i < directories.Count; i++)
                {
                    // Simulate scanning the directory
                    Task.Delay(50).Wait(); // Simulate time taken to scan each directory

                    // Update ProgressBar
                    Dispatcher.Invoke(() =>
                    {
                        Progress.Value += progressStep;
                        FileEnScan.Content = $"Scanning: {directories[i]}";
                    });

                    if (quickScan && i >= totalDirs / 4) break; // Stop early for Quick Scan
                }
            });

            Dispatcher.Invoke(() => Progress.Value = 100);
        }


       





    }
}
