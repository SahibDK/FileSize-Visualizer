using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSize.Display;
using FileSize.Scanner;

namespace FileSize.Display.Tests
{
    [TestClass]
    public class ViewSwitcherTests
    {
        [TestMethod]
        public void Constructor_InitializesWithListViewMode()
        {
            // This test requires a UI thread, so we'll use a simple test
            // that doesn't actually create the control but verifies the logic
            
            // Arrange & Act
            var viewSwitcher = new ViewSwitcher();
            
            // Assert
            Assert.AreEqual(DisplayMode.List, viewSwitcher.CurrentMode, "Default mode should be List");
        }

        [TestMethod]
        public void CurrentMode_WhenChanged_UpdatesMode()
        {
            // Arrange
            var viewSwitcher = new ViewSwitcher();
            
            // Act
            viewSwitcher.CurrentMode = DisplayMode.Treemap;
            
            // Assert
            Assert.AreEqual(DisplayMode.Treemap, viewSwitcher.CurrentMode, "Mode should be updated to Treemap");
            
            // Act again
            viewSwitcher.CurrentMode = DisplayMode.List;
            
            // Assert again
            Assert.AreEqual(DisplayMode.List, viewSwitcher.CurrentMode, "Mode should be updated back to List");
        }
    }
}
