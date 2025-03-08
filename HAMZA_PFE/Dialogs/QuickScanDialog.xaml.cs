using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;

namespace HAMZA_PFE.Dialogs
{
    public partial class QuickScanDialog : FluentWindow
    {
        private List<string> _malwareHashes; 
        private CancellationTokenSource _cancellationTokenSource;

        public QuickScanDialog(string datasetPath)
        {
            InitializeComponent();
            FileEnScan.Content = "Waiting...";
            TimeEstimation.Content = "Calculating...";
            Progress.Value = 0;

            // Load malware hashes from the dataset (full.csv)
            _malwareHashes = LoadMalwareHashes(datasetPath);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private List<string> LoadMalwareHashes(string datasetPath)
        {
            List<string> malwareHashes = new List<string>();

            try
            {
                // Read all lines from the dataset (full.csv)
                var lines = File.ReadAllLines(datasetPath);

                // Skip the header row and process each line
                foreach (var line in lines.Skip(1))
                {
                    var columns = line.Split(',');

                    // Ensure there are enough columns and the SHA-256 hash is present
                    if (columns.Length > 1)
                    {
                        string sha256Hash = columns[1].Trim('"').ToLowerInvariant();
                        malwareHashes.Add(sha256Hash);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading malware dataset: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            return malwareHashes;
        }

        public async void StartScan(object sender, RoutedEventArgs e)
        {
            // Disable the Start button to prevent multiple clicks
            okButton.IsEnabled = false;
            cancelButton.IsEnabled = true;

            FileEnScan.Content = "Scanning: C:\\";
            Progress.Value = 0;

            try
            {
                // Start the scan
                bool malwareDetected = await RunScanAsync("C:\\", _cancellationTokenSource.Token);

                // Update the UI with the scan result
                FileEnScan.Content = malwareDetected ? "Malware Detected!" : "No Malware Found!";
            }
            catch (OperationCanceledException)
            {
                FileEnScan.Content = "Scan Cancelled!";
            }
            catch (Exception ex)
            {
                FileEnScan.Content = $"Error: {ex.Message}";
            }
            finally
            {
                // Re-enable the Start button
                okButton.IsEnabled = true;
                cancelButton.IsEnabled = false;
            }
        }

        private async Task<bool> RunScanAsync(string rootDirectory, CancellationToken cancellationToken)
        {
            bool malwareDetected = false;

            try
            {
                // Get all files in the directory (and subdirectories)
                var files = SafeEnumerateFiles(rootDirectory);

                int totalFiles = files.Count;
                int scannedFiles = 0;

                await Task.Run(() =>
                {
                    foreach (string filePath in files)
                    {
                        // Check for cancellation
                        cancellationToken.ThrowIfCancellationRequested();

                        // Log the file being scanned to the console
                        Console.WriteLine($"Scanning: {filePath}");

                        // Update the UI with the current file being scanned
                        Dispatcher.Invoke(() =>
                        {
                            ScannedFilesListBox.Items.Add(filePath);
                            Progress.Value = (double)scannedFiles / totalFiles * 100;
                        });

                        try
                        {
                            // Compute the SHA-256 hash of the file
                            string fileHash = ComputeSHA256(filePath);

                            // Check if the hash exists in the malware dataset
                            if (_malwareHashes.Contains(fileHash))
                            {
                                malwareDetected = true;
                                break; // Stop scanning if malware is detected
                            }
                        }
                        catch (UnauthorizedAccessException) { } // Ignore inaccessible files
                        catch (IOException) { } // Ignore I/O errors
                        catch (Exception) { } // Ignore other errors

                        scannedFiles++;
                    }
                }, cancellationToken);
            }
            catch (Exception) { }

            return malwareDetected;
        }

        private List<string> SafeEnumerateFiles(string rootDirectory)
        {
            var files = new List<string>();

            try
            {
                // Get files in the current directory
                foreach (string file in Directory.EnumerateFiles(rootDirectory, "*.*", SearchOption.TopDirectoryOnly))
                {
                    files.Add(file);
                }

                // Recursively get files in subdirectories
                foreach (string subDir in SafeEnumerateDirectories(rootDirectory))
                {
                    files.AddRange(SafeEnumerateFiles(subDir));
                }
            }
            catch (UnauthorizedAccessException) { } // Ignore inaccessible directories
            catch (Exception) { } // Ignore other errors

            return files;
        }

        private IEnumerable<string> SafeEnumerateDirectories(string rootDirectory)
        {
            try
            {
                return Directory.EnumerateDirectories(rootDirectory, "*", SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException) { return Enumerable.Empty<string>(); } // Ignore inaccessible directories
            catch (Exception) { return Enumerable.Empty<string>(); } // Ignore other errors
        }

        private string ComputeSHA256(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            // Cancel the scan if it's running
            _cancellationTokenSource.Cancel();
            this.Close();
        }
    }
}

