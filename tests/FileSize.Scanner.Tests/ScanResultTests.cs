using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSize.Scanner;

namespace FileSize.Scanner.Tests
{
    [TestClass]
    public class ScanResultTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_InitializesProperties()
        {
            // Arrange
            string drive = "C:\\";
            var files = new List<FileInfo>
            {
                new FileInfo(@"C:\file1.txt", 1024 * 1024 * 10), // 10 MB
                new FileInfo(@"C:\file2.txt", 1024 * 1024 * 20), // 20 MB
                new FileInfo(@"C:\file3.txt", 1024 * 1024 * 30)  // 30 MB
            };
            
            // Act
            var scanResult = new ScanResult(drive, files);
            
            // Assert
            Assert.AreEqual(drive, scanResult.Drive, "Drive should match the input");
            Assert.AreEqual(3, scanResult.Files.Count, "Files count should match");
            Assert.AreEqual(1024 * 1024 * 60, scanResult.TotalSizeInBytes, "TotalSizeInBytes should be calculated correctly");
            Assert.AreEqual(60.0, scanResult.TotalSizeInMB, 0.001, "TotalSizeInMB should be calculated correctly");
        }

        [TestMethod]
        public void GetFilesSortedBySize_ReturnsFilesSortedBySize()
        {
            // Arrange
            string drive = "C:\\";
            var files = new List<FileInfo>
            {
                new FileInfo(@"C:\file1.txt", 1024 * 1024 * 10), // 10 MB
                new FileInfo(@"C:\file2.txt", 1024 * 1024 * 30), // 30 MB
                new FileInfo(@"C:\file3.txt", 1024 * 1024 * 20)  // 20 MB
            };
            
            // Act
            var scanResult = new ScanResult(drive, files);
            var sortedFiles = scanResult.GetFilesSortedBySize().ToList();
            
            // Assert
            Assert.AreEqual(3, sortedFiles.Count, "Files count should match");
            Assert.AreEqual(1024 * 1024 * 30, sortedFiles[0].SizeInBytes, "First file should be the largest");
            Assert.AreEqual(1024 * 1024 * 20, sortedFiles[1].SizeInBytes, "Second file should be the second largest");
            Assert.AreEqual(1024 * 1024 * 10, sortedFiles[2].SizeInBytes, "Third file should be the smallest");
        }

        [TestMethod]
        public void GetFilesLargerThan_ReturnsFilesLargerThanSpecifiedSize()
        {
            // Arrange
            string drive = "C:\\";
            var files = new List<FileInfo>
            {
                new FileInfo(@"C:\file1.txt", 1024 * 1024 * 10), // 10 MB
                new FileInfo(@"C:\file2.txt", 1024 * 1024 * 20), // 20 MB
                new FileInfo(@"C:\file3.txt", 1024 * 1024 * 30), // 30 MB
                new FileInfo(@"C:\file4.txt", 1024 * 1024 * 40), // 40 MB
                new FileInfo(@"C:\file5.txt", 1024 * 1024 * 50)  // 50 MB
            };
            
            // Act
            var scanResult = new ScanResult(drive, files);
            var largeFiles = scanResult.GetFilesLargerThan(25).ToList();
            
            // Assert
            Assert.AreEqual(3, largeFiles.Count, "Should return 3 files larger than 25 MB");
            Assert.IsTrue(largeFiles.All(f => f.SizeInMB >= 25), "All files should be larger than 25 MB");
        }
    }
}
