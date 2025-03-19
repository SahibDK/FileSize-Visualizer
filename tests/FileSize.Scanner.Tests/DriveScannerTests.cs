using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSize.Scanner;

namespace FileSize.Scanner.Tests
{
    [TestClass]
    public class DriveScannerTests
    {
        [TestMethod]
        public void GetAvailableDrives_ReturnsNonEmptyList()
        {
            // Arrange
            
            // Act
            var drives = DriveScanner.GetAvailableDrives().ToList();
            
            // Assert
            Assert.IsTrue(drives.Count > 0, "Should return at least one drive");
            foreach (var drive in drives)
            {
                Assert.IsTrue(Directory.Exists(drive), $"Drive {drive} should exist");
            }
        }

        [TestMethod]
        public async Task ScanDriveAsync_WithValidDrive_ReturnsResults()
        {
            // Arrange
            var scanner = new DriveScanner();
            var drive = DriveScanner.GetAvailableDrives().First();
            
            // Act
            var result = await scanner.ScanDriveAsync(drive, 1.0); // Use 1MB minimum size for testing
            
            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(drive, result.Drive, "Drive should match the input");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ScanDriveAsync_WithInvalidDrive_ThrowsException()
        {
            // Arrange
            var scanner = new DriveScanner();
            var invalidDrive = "Z:\\NonExistentDrive\\";
            
            // Act
            await scanner.ScanDriveAsync(invalidDrive);
            
            // Assert is handled by ExpectedException attribute
        }
    }
}
