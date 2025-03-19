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
        
        // Zoom functionality
        private TreemapItem? _focusedItem;
        private Stack<TreemapItem> _zoomHistory = new Stack<TreemapItem>();
        private bool _isZoomed = false;
        
        // Status text
        private TextBlock _statusText;

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
            
            // Create status text for zoom instructions
            _statusText = new TextBlock
            {
                Text = "Click on a file to zoom in. Right-click to zoom out.",
                Foreground = Brushes.Black,
                Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255)),
                Padding = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10)
            };
            
            // Create a grid to hold the canvas and status text
            var grid = new Grid();
            grid.Children.Add(_canvas);
            grid.Children.Add(_statusText);

            // Set the content
            Content = grid;

            // Handle size changes
            SizeChanged += OnSizeChanged;
            
            // Handle mouse events for zooming
            _canvas.MouseLeftButtonDown += OnMouseLeftButtonDown;
            _canvas.MouseRightButtonDown += OnMouseRightButtonDown;
            _canvas.MouseWheel += OnMouseWheel;
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

            // Calculate the available area
            var availableWidth = _canvas.ActualWidth > 0 ? _canvas.ActualWidth : 800;
            var availableHeight = _canvas.ActualHeight > 0 ? _canvas.ActualHeight : 600;

            // If we're zoomed in, only show the focused item and its children
            if (_isZoomed && _focusedItem != null)
            {
                // Update status text
                _statusText.Text = $"Zoomed in on: {_focusedItem.File.Name} - Right-click to zoom out";
                
                // Get all files in the same directory as the focused item
                var directory = System.IO.Path.GetDirectoryName(_focusedItem.File.FullPath);
                var itemsInDirectory = _items.Where(i => System.IO.Path.GetDirectoryName(i.File.FullPath) == directory).ToList();
                
                // Calculate total size for this directory
                var dirTotalSize = itemsInDirectory.Sum(i => i.File.SizeInBytes);
                
                // Sort items by size (largest first)
                itemsInDirectory.Sort((a, b) => b.File.SizeInBytes.CompareTo(a.File.SizeInBytes));
                
                // Layout the treemap for this directory
                LayoutSquarified(new Rect(0, 0, availableWidth, availableHeight), itemsInDirectory, dirTotalSize);
            }
            else
            {
                // Reset status text
                _statusText.Text = "Click on a file to zoom in. Right-click to zoom out.";
                
                // Sort items by size (largest first)
                _items.Sort((a, b) => b.File.SizeInBytes.CompareTo(a.File.SizeInBytes));
                
                // Layout the treemap using the squarified algorithm
                LayoutSquarified(new Rect(0, 0, availableWidth, availableHeight), _items, _totalSize);
            }
        }
        
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get the clicked point
            var clickedPoint = e.GetPosition(_canvas);
            
            // Find the rectangle at this point
            var hitTestResult = VisualTreeHelper.HitTest(_canvas, clickedPoint);
            if (hitTestResult?.VisualHit is Rectangle clickedRectangle && clickedRectangle.Tag is TreemapItem item)
            {
                // Set the focused item
                _focusedItem = item;
                
                // Add to zoom history
                _zoomHistory.Push(_focusedItem);
                
                // Set zoomed flag
                _isZoomed = true;
                
                // Relayout the treemap
                LayoutTreemap();
            }
        }
        
        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // If we're zoomed in, zoom out
            if (_isZoomed)
            {
                // Pop the current focused item from the history
                if (_zoomHistory.Count > 0)
                {
                    _zoomHistory.Pop();
                }
                
                // If there's a previous item in the history, set it as the focused item
                if (_zoomHistory.Count > 0)
                {
                    _focusedItem = _zoomHistory.Peek();
                }
                else
                {
                    // If there's no previous item, reset to the full view
                    _focusedItem = null;
                    _isZoomed = false;
                }
                
                // Relayout the treemap
                LayoutTreemap();
            }
        }
        
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Get the mouse position
            var mousePosition = e.GetPosition(_canvas);
            
            // Find the rectangle at this point
            var hitTestResult = VisualTreeHelper.HitTest(_canvas, mousePosition);
            if (hitTestResult?.VisualHit is Rectangle rectangle && rectangle.Tag is TreemapItem item)
            {
                // If scrolling up, zoom in
                if (e.Delta > 0)
                {
                    // Set the focused item
                    _focusedItem = item;
                    
                    // Add to zoom history
                    _zoomHistory.Push(_focusedItem);
                    
                    // Set zoomed flag
                    _isZoomed = true;
                }
                // If scrolling down, zoom out
                else if (e.Delta < 0)
                {
                    // If we're zoomed in, zoom out
                    if (_isZoomed)
                    {
                        // Pop the current focused item from the history
                        if (_zoomHistory.Count > 0)
                        {
                            _zoomHistory.Pop();
                        }
                        
                        // If there's a previous item in the history, set it as the focused item
                        if (_zoomHistory.Count > 0)
                        {
                            _focusedItem = _zoomHistory.Peek();
                        }
                        else
                        {
                            // If there's no previous item, reset to the full view
                            _focusedItem = null;
                            _isZoomed = false;
                        }
                    }
                }
                
                // Relayout the treemap
                LayoutTreemap();
            }
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
                StrokeThickness = 1,
                Tag = item // Store the TreemapItem in the Tag property for easy retrieval
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
                Text = $"Full Path: {file.FullPath}",
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 400
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Size: {file.SizeInMB:F2} MB ({file.SizeInBytes:N0} bytes)"
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
            r = (byte)Math.Max((int)r, 100);
            g = (byte)Math.Max((int)g, 100);
            b = (byte)Math.Max((int)b, 100);

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
