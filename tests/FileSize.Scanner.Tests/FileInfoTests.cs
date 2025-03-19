using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSize.Scanner;

namespace FileSize.Scanner.Tests
{
    [TestClass]
    public class FileInfoTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_InitializesProperties()
        {
            // Arrange
            string fullPath = @"C:\test\file.txt";
            long sizeInBytes = 1024 * 1024 * 100; // 100 MB
            
            // Act
            var fileInfo = new FileInfo(fullPath, sizeInBytes);
            
            // Assert
            Assert.AreEqual(fullPath, fileInfo.FullPath, "FullPath should match the input");
            Assert.AreEqual(sizeInBytes, fileInfo.SizeInBytes, "SizeInBytes should match the input");
            Assert.AreEqual("file.txt", fileInfo.Name, "Name should be extracted correctly");
            Assert.AreEqual(@"C:\test", fileInfo.Directory, "Directory should be extracted correctly");
            Assert.AreEqual(100.0, fileInfo.SizeInMB, 0.001, "SizeInMB should be calculated correctly");
        }

        [TestMethod]
        public void FromSystemFileInfo_WithValidFileInfo_CreatesFileInfo()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            try
            {
                // Create a file with some content
                File.WriteAllText(tempFilePath, new string('A', 1000)); // 1000 bytes
                var systemFileInfo = new System.IO.FileInfo(tempFilePath);
                
                // Act
                var fileInfo = FileInfo.FromSystemFileInfo(systemFileInfo);
                
                // Assert
                Assert.AreEqual(systemFileInfo.FullName, fileInfo.FullPath, "FullPath should match");
                Assert.AreEqual(systemFileInfo.Length, fileInfo.SizeInBytes, "SizeInBytes should match");
                Assert.AreEqual(systemFileInfo.Name, fileInfo.Name, "Name should match");
                Assert.AreEqual(systemFileInfo.DirectoryName, fileInfo.Directory, "Directory should match");
            }
            finally
            {
                // Clean up
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
    }
}
