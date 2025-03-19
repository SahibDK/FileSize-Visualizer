using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FileSize.Scanner;

namespace FileSize.Display
{
    /// <summary>
    /// Provides functionality to switch between different views.
    /// </summary>
    public class ViewSwitcher : UserControl
    {
        private readonly Grid _grid;
        private readonly ListView _listView;
        private readonly TreemapView _treemapView;
        private DisplayMode _currentMode;

        /// <summary>
        /// Gets or sets the current display mode.
        /// </summary>
        public DisplayMode CurrentMode
        {
            get => _currentMode;
            set
            {
                if (_currentMode != value)
                {
                    _currentMode = value;
                    UpdateView();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewSwitcher"/> class.
        /// </summary>
        public ViewSwitcher()
        {
            // Create the grid
            _grid = new Grid();

            // Create the views
            _listView = new ListView();
            _treemapView = new TreemapView();

            // Add the views to the grid
            _grid.Children.Add(_listView);
            _grid.Children.Add(_treemapView);

            // Set the content
            Content = _grid;

            // Set the default mode
            _currentMode = DisplayMode.List;
            UpdateView();
        }

        /// <summary>
        /// Displays the specified files in the current view.
        /// </summary>
        /// <param name="files">The files to display.</param>
        public void DisplayFiles(IEnumerable<Scanner.FileInfo> files)
        {
            switch (_currentMode)
            {
                case DisplayMode.List:
                    _listView.DisplayFiles(files);
                    break;
                case DisplayMode.Treemap:
                    _treemapView.DisplayFiles(files);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Clears the current view.
        /// </summary>
        public void Clear()
        {
            _listView.Clear();
            _treemapView.Clear();
        }

        private void UpdateView()
        {
            switch (_currentMode)
            {
                case DisplayMode.List:
                    _listView.Visibility = Visibility.Visible;
                    _treemapView.Visibility = Visibility.Collapsed;
                    break;
                case DisplayMode.Treemap:
                    _listView.Visibility = Visibility.Collapsed;
                    _treemapView.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    /// <summary>
    /// Specifies the display mode.
    /// </summary>
    public enum DisplayMode
    {
        /// <summary>
        /// List view mode.
        /// </summary>
        List,

        /// <summary>
        /// Treemap view mode.
        /// </summary>
        Treemap
    }
}
