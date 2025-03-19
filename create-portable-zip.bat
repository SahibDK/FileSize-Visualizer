@echo off
echo Creating portable ZIP package for FileSize Visualizer...

REM Create the installer directory if it doesn't exist
if not exist installer mkdir installer

REM Create a temporary directory for the package files
set TEMP_DIR=%TEMP%\FileSize-Portable
if exist %TEMP_DIR% rmdir /S /Q %TEMP_DIR%
mkdir %TEMP_DIR%

REM Copy the executable and license
echo Copying files...
xcopy /E /I /Y publish\*.* %TEMP_DIR%\FileSize
copy LICENSE %TEMP_DIR%\FileSize

REM Create a simple README.txt file
echo Creating README.txt...
echo FileSize Visualizer v1.0 > %TEMP_DIR%\README.txt
echo ======================= >> %TEMP_DIR%\README.txt
echo. >> %TEMP_DIR%\README.txt
echo Usage Instructions: >> %TEMP_DIR%\README.txt
echo 1. Extract the ZIP file to a location of your choice >> %TEMP_DIR%\README.txt
echo 2. Run FileSize.UI.exe from the extracted folder >> %TEMP_DIR%\README.txt
echo. >> %TEMP_DIR%\README.txt
echo Note: This application requires Windows 10 or later. >> %TEMP_DIR%\README.txt
echo. >> %TEMP_DIR%\README.txt
echo If you encounter "This app can't run on your PC" message: >> %TEMP_DIR%\README.txt
echo 1. Right-click on FileSize.UI.exe >> %TEMP_DIR%\README.txt
echo 2. Select Properties >> %TEMP_DIR%\README.txt
echo 3. Check "Unblock" at the bottom of the Properties dialog >> %TEMP_DIR%\README.txt
echo 4. Click Apply and OK >> %TEMP_DIR%\README.txt
echo. >> %TEMP_DIR%\README.txt
echo Copyright (c) 2025 QualityGate ApS. All rights reserved. >> %TEMP_DIR%\README.txt

REM Create a simple run.bat file
echo Creating run.bat...
echo @echo off > %TEMP_DIR%\run.bat
echo echo Running FileSize Visualizer v1.0... >> %TEMP_DIR%\run.bat
echo start FileSize\FileSize.UI.exe >> %TEMP_DIR%\run.bat

REM Create a ZIP file
echo Creating ZIP file...
powershell -Command "Compress-Archive -Path '%TEMP_DIR%\*' -DestinationPath 'installer\FileSize-Visualizer-Portable-v1.0.zip' -Force"

echo.
echo Portable ZIP package created successfully!
echo Package location: %CD%\installer\FileSize-Visualizer-Portable-v1.0.zip
echo.

pause
