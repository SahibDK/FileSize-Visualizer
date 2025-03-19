using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FileSize.Scanner;

namespace FileSize.Display
{
    /// <summary>
    /// Provides a treemap visualization for displaying file sizes.
    /// </summary>
    public class TreemapView : UserControl
    {
        private readonly Canvas _canvas;
        private readonly ToolTip _toolTip;
        private readonly List<TreemapItem> _items = new();
        private double _totalSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreemapView"/> class.
        /// </summary>
        public TreemapView()
        {
            // Create the canvas
            _canvas = new Canvas
            {
                Background = Brushes.White
            };

            // Create the tooltip
            _toolTip = new ToolTip
            {
                Content = string.Empty,
                Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse
            };

            // Set the content
            Content = _canvas;

            // Handle size changes
            SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// Displays the specified files in the treemap view.
        /// </summary>
        /// <param name="files">The files to display.</param>
        public void DisplayFiles(IEnumerable<Scanner.FileInfo> files)
        {
            // Clear existing items
            _items.Clear();
            _canvas.Children.Clear();

            // Create treemap items
            var filesList = files.ToList();
            _totalSize = filesList.Sum(f => f.SizeInBytes);

            foreach (var file in filesList)
            {
                var item = new TreemapItem(file);
                _items.Add(item);
            }

            // Layout the treemap
            LayoutTreemap();
        }

        /// <summary>
        /// Clears the treemap view.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
            _canvas.Children.Clear();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            LayoutTreemap();
        }

        private void LayoutTreemap()
        {
            if (_items.Count == 0 || _totalSize == 0)
                return;

            _canvas.Children.Clear();

            // Sort items by size (largest first)
            _items.Sort((a, b) => b.File.SizeInBytes.CompareTo(a.File.SizeInBytes));

            // Calculate the available area
            var availableWidth = _canvas.ActualWidth > 0 ? _canvas.ActualWidth : 800;
            var availableHeight = _canvas.ActualHeight > 0 ? _canvas.ActualHeight : 600;

            // Layout the treemap using the squarified algorithm
            LayoutSquarified(new Rect(0, 0, availableWidth, availableHeight), _items, _totalSize);
        }

        private void LayoutSquarified(Rect rect, List<TreemapItem> items, double totalSize)
        {
            if (items.Count == 0)
                return;

            // For simplicity, we'll use a basic slice-and-dice algorithm
            // A more advanced implementation would use the squarified treemap algorithm

            double x = rect.X;
            double y = rect.Y;
            double remainingWidth = rect.Width;
            double remainingHeight = rect.Height;

            foreach (var item in items)
            {
                // Calculate the item's area based on its proportion of the total size
                double proportion = item.File.SizeInBytes / totalSize;
                double area = proportion * rect.Width * rect.Height;

                // Determine whether to slice horizontally or vertically
                if (remainingWidth >= remainingHeight)
                {
                    // Slice horizontally
                    double width = area / remainingHeight;
                    if (width > remainingWidth)
                        width = remainingWidth;

                    CreateRectangle(item, x, y, width, remainingHeight);
                    x += width;
                    remainingWidth -= width;
                }
                else
                {
                    // Slice vertically
                    double height = area / remainingWidth;
                    if (height > remainingHeight)
                        height = remainingHeight;

                    CreateRectangle(item, x, y, remainingWidth, height);
                    y += height;
                    remainingHeight -= height;
                }
            }
        }

        private void CreateRectangle(TreemapItem item, double x, double y, double width, double height)
        {
            if (width <= 0 || height <= 0)
                return;

            // Create the rectangle
            var rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = GetColorForFile(item.File),
                Stroke = Brushes.White,
                StrokeThickness = 1
            };

            // Set the position
            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);

            // Add tooltip
            rectangle.ToolTip = CreateToolTip(item.File);

            // Add to canvas
            _canvas.Children.Add(rectangle);
        }

        private ToolTip CreateToolTip(Scanner.FileInfo file)
        {
            var toolTip = new ToolTip();
            var stackPanel = new StackPanel();

            stackPanel.Children.Add(new TextBlock
            {
                Text = file.Name,
                FontWeight = FontWeights.Bold
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Directory: {file.Directory}"
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Size: {file.SizeInMB:F2} MB"
            });

            toolTip.Content = stackPanel;
            return toolTip;
        }

        private Brush GetColorForFile(Scanner.FileInfo file)
        {
            // Generate a color based on the file extension
            string extension = System.IO.Path.GetExtension(file.Name).ToLowerInvariant();
            
            // Simple hash function to generate a color
            int hash = extension.GetHashCode();
            byte r = (byte)((hash & 0xFF0000) >> 16);
            byte g = (byte)((hash & 0x00FF00) >> 8);
            byte b = (byte)(hash & 0x0000FF);

            // Ensure the color is not too dark or too light
            r = (byte)Math.Max(r, 100);
            g = (byte)Math.Max(g, 100);
            b = (byte)Math.Max(b, 100);

            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        private class TreemapItem
        {
            public Scanner.FileInfo File { get; }

            public TreemapItem(Scanner.FileInfo file)
            {
                File = file;
            }
        }
    }
}
