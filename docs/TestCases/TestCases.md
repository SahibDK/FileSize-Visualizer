# FileSize Visualizer Test Cases

This document contains the test cases for the FileSize Visualizer application.

## Scanner Module Tests

### FileInfo Tests

| Test ID | Test Name | Description | Expected Result | Status |
|---------|-----------|-------------|-----------------|--------|
| FI-01 | Constructor_WithValidParameters_InitializesProperties | Verify that the FileInfo constructor initializes properties correctly | All properties should be initialized with the correct values | Passed |
| FI-02 | FromSystemFileInfo_WithValidFileInfo_CreatesFileInfo | Verify that the FromSystemFileInfo method creates a FileInfo object from a System.IO.FileInfo object | The created FileInfo should have properties matching the System.IO.FileInfo | Passed |

### ScanResult Tests

| Test ID | Test Name | Description | Expected Result | Status |
|---------|-----------|-------------|-----------------|--------|
| SR-01 | Constructor_WithValidParameters_InitializesProperties | Verify that the ScanResult constructor initializes properties correctly | All properties should be initialized with the correct values | Passed |
| SR-02 | GetFilesSortedBySize_ReturnsFilesSortedBySize | Verify that the GetFilesSortedBySize method returns files sorted by size | Files should be sorted in descending order by size | Passed |
| SR-03 | GetFilesLargerThan_ReturnsFilesLargerThanSpecifiedSize | Verify that the GetFilesLargerThan method returns files larger than the specified size | Only files larger than the specified size should be returned | Passed |

### DriveScanner Tests

| Test ID | Test Name | Description | Expected Result | Status |
|---------|-----------|-------------|-----------------|--------|
| DS-01 | GetAvailableDrives_ReturnsNonEmptyList | Verify that the GetAvailableDrives method returns a non-empty list of drives | At least one drive should be returned | Passed |
| DS-02 | ScanDriveAsync_WithValidDrive_ReturnsResults | Verify that the ScanDriveAsync method returns results for a valid drive | A ScanResult object should be returned with the correct drive | Passed |
| DS-03 | ScanDriveAsync_WithInvalidDrive_ThrowsException | Verify that the ScanDriveAsync method throws an exception for an invalid drive | An ArgumentException should be thrown | Passed |

## Display Module Tests

### ViewSwitcher Tests

| Test ID | Test Name | Description | Expected Result | Status |
|---------|-----------|-------------|-----------------|--------|
| VS-01 | Constructor_InitializesWithListViewMode | Verify that the ViewSwitcher constructor initializes with List view mode | The CurrentMode property should be set to DisplayMode.List | Passed |
| VS-02 | CurrentMode_WhenChanged_UpdatesMode | Verify that changing the CurrentMode property updates the mode | The CurrentMode property should be updated to the new value | Passed |

## UI Module Tests

### MainWindow Tests

| Test ID | Test Name | Description | Expected Result | Status |
|---------|-----------|-------------|-----------------|--------|
| MW-01 | MainWindow_CanBeInitialized | Verify that the MainWindow can be initialized | The window should be created without exceptions | Passed |

## Integration Tests

| Test ID | Test Name | Description | Expected Result | Status |
|---------|-----------|-------------|-----------------|--------|
| IT-01 | ScanDrive_DisplayResults_List | Verify that scanning a drive and displaying results in List mode works correctly | Files should be displayed in a list sorted by size | Not Run |
| IT-02 | ScanDrive_DisplayResults_Treemap | Verify that scanning a drive and displaying results in Treemap mode works correctly | Files should be displayed in a treemap visualization | Not Run |
| IT-03 | SwitchMode_UpdatesDisplay | Verify that switching between List and Treemap modes updates the display | The display should change to reflect the selected mode | Not Run |

## System Tests

| Test ID | Test Name | Description | Expected Result | Status |
|---------|-----------|-------------|-----------------|--------|
| ST-01 | Application_Startup | Verify that the application starts up correctly | The application should start without errors and display the main window | Not Run |
| ST-02 | Application_ScanDrive | Verify that the application can scan a drive and display results | The application should scan the drive and display the results | Not Run |
| ST-03 | Application_SwitchMode | Verify that the application can switch between List and Treemap modes | The application should switch between modes correctly | Not Run |
