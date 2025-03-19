using System;
using System.IO;

namespace FileSize.Scanner
{
    /// <summary>
    /// Represents information about a file, including its path and size.
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// Gets the full path of the file.
        /// </summary>
        public string FullPath { get; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the size of the file in bytes.
        /// </summary>
        public long SizeInBytes { get; }

        /// <summary>
        /// Gets the size of the file in megabytes.
        /// </summary>
        public double SizeInMB => SizeInBytes / (1024.0 * 1024.0);

        /// <summary>
        /// Gets the directory containing the file.
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfo"/> class.
        /// </summary>
        /// <param name="fullPath">The full path of the file.</param>
        /// <param name="sizeInBytes">The size of the file in bytes.</param>
        public FileInfo(string fullPath, long sizeInBytes)
        {
            FullPath = fullPath;
            SizeInBytes = sizeInBytes;
            Name = Path.GetFileName(fullPath);
            Directory = Path.GetDirectoryName(fullPath) ?? string.Empty;
        }

        /// <summary>
        /// Creates a new <see cref="FileInfo"/> instance from a <see cref="System.IO.FileInfo"/> object.
        /// </summary>
        /// <param name="fileInfo">The <see cref="System.IO.FileInfo"/> object.</param>
        /// <returns>A new <see cref="FileInfo"/> instance.</returns>
        public static FileInfo FromSystemFileInfo(System.IO.FileInfo fileInfo)
        {
            return new FileInfo(fileInfo.FullName, fileInfo.Length);
        }
    }
}
