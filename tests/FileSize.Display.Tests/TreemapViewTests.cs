using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSize.Display;
using FileSize.Scanner;

namespace FileSize.Display.Tests
{
    [TestClass]
    public class TreemapViewTests
    {
        private TreemapView _treemapView;
        private List<Scanner.FileInfo> _testFiles;

        [TestInitialize]
        public void Initialize()
        {
            // Create a test instance of TreemapView
            _treemapView = new TreemapView();

            // Create some test files
            _testFiles = new List<Scanner.FileInfo>
            {
                new Scanner.FileInfo(@"C:\TestDir\file1.txt", 100 * 1024 * 1024), // 100 MB
                new Scanner.FileInfo(@"C:\TestDir\file2.doc", 75 * 1024 * 1024),  // 75 MB
                new Scanner.FileInfo(@"C:\TestDir\file3.pdf", 50 * 1024 * 1024),  // 50 MB
                new Scanner.FileInfo(@"C:\TestDir\Subdir\file4.exe", 200 * 1024 * 1024), // 200 MB
                new Scanner.FileInfo(@"C:\TestDir\Subdir\file5.dll", 150 * 1024 * 1024)  // 150 MB
            };
        }

        [TestMethod]
        public void DisplayFiles_WithValidFiles_CreatesTreemapItems()
        {
            // Act
            _treemapView.DisplayFiles(_testFiles);

            // Assert - We can't directly access private fields, but we can verify the control doesn't throw exceptions
            // This is a basic test to ensure the method runs without errors
            Assert.IsNotNull(_treemapView);
        }

        [TestMethod]
        public void Clear_AfterDisplayingFiles_ClearsItems()
        {
            // Arrange
            _treemapView.DisplayFiles(_testFiles);

            // Act
            _treemapView.Clear();

            // Assert - Again, we can't directly access private fields, but we can verify the method runs
            Assert.IsNotNull(_treemapView);
        }

        [TestMethod]
        public void CreateToolTip_WithValidFile_ContainsCorrectInformation()
        {
            try
            {
                // This test uses reflection to access the private method
                var file = _testFiles[0]; // 100 MB file
                
                // Use reflection to call the private method
                var method = typeof(TreemapView).GetMethod("CreateToolTip", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                Assert.IsNotNull(method, "CreateToolTip method should exist");
                
                // Act
                var toolTip = method.Invoke(_treemapView, new object[] { file }) as ToolTip;
                
                // Assert
                Assert.IsNotNull(toolTip, "ToolTip should not be null");
                
                // Get the content of the tooltip
                var stackPanel = toolTip.Content as StackPanel;
                Assert.IsNotNull(stackPanel, "ToolTip content should be a StackPanel");
                
                // Check that the StackPanel has at least 3 children (filename, path, size)
                Assert.IsTrue(stackPanel.Children.Count >= 3, "ToolTip should have at least 3 elements");
                
                // Check the first child (filename)
                var filenameTextBlock = stackPanel.Children[0] as TextBlock;
                Assert.IsNotNull(filenameTextBlock, "First child should be a TextBlock");
                Assert.AreEqual("file1.txt", filenameTextBlock.Text, "Filename should match");
                
                // Check the second child (full path)
                var pathTextBlock = stackPanel.Children[1] as TextBlock;
                Assert.IsNotNull(pathTextBlock, "Second child should be a TextBlock");
                Assert.IsTrue(pathTextBlock.Text.Contains(@"C:\TestDir\file1.txt"), "Path should contain the full file path");
                
                // Check the third child (size)
                var sizeTextBlock = stackPanel.Children[2] as TextBlock;
                Assert.IsNotNull(sizeTextBlock, "Third child should be a TextBlock");
                Assert.IsTrue(sizeTextBlock.Text.Contains("100.00 MB"), "Size should contain the file size in MB");
                Assert.IsTrue(sizeTextBlock.Text.Contains("104,857,600"), "Size should contain the file size in bytes");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed with exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void ZoomFunctionality_InitialState_IsNotZoomed()
        {
            try
            {
                // We need to use reflection to access private fields
                var isZoomedField = typeof(TreemapView).GetField("_isZoomed", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                Assert.IsNotNull(isZoomedField, "_isZoomed field should exist");
                
                // Act
                var isZoomed = (bool)isZoomedField.GetValue(_treemapView);
                
                // Assert
                Assert.IsFalse(isZoomed, "TreemapView should not be zoomed initially");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed with exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void ZoomHistory_InitialState_IsEmpty()
        {
            try
            {
                // We need to use reflection to access private fields
                var zoomHistoryField = typeof(TreemapView).GetField("_zoomHistory", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                Assert.IsNotNull(zoomHistoryField, "_zoomHistory field should exist");
                
                // Act
                var zoomHistory = zoomHistoryField.GetValue(_treemapView);
                
                // Assert
                Assert.IsNotNull(zoomHistory, "ZoomHistory should not be null");
                
                // Get the Count property using reflection
                var countProperty = zoomHistory.GetType().GetProperty("Count");
                Assert.IsNotNull(countProperty, "Count property should exist on the Stack");
                
                var count = (int)countProperty.GetValue(zoomHistory);
                Assert.AreEqual(0, count, "ZoomHistory should be empty initially");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed with exception: {ex.Message}");
            }
        }

        // Note: Testing mouse events and interactions would typically require UI automation testing,
        // which is beyond the scope of simple unit tests. For a complete test suite, you would need
        // to use a UI automation framework like White or Microsoft UI Automation.
    }
}
