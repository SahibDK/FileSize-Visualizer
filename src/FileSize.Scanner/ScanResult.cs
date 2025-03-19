using System;
using System.Collections.Generic;
using System.Linq;

namespace FileSize.Scanner
{
    /// <summary>
    /// Represents the result of a drive scan operation.
    /// </summary>
    public class ScanResult
    {
        /// <summary>
        /// Gets the drive that was scanned.
        /// </summary>
        public string Drive { get; }

        /// <summary>
        /// Gets the collection of files found during the scan.
        /// </summary>
        public IReadOnlyList<FileInfo> Files { get; }

        /// <summary>
        /// Gets the total size of all files in bytes.
        /// </summary>
        public long TotalSizeInBytes { get; }

        /// <summary>
        /// Gets the total size of all files in megabytes.
        /// </summary>
        public double TotalSizeInMB => TotalSizeInBytes / (1024.0 * 1024.0);

        /// <summary>
        /// Gets the timestamp when the scan was completed.
        /// </summary>
        public DateTime ScanTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanResult"/> class.
        /// </summary>
        /// <param name="drive">The drive that was scanned.</param>
        /// <param name="files">The collection of files found during the scan.</param>
        public ScanResult(string drive, IEnumerable<FileInfo> files)
        {
            Drive = drive;
            Files = files.OrderByDescending(f => f.SizeInBytes).ToList();
            TotalSizeInBytes = Files.Sum(f => f.SizeInBytes);
            ScanTime = DateTime.Now;
        }

        /// <summary>
        /// Gets the files sorted by size in descending order.
        /// </summary>
        /// <returns>The files sorted by size.</returns>
        public IEnumerable<FileInfo> GetFilesSortedBySize()
        {
            return Files;
        }

        /// <summary>
        /// Gets the files filtered by minimum size.
        /// </summary>
        /// <param name="minimumSizeMB">The minimum size in megabytes.</param>
        /// <returns>The files filtered by minimum size.</returns>
        public IEnumerable<FileInfo> GetFilesLargerThan(double minimumSizeMB)
        {
            return Files.Where(f => f.SizeInMB >= minimumSizeMB);
        }
    }
}
