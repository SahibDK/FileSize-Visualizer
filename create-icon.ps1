# PowerShell script to create a simple icon for FileSize Visualizer
# This script creates a very basic icon using System.Drawing

Add-Type -AssemblyName System.Drawing

# Create a bitmap
$width = 64
$height = 64
$bmp = New-Object System.Drawing.Bitmap($width, $height)
$graphics = [System.Drawing.Graphics]::FromImage($bmp)

# Fill background
$graphics.Clear([System.Drawing.Color]::FromArgb(0, 120, 215)) # Windows blue

# Draw a simple folder icon
$folderBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::White)
$folderPath = New-Object System.Drawing.Drawing2D.GraphicsPath
$rect1 = New-Object System.Drawing.Rectangle(12, 20, 40, 30)
$rect2 = New-Object System.Drawing.Rectangle(18, 14, 15, 6)
$folderPath.AddRectangle($rect1)
$folderPath.AddRectangle($rect2)
$graphics.FillPath($folderBrush, $folderPath)

# Draw a magnifying glass
$penWidth = 3
$pen = New-Object System.Drawing.Pen([System.Drawing.Color]::White, $penWidth)
$graphics.DrawEllipse($pen, 30, 25, 20, 20)
$graphics.DrawLine($pen, 45, 40, 52, 47)

# Clean up
$graphics.Dispose()

# Save as icon
$iconPath = "app-icon.ico"
$bmp.Save("temp.png", [System.Drawing.Imaging.ImageFormat]::Png)

# Convert PNG to ICO using PowerShell
$iconFile = Join-Path -Path $PSScriptRoot -ChildPath $iconPath
$tempFile = Join-Path -Path $PSScriptRoot -ChildPath "temp.png"

# Use PowerShell to convert PNG to ICO
Add-Type -AssemblyName System.Windows.Forms
$icon = [System.Drawing.Icon]::FromHandle(([System.Drawing.Bitmap]::FromFile($tempFile)).GetHicon())
$fileStream = New-Object System.IO.FileStream($iconFile, [System.IO.FileMode]::Create)
$icon.Save($fileStream)
$fileStream.Close()
$icon.Dispose()

# Clean up temp file
Remove-Item $tempFile

Write-Host "Icon created at: $iconFile"
