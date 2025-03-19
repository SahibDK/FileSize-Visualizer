using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FileSize.Scanner;

namespace FileSize.Display
{
    /// <summary>
    /// Provides a list view for displaying file information.
    /// </summary>
    public class ListView : UserControl
    {
        private readonly DataGrid _dataGrid;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ListView()
        {
            // Create the data grid
            _dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                IsReadOnly = true,
                SelectionMode = DataGridSelectionMode.Single,
                SelectionUnit = DataGridSelectionUnit.FullRow,
                CanUserSortColumns = true,
                CanUserResizeColumns = true,
                CanUserReorderColumns = true,
                AlternatingRowBackground = System.Windows.Media.Brushes.AliceBlue,
                HorizontalGridLinesBrush = System.Windows.Media.Brushes.LightGray,
                VerticalGridLinesBrush = System.Windows.Media.Brushes.LightGray
            };

            // Add columns
            _dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Name",
                Binding = new Binding("Name"),
                Width = new DataGridLength(300)
            });

            _dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Directory",
                Binding = new Binding("Directory"),
                Width = new DataGridLength(400)
            });

            _dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Size (MB)",
                Binding = new Binding("SizeInMB") { StringFormat = "F2" },
                Width = new DataGridLength(100)
            });

            // Set the content
            Content = _dataGrid;
        }

        /// <summary>
        /// Displays the specified files in the list view.
        /// </summary>
        /// <param name="files">The files to display.</param>
        public void DisplayFiles(IEnumerable<Scanner.FileInfo> files)
        {
            _dataGrid.ItemsSource = files;
        }

        /// <summary>
        /// Clears the list view.
        /// </summary>
        public void Clear()
        {
            _dataGrid.ItemsSource = null;
        }
    }
}
