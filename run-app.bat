@echo off
echo FileSize Visualizer v1.0 - Now with enhanced tooltips and zoom functionality!
echo Building the application...
dotnet build FileSize.sln -v detailed
if %ERRORLEVEL% EQU 0 (
    echo Build successful!
    echo Running the application...
    dotnet run --project src\FileSize.UI
) else (
    echo Build failed with exit code %ERRORLEVEL%
    echo Detailed error information:
    dotnet build FileSize.sln -v diagnostic
)
pause
