# Test script for FileSize Visualizer

# Build the application
Write-Host "Building the application..."
dotnet build

# Check if the build was successful
if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful!"
    
    # Run the application
    Write-Host "Running the application..."
    dotnet run --project src\FileSize.UI
} else {
    Write-Host "Build failed with exit code $LASTEXITCODE"
}
