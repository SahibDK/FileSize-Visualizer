# PowerShell script to create a very simple icon for FileSize Visualizer

# Create a simple text file that will be used by the Inno Setup compiler
$iconContent = @"
[General Info]
IconCount=1
[IconInfo_0]
IconType=3
IconWidth=32
IconHeight=32
IconBpp=32
"@

$iconContent | Out-File -FilePath "app-icon.txt" -Encoding ASCII

Write-Host "Created a placeholder icon file. The Inno Setup compiler will use a default icon."
