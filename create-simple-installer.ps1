# PowerShell script to create a simple installer for FileSize Visualizer

# Create installer directory if it doesn't exist
$installerDir = Join-Path -Path $PSScriptRoot -ChildPath "installer"
if (-not (Test-Path -Path $installerDir)) {
    New-Item -Path $installerDir -ItemType Directory | Out-Null
}

# Path to the installer
$installerPath = Join-Path -Path $installerDir -ChildPath "FileSize-Visualizer-Setup.exe"

# Create a self-extracting installer using PowerShell
$publishDir = Join-Path -Path $PSScriptRoot -ChildPath "publish"
$licenseFile = Join-Path -Path $PSScriptRoot -ChildPath "LICENSE"

# Create a batch file that will be included in the installer
$installBatchContent = @"
@echo off
echo Installing FileSize Visualizer...

REM Create program directory
if not exist "%ProgramFiles%\FileSize Visualizer" mkdir "%ProgramFiles%\FileSize Visualizer"

REM Copy files
xcopy /E /I /Y "%~dp0Files\*.*" "%ProgramFiles%\FileSize Visualizer"

REM Create shortcuts
powershell -Command "$ws = New-Object -ComObject WScript.Shell; $shortcut = $ws.CreateShortcut('%USERPROFILE%\Desktop\FileSize Visualizer.lnk'); $shortcut.TargetPath = '%ProgramFiles%\FileSize Visualizer\FileSize.UI.exe'; $shortcut.Save()"
powershell -Command "$ws = New-Object -ComObject WScript.Shell; $shortcut = $ws.CreateShortcut('%ProgramData%\Microsoft\Windows\Start Menu\Programs\FileSize Visualizer.lnk'); $shortcut.TargetPath = '%ProgramFiles%\FileSize Visualizer\FileSize.UI.exe'; $shortcut.Save()"

REM Create uninstaller
echo @echo off > "%ProgramFiles%\FileSize Visualizer\uninstall.bat"
echo echo Uninstalling FileSize Visualizer... >> "%ProgramFiles%\FileSize Visualizer\uninstall.bat"
echo rmdir /S /Q "%ProgramFiles%\FileSize Visualizer" >> "%ProgramFiles%\FileSize Visualizer\uninstall.bat"
echo del "%USERPROFILE%\Desktop\FileSize Visualizer.lnk" >> "%ProgramFiles%\FileSize Visualizer\uninstall.bat"
echo del "%ProgramData%\Microsoft\Windows\Start Menu\Programs\FileSize Visualizer.lnk" >> "%ProgramFiles%\FileSize Visualizer\uninstall.bat"
echo echo Uninstallation complete. >> "%ProgramFiles%\FileSize Visualizer\uninstall.bat"

REM Create uninstaller shortcut
powershell -Command "$ws = New-Object -ComObject WScript.Shell; $shortcut = $ws.CreateShortcut('%ProgramData%\Microsoft\Windows\Start Menu\Programs\Uninstall FileSize Visualizer.lnk'); $shortcut.TargetPath = '%ProgramFiles%\FileSize Visualizer\uninstall.bat'; $shortcut.Save()"

echo Installation complete!
echo.
echo FileSize Visualizer has been installed to "%ProgramFiles%\FileSize Visualizer"
echo Shortcuts have been created on the desktop and in the Start Menu.
echo.
echo Press any key to launch the application...
pause > nul
start "" "%ProgramFiles%\FileSize Visualizer\FileSize.UI.exe"
"@

# Create a temporary directory for the installer files
$tempDir = Join-Path -Path $env:TEMP -ChildPath ([System.Guid]::NewGuid().ToString())
New-Item -Path $tempDir -ItemType Directory | Out-Null

# Create the Files directory
$filesDir = Join-Path -Path $tempDir -ChildPath "Files"
New-Item -Path $filesDir -ItemType Directory | Out-Null

# Copy the publish files to the Files directory
Copy-Item -Path "$publishDir\*" -Destination $filesDir -Recurse

# Copy the license file
Copy-Item -Path $licenseFile -Destination $filesDir

# Create the install.bat file
$installBatchPath = Join-Path -Path $tempDir -ChildPath "install.bat"
$installBatchContent | Out-File -FilePath $installBatchPath -Encoding ASCII

# Create a self-extracting archive using 7-Zip if available
$sevenZipPath = "C:\Program Files\7-Zip\7z.exe"
if (Test-Path $sevenZipPath) {
    Write-Host "Creating installer using 7-Zip..."
    
    # Create a SFX config file
    $sfxConfigPath = Join-Path -Path $tempDir -ChildPath "sfx_config.txt"
    @"
;!@Install@!UTF-8!
Title="FileSize Visualizer Installer"
BeginPrompt="Do you want to install FileSize Visualizer?"
RunProgram="install.bat"
;!@InstallEnd@!
"@ | Out-File -FilePath $sfxConfigPath -Encoding ASCII
    
    # Create a temporary archive
    $tempArchive = Join-Path -Path $tempDir -ChildPath "temp.7z"
    & $sevenZipPath a -t7z $tempArchive "$tempDir\*" -r -mx=9
    
    # Create the self-extracting archive
    $sfxModule = Join-Path -Path (Split-Path $sevenZipPath) -ChildPath "7z.sfx"
    if (Test-Path $sfxModule) {
        Get-Content $sfxConfigPath, $tempArchive -Encoding Byte -ReadCount 0 | Set-Content $installerPath -Encoding Byte
        Write-Host "Installer created successfully at: $installerPath"
    } else {
        Write-Host "SFX module not found. Creating a simple ZIP installer instead."
        # Fall back to simple ZIP
        Compress-Archive -Path "$tempDir\*" -DestinationPath "$installerDir\FileSize-Visualizer-Files.zip" -Force
        Write-Host "ZIP archive created at: $installerDir\FileSize-Visualizer-Files.zip"
    }
} else {
    Write-Host "7-Zip not found. Creating a simple ZIP installer instead."
    # Create a simple ZIP file
    Compress-Archive -Path "$tempDir\*" -DestinationPath "$installerDir\FileSize-Visualizer-Files.zip" -Force
    Write-Host "ZIP archive created at: $installerDir\FileSize-Visualizer-Files.zip"
    
    # Create a simple batch file to extract and run the installer
    $launcherPath = Join-Path -Path $installerDir -ChildPath "Install-FileSize-Visualizer.bat"
    @"
@echo off
echo Extracting FileSize Visualizer installer...
powershell -Command "Expand-Archive -Path '%~dp0FileSize-Visualizer-Files.zip' -DestinationPath '%TEMP%\FileSize-Visualizer-Install' -Force"
echo Running installer...
call "%TEMP%\FileSize-Visualizer-Install\install.bat"
"@ | Out-File -FilePath $launcherPath -Encoding ASCII
    
    Write-Host "Launcher created at: $launcherPath"
}

# Clean up
Remove-Item -Path $tempDir -Recurse -Force

Write-Host "Installer creation complete."
