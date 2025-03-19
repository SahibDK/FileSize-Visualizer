using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FileSize.Display;
using FileSize.Scanner;

namespace FileSize.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DriveScanner _driveScanner;
        private readonly ViewSwitcher _viewSwitcher;
        private CancellationTokenSource? _cancellationTokenSource;
        private ScanResult? _currentScanResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initialize the drive scanner
            _driveScanner = new DriveScanner();

            // Initialize the view switcher
            _viewSwitcher = new ViewSwitcher();
            ViewContainer.Content = _viewSwitcher;

            // Populate the drive combo box
            PopulateDriveComboBox();

            // Handle view mode changes
            ViewModeComboBox.SelectionChanged += ViewModeComboBox_SelectionChanged;

            // Handle window loaded event
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Select the first drive by default
            if (DriveComboBox.Items.Count > 0)
            {
                DriveComboBox.SelectedIndex = 0;
            }
        }

        private void PopulateDriveComboBox()
        {
            DriveComboBox.Items.Clear();

            foreach (var drive in DriveScanner.GetAvailableDrives())
            {
                DriveComboBox.Items.Add(drive);
            }
        }

        private void ViewModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewSwitcher != null)
            {
                // Update the view mode
                _viewSwitcher.CurrentMode = ViewModeComboBox.SelectedIndex == 0
                    ? DisplayMode.List
                    : DisplayMode.Treemap;

                // Update the view if we have scan results
                if (_currentScanResult != null)
                {
                    _viewSwitcher.DisplayFiles(_currentScanResult.GetFilesLargerThan(50));
                }
            }
        }

        private async void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected drive
            var selectedDrive = DriveComboBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedDrive))
            {
                MessageBox.Show("Please select a drive to scan.", "No Drive Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Cancel any ongoing scan
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Update UI for scanning
                ScanButton.IsEnabled = false;
                DriveComboBox.IsEnabled = false;
                ViewModeComboBox.IsEnabled = false;
                LoadingPanel.Visibility = Visibility.Visible;
                StatusTextBlock.Text = $"Scanning {selectedDrive}...";
                FileCountTextBlock.Text = "0 files";
                TotalSizeTextBlock.Text = "0 MB";

                // Create a progress reporter
                var progress = new Progress<int>(count =>
                {
                    FileCountTextBlock.Text = $"{count} files found";
                });

                // Scan the drive
                _currentScanResult = await _driveScanner.ScanDriveAsync(
                    selectedDrive,
                    50.0, // Minimum file size in MB
                    progress,
                    _cancellationTokenSource.Token);

                // Update the view
                var largeFiles = _currentScanResult.GetFilesLargerThan(50).ToList();
                _viewSwitcher.DisplayFiles(largeFiles);

                // Update status
                StatusTextBlock.Text = $"Scan completed: {selectedDrive}";
                FileCountTextBlock.Text = $"{largeFiles.Count} files";
                TotalSizeTextBlock.Text = $"{_currentScanResult.TotalSizeInMB:F2} MB";
            }
            catch (OperationCanceledException)
            {
                // Scan was cancelled
                StatusTextBlock.Text = "Scan cancelled";
                _viewSwitcher.Clear();
            }
            catch (Exception ex)
            {
                // Handle errors
                MessageBox.Show($"An error occurred while scanning: {ex.Message}", "Scan Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusTextBlock.Text = "Scan failed";
            }
            finally
            {
                // Restore UI
                ScanButton.IsEnabled = true;
                DriveComboBox.IsEnabled = true;
                ViewModeComboBox.IsEnabled = true;
                LoadingPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
