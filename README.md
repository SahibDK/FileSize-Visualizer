# FileSize-Visualizer

A Windows application that scans drives and visualizes file sizes, helping users identify large files (≥ 50MB).

## Features

- Drive selection for scanning
- Two display modes:
  - **List Mode**: Files sorted from largest to smallest
  - **Visual Mode**: Treemap visualization of file sizes
- Filter for files ≥ 50MB
- Simple and intuitive user interface

## Application Architecture

The application is divided into three main modules:

### 1. UI Module

The UI Module handles the user interface components and user interactions.

**Responsibilities:**
- Drive selection dropdown
- Display mode toggle (Visual/List)
- Main application window and layout
- User interaction handling

**Key Components:**
- MainWindow.xaml - Main application window
- DriveSelector.cs - Logic for populating and handling drive selection
- ModeToggle.cs - Logic for switching between visual and list modes

### 2. File Scanner Module

The File Scanner Module is responsible for scanning drives and collecting file information.

**Responsibilities:**
- Drive scanning
- File size calculation
- Filtering files ≥ 50MB
- Collecting file metadata

**Key Components:**
- DriveScanner.cs - Core scanning functionality
- FileInfo.cs - Data structure for file information
- ScanResult.cs - Container for scan results

### 3. Display Module

The Display Module handles the visualization and presentation of scan results.

**Responsibilities:**
- List view implementation
- Treemap visualization
- View switching logic

**Key Components:**
- ListView.cs - Implementation of the list view
- TreemapView.cs - Implementation of the treemap visualization
- ViewSwitcher.cs - Logic for switching between views

## Technology Stack

- **Framework**: .NET 6.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Language**: C#
- **IDE**: Visual Studio
- **Version Control**: Git
- **Testing Framework**: MSTest

## Getting Started

### Prerequisites

- Windows 10 or later
- .NET 6.0 SDK or later
- Visual Studio 2022 or later

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/YourUsername/FileSize-Visualizer.git
   ```

2. Open the solution in Visual Studio:
   ```
   FileSize-Visualizer.sln
   ```

3. Build and run the application.

## Usage

1. Launch the application
2. Select a drive from the dropdown
3. Choose display mode (Visual or List)
4. Click "Scan" to begin scanning
5. View results in the selected display mode

## License

This project is licensed under the MIT License - see the LICENSE file for details.
