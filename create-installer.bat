@echo off
echo Creating FileSize Visualizer Installer...

REM Create the installer directory if it doesn't exist
if not exist installer mkdir installer

REM Check if Inno Setup is installed
where iscc >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Inno Setup Compiler (iscc) not found.
    echo Please install Inno Setup from https://jrsoftware.org/isdl.php
    echo or run the downloaded innosetup-installer.exe
    pause
    exit /b 1
)

REM Compile the installer
iscc FileSize-Installer.iss

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Installer created successfully!
    echo Installer location: %CD%\installer\FileSize-Visualizer-Setup.exe
    echo.
) else (
    echo.
    echo Failed to create installer.
    echo.
)

pause
