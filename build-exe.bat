@echo off
echo Building FileSize Visualizer v1.0 executable...

REM Clean any previous builds
echo Cleaning previous builds...
dotnet clean FileSize.sln

REM Build the solution in Release mode
echo Building solution in Release mode...
dotnet build FileSize.sln -c Release

REM Publish the UI project as a self-contained application
echo Publishing executable...
dotnet publish src\FileSize.UI\FileSize.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o publish

echo.
echo Build completed!
echo Executable created at: %CD%\publish\FileSize.UI.exe
echo.

pause
