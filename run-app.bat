@echo off
echo Building the application...
dotnet build
if %ERRORLEVEL% EQU 0 (
    echo Build successful!
    echo Running the application...
    dotnet run --project src\FileSize.UI
) else (
    echo Build failed with exit code %ERRORLEVEL%
)
pause
