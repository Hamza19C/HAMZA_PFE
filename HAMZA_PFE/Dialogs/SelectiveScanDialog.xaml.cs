using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Diagnostics;
using Wpf.Ui.Controls;

namespace HAMZA_PFE.Dialogs
{
    public partial class SelectiveScanDialog : FluentWindow
    {
        private HashSet<string> _malwareHashes;
        private CancellationTokenSource _cancellationTokenSource;
        private List<string> _selectedFiles;
        public SelectiveScanDialog(string datasetPath)
        {
            InitializeComponent();
            FileEnScan.Content = "Waiting...";
            Progress.Value = 0;
            ScanResultLabel.Content = "";

            Debug.WriteLine("Initializing SelectiveScanDialog");
            _malwareHashes = LoadMalwareHashes(datasetPath);
            _cancellationTokenSource = new CancellationTokenSource();
            _selectedFiles = new List<string>();
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
                Debug.WriteLine($"Error loading malware dataset: {ex}");
            }
            return malwareHashes;
        }
        private async void ChooseFilesButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a custom MessageBox with "File" and "Folder" options
            var customMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "Select Files or Folder",
                Content = "Do you want to select files or a folder?",
                PrimaryButtonText = "File",
                SecondaryButtonText = "Folder",
                CloseButtonText = "Cancel"
            };
            // Use ShowDialogAsync instead of ShowDialog
            var result = await customMessageBox.ShowDialogAsync();
            if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
            {
                // User chose to select files
                var openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Multiselect = true, // Allow multiple file selection
                    Filter = "All Files (*.*)|*.*", // Filter for all file types
                    Title = "Select Files"
                };
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _selectedFiles = openFileDialog.FileNames.ToList();
                    SelectedFilesListBox.ItemsSource = _selectedFiles;
                }
            }
            else if (result == Wpf.Ui.Controls.MessageBoxResult.Secondary)
            {
                // User chose to select a folder
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog
                {
                    Description = "Select a folder to scan",
                    UseDescriptionForTitle = true
                };

                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string folderPath = folderBrowserDialog.SelectedPath;
                    _selectedFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).ToList();
                    SelectedFilesListBox.ItemsSource = _selectedFiles;
                }
            }
        }
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFiles.Count == 0)
            {
                System.Windows.MessageBox.Show("Please select at least one file or folder to scan.", "No Files Selected", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            Debug.WriteLine("Starting selective scan...");
            StartButton.IsEnabled = false;
            ChooseFilesButton.IsEnabled = false;
            CancelButton.IsEnabled = false;
            FileEnScan.Content = "Scanning...";
            Progress.Value = 0;
            ScanResultLabel.Content = "";
            try
            {
                bool malwareDetected = await RunScanAsync(_selectedFiles, _cancellationTokenSource.Token);

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
                StartButton.IsEnabled = true;
                ChooseFilesButton.IsEnabled = true;
                CancelButton.IsEnabled = true;
                Progress.Value = 100; // Ensure progress bar completes
            }
        }
        private async Task<bool> RunScanAsync(List<string> files, CancellationToken cancellationToken)
        {
            bool malwareDetected = false;
            try
            {
                int totalFiles = files.Count;
                int scannedFiles = 0;

                Debug.WriteLine($"Total files to scan: {totalFiles}");

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

                            // Calculate estimated time remaining
                            var elapsedTime = DateTime.Now - startTime;
                            var estimatedTotalTime = elapsedTime.TotalSeconds / (scannedFiles + 1) * totalFiles;
                            var estimatedTimeRemaining = TimeSpan.FromSeconds(estimatedTotalTime - elapsedTime.TotalSeconds);
                            TimeEstimation.Content = $"{(int)estimatedTimeRemaining.TotalMinutes}m {estimatedTimeRemaining.Seconds}s remaining";
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
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Closing SelectiveScanDialog");
            _cancellationTokenSource.Cancel();
            this.Close();
        }
    }
}