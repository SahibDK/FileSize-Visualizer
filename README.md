# FileSize-Visualizer

**Version: 1.0**

A Windows application that scans drives and visualizes file sizes, helping users identify large files (≥ 50MB).

## Latest Updates

- **Enhanced Tooltips**: Hover over any file in the visualization to see detailed information including full file path and size in both MB and bytes.
- **Zoom Functionality**: Click on any file to zoom in and see smaller files in the same directory. Right-click or use the mouse wheel to zoom out.

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

### Installation

#### Option 1: Run the Installer

1. Download the latest release from the releases page
2. Run the installer (`FileSize-Visualizer-Setup.exe`)
3. Follow the installation instructions
4. Launch the application from the desktop shortcut or Start menu

#### Option 2: Run the Executable

1. Download the latest release from the releases page
2. Extract the ZIP file
3. Run `FileSize.UI.exe`

#### Option 3: Build from Source

1. Prerequisites:
   - .NET 9.0 SDK or later
   - Visual Studio 2022 or later

2. Clone the repository:
   ```
   git clone https://github.com/YourUsername/FileSize-Visualizer.git
   ```

3. Open the solution in Visual Studio:
   ```
   FileSize-Visualizer.sln
   ```

4. Build and run the application.

### Building the Executable and Installer

#### Building the Executable

To build a standalone executable:

1. Run the `build-exe.bat` script in the root directory
2. The executable will be created in the `publish` folder
3. Run `run-exe.bat` to launch the application

#### Building the Installer

To build an installer:

1. First build the executable using the steps above
2. Run the `create-portable-zip.bat` script to create a portable ZIP package (recommended)
3. The ZIP package will be created in the `installer` folder as `FileSize-Visualizer-Portable-v1.0.zip`

Alternative installer scripts are also available:
- `create-simple-installer.ps1` - Creates a self-extracting installer
- `create-installer.bat` - Uses Inno Setup (if installed)
- `create-nsis-installer.bat` - Uses NSIS (if installed)

#### Troubleshooting Installation Issues

If you encounter a "This app can't run on your PC" message when trying to run the application:

1. Use the portable ZIP package (`FileSize-Visualizer-Portable-v1.0.zip`) instead of the installer
2. After extracting the ZIP file, right-click on `FileSize.UI.exe`
3. Select Properties
4. Check "Unblock" at the bottom of the Properties dialog
5. Click Apply and OK

## Usage

1. Launch the application
2. Select a drive from the dropdown
3. Choose display mode (Visual or List)
4. Click "Scan" to begin scanning
5. View results in the selected display mode

## License

This project is licensed under a proprietary license. All rights reserved by QualityGate ApS. See the LICENSE file for details.
