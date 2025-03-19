using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileSize.Scanner
{
    /// <summary>
    /// Provides functionality to scan drives for files.
    /// </summary>
    public class DriveScanner
    {
        private const double DefaultMinimumFileSizeMB = 50.0;

        /// <summary>
        /// Gets all available drives on the system.
        /// </summary>
        /// <returns>A collection of drive names.</returns>
        public static IEnumerable<string> GetAvailableDrives()
        {
            return DriveInfo.GetDrives()
                .Where(d => d.IsReady)
                .Select(d => d.Name);
        }

        /// <summary>
        /// Scans a drive for files larger than the specified minimum size.
        /// </summary>
        /// <param name="driveName">The name of the drive to scan.</param>
        /// <param name="minimumFileSizeMB">The minimum file size in megabytes.</param>
        /// <param name="progressCallback">An optional callback to report progress.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ScanResult"/> containing the scan results.</returns>
        public async Task<ScanResult> ScanDriveAsync(
            string driveName,
            double minimumFileSizeMB = DefaultMinimumFileSizeMB,
            IProgress<int>? progressCallback = null,
            CancellationToken cancellationToken = default)
        {
            // Validate drive name
            if (!Directory.Exists(driveName))
            {
                throw new ArgumentException($"Drive {driveName} does not exist or is not accessible.", nameof(driveName));
            }

            // Calculate minimum file size in bytes
            long minimumFileSizeBytes = (long)(minimumFileSizeMB * 1024 * 1024);

            // Scan the drive
            var files = new List<FileInfo>();
            await Task.Run(() =>
            {
                try
                {
                    ScanDirectory(driveName, minimumFileSizeBytes, files, progressCallback, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // Scan was cancelled
                    throw;
                }
                catch (Exception ex)
                {
                    // Log the error but continue with the files we've found so far
                    Console.WriteLine($"Error scanning drive: {ex.Message}");
                }
            }, cancellationToken);

            return new ScanResult(driveName, files);
        }

        private void ScanDirectory(
            string directoryPath,
            long minimumFileSizeBytes,
            List<FileInfo> files,
            IProgress<int>? progressCallback,
            CancellationToken cancellationToken)
        {
            // Check for cancellation
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Get all files in the current directory
                foreach (var filePath in Directory.GetFiles(directoryPath))
                {
                    try
                    {
                        var fileInfo = new System.IO.FileInfo(filePath);
                        if (fileInfo.Length >= minimumFileSizeBytes)
                        {
                            files.Add(FileInfo.FromSystemFileInfo(fileInfo));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error but continue with other files
                        Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
                    }
                }

                // Recursively scan subdirectories
                foreach (var subDirectoryPath in Directory.GetDirectories(directoryPath))
                {
                    try
                    {
                        ScanDirectory(subDirectoryPath, minimumFileSizeBytes, files, progressCallback, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        // Log the error but continue with other directories
                        Console.WriteLine($"Error scanning directory {subDirectoryPath}: {ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Skip directories we don't have access to
            }
            catch (DirectoryNotFoundException)
            {
                // Skip directories that don't exist or were removed during the scan
            }

            // Report progress
            progressCallback?.Report(files.Count);
        }
    }
}
