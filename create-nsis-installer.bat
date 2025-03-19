@echo off
echo Creating FileSize Visualizer Installer using NSIS...

REM Create the installer directory if it doesn't exist
if not exist installer mkdir installer

REM Check if NSIS is installed
where makensis >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo NSIS (makensis) not found.
    echo Please install NSIS from https://nsis.sourceforge.io/Download
    
    REM Try to find NSIS in common installation locations
    if exist "C:\Program Files (x86)\NSIS\makensis.exe" (
        echo Found NSIS at C:\Program Files (x86)\NSIS\makensis.exe
        set NSIS_PATH="C:\Program Files (x86)\NSIS\makensis.exe"
        goto BuildInstaller
    )
    
    if exist "C:\Program Files\NSIS\makensis.exe" (
        echo Found NSIS at C:\Program Files\NSIS\makensis.exe
        set NSIS_PATH="C:\Program Files\NSIS\makensis.exe"
        goto BuildInstaller
    )
    
    echo NSIS not found in common locations.
    echo Please install NSIS or add it to your PATH.
    pause
    exit /b 1
)

set NSIS_PATH=makensis

:BuildInstaller
echo Building installer with %NSIS_PATH%...
%NSIS_PATH% FileSize-Installer.nsi

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
