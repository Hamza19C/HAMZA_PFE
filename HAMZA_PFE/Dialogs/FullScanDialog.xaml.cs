using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Diagnostics;
using Wpf.Ui.Controls;

namespace HAMZA_PFE.Dialogs
{
    public partial class FullScanDialog : FluentWindow
    {
        private HashSet<string> _malwareHashes;
        private CancellationTokenSource _cancellationTokenSource;

        public FullScanDialog(string datasetPath)
        {
            InitializeComponent();
            FileEnScan.Content = "Waiting...";
            TimeEstimation.Content = "Calculating...";
            ScanResultLabel.Content = "";
            Progress.Value = 0;

            Debug.WriteLine("Initializing QuickScanDialog");
            _malwareHashes = LoadMalwareHashes(datasetPath);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private HashSet<string> LoadMalwareHashes(string datasetPath)
        {
            var malwareHashes = new HashSet<string>();

            try
            {
                using (var reader = new StreamReader(datasetPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    BadDataFound = null,
                    MissingFieldFound = null
                }))
                {
                    while (csv.Read())
                    {
                        var hash = csv.GetField<string>(1)?.Trim().ToLowerInvariant();
                        if (!string.IsNullOrEmpty(hash))
                        {
                            malwareHashes.Add(hash);
                        }
                    }
                }
                Debug.WriteLine($"Loaded {malwareHashes.Count} malware hashes.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading malware dataset: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                Debug.WriteLine($"Error loading malware dataset: {ex.Message}");
            }

            return malwareHashes;
        }

        public async void StartScan(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Starting scan...");
            okButton.IsEnabled = false; // Disable Start button
            cancelButton.IsEnabled = false; // Disable Cancel button during scan
            FileEnScan.Content = "Scanning...";
            Progress.Value = 0;
            ScanResultLabel.Content = "";

            try
            {
                string scanDirectory = "C:\\Users\\IdeaPad S340\\Desktop\\3L\\GE";
                bool malwareDetected = await RunScanAsync(scanDirectory, _cancellationTokenSource.Token);

                ScanResultLabel.Content = malwareDetected ? "🚨 Malware Detected!" : "✅ No Malware Found!";
                ScanResultLabel.Foreground = malwareDetected ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Green;

                Debug.WriteLine(malwareDetected ? "Malware Detected!" : "No Malware Found!");
            }
            catch (OperationCanceledException)
            {
                FileEnScan.Content = "Scan Cancelled!";
                ScanResultLabel.Content = "Scan was cancelled!";
                Debug.WriteLine("Scan Cancelled!");
            }
            catch (Exception ex)
            {
                FileEnScan.Content = "Error during scan!";
                ScanResultLabel.Content = $"Error: {ex.Message}";
                Debug.WriteLine($"Error during scan: {ex.Message}");
            }
            finally
            {
                okButton.IsEnabled = true; // Re-enable Start button
                cancelButton.IsEnabled = true; // Re-enable Cancel button after scan
            }
        }

        private async Task<bool> RunScanAsync(string rootDirectory, CancellationToken cancellationToken)
        {
            bool malwareDetected = false;

            try
            {
                var files = SafeEnumerateFiles(rootDirectory);
                int totalFiles = files.Count;
                int scannedFiles = 0;

                Debug.WriteLine($"Total files found: {totalFiles}");

                var startTime = DateTime.Now;

                await Task.Run(() =>
                {
                    foreach (string filePath in files)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        Debug.WriteLine($"Scanning: {filePath}");

                        Dispatcher.Invoke(() =>
                        {
                            FileEnScan.Content = filePath;
                            Progress.Value = (double)scannedFiles / totalFiles * 100;

                            var elapsedTime = DateTime.Now - startTime;
                            var timePerFile = elapsedTime.TotalSeconds / (scannedFiles + 1);
                            var estimatedTotalTime = timePerFile * totalFiles;
                            var estimatedTimeRemaining = TimeSpan.FromSeconds(estimatedTotalTime - elapsedTime.TotalSeconds);

                            TimeEstimation.Content = $" {estimatedTimeRemaining:mm\\:ss}";
                        });

                        try
                        {
                            string fileHash = ComputeSHA256(filePath);
                            if (_malwareHashes.Contains(fileHash))
                            {
                                Debug.WriteLine($"Malware detected in: {filePath}");
                                malwareDetected = true;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error scanning file {filePath}: {ex.Message}");
                        }

                        scannedFiles++;
                    }
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RunScanAsync: {ex.Message}");
            }

            return malwareDetected;
        }

        private List<string> SafeEnumerateFiles(string rootDirectory)
        {
            var files = new List<string>();

            try
            {
                foreach (string file in Directory.EnumerateFiles(rootDirectory, "*.*", SearchOption.AllDirectories))
                {
                    files.Add(file);
                }
                Debug.WriteLine($"Enumerated {files.Count} files in {rootDirectory}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error enumerating files: {ex.Message}");
            }

            return files;
        }

        private string ComputeSHA256(string filePath)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                    Debug.WriteLine($"Computed SHA-256 for {filePath}: {hash}");
                    return hash;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error computing SHA-256 for {filePath}: {ex.Message}");
                throw;
            }
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Closing QuickScanDialog");
            _cancellationTokenSource.Cancel();
            this.Close();
        }
    }
}
