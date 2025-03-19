# PowerShell script to fix encoding issues in project files

# Function to fix encoding of a file
function Fix-FileEncoding {
    param (
        [string]$filePath
    )
    
    Write-Host "Fixing encoding for: $filePath"
    
    # Read the file content
    $content = Get-Content -Path $filePath -Raw
    
    # Remove any BOM characters and ensure proper XML format
    $content = $content -replace "^\xEF\xBB\xBF", ""  # Remove UTF-8 BOM
    $content = $content -replace "^\xFE\xFF", ""      # Remove UTF-16 BE BOM
    $content = $content -replace "^\xFF\xFE", ""      # Remove UTF-16 LE BOM
    
    # Ensure the file starts with the proper XML declaration
    if (-not $content.StartsWith("<Project")) {
        $content = $content -replace "^.*?<Project", "<Project"
    }
    
    # Write the content back to the file with UTF-8 encoding without BOM
    $utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $false
    [System.IO.File]::WriteAllText($filePath, $content, $utf8NoBomEncoding)
    
    Write-Host "Fixed encoding for: $filePath"
}

# Fix encoding for all project files
$projectFiles = @(
    "src\FileSize.UI\FileSize.UI.csproj",
    "src\FileSize.Scanner\FileSize.Scanner.csproj",
    "src\FileSize.Display\FileSize.Display.csproj",
    "tests\FileSize.Scanner.Tests\FileSize.Scanner.Tests.csproj",
    "tests\FileSize.Display.Tests\FileSize.Display.Tests.csproj",
    "tests\FileSize.UI.Tests\FileSize.UI.Tests.csproj"
)

foreach ($file in $projectFiles) {
    $fullPath = Join-Path -Path $PSScriptRoot -ChildPath $file
    Fix-FileEncoding -filePath $fullPath
}

Write-Host "All project files have been fixed."
