using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSize.UI;

namespace FileSize.UI.Tests
{
    [TestClass]
    public class MainWindowTests
    {
        [TestMethod]
        public void MainWindow_CanBeInitialized()
        {
            // Note: This test is a simple verification that the MainWindow can be instantiated
            // More comprehensive UI testing would typically be done with UI automation frameworks
            
            // Arrange & Act & Assert
            // Just verify that we can create the window without exceptions
            try
            {
                // This will throw an exception if there are issues with the XAML or initialization
                var window = new MainWindow();
                
                // Basic verification
                Assert.IsNotNull(window, "Window should be created");
                Assert.AreEqual("FileSize Visualizer", window.Title, "Window title should be set correctly");
            }
            catch (Exception ex)
            {
                Assert.Fail($"MainWindow initialization failed: {ex.Message}");
            }
        }
    }
}
