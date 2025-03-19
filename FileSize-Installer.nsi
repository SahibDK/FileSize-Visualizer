; FileSize Visualizer Installer Script
; Created with NSIS

!include "MUI2.nsh"

; General
Name "FileSize Visualizer"
OutFile "installer\FileSize-Visualizer-Setup.exe"
InstallDir "$PROGRAMFILES\FileSize Visualizer"
InstallDirRegKey HKLM "Software\FileSize Visualizer" "Install_Dir"
RequestExecutionLevel admin

; Interface Settings
!define MUI_ABORTWARNING

; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "LICENSE"
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Languages
!insertmacro MUI_LANGUAGE "English"

; Installer Sections
Section "FileSize Visualizer" SecMain
  SetOutPath "$INSTDIR"
  
  ; Add files
  File /r "publish\*.*"
  File "LICENSE"
  
  ; Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  ; Create shortcuts
  CreateDirectory "$SMPROGRAMS\FileSize Visualizer"
  CreateShortcut "$SMPROGRAMS\FileSize Visualizer\FileSize Visualizer.lnk" "$INSTDIR\FileSize.UI.exe"
  CreateShortcut "$SMPROGRAMS\FileSize Visualizer\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
  
  ; Write registry keys
  WriteRegStr HKLM "Software\FileSize Visualizer" "Install_Dir" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FileSize Visualizer" "DisplayName" "FileSize Visualizer"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FileSize Visualizer" "UninstallString" '"$INSTDIR\Uninstall.exe"'
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FileSize Visualizer" "Publisher" "QualityGate ApS"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FileSize Visualizer" "DisplayVersion" "1.0"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FileSize Visualizer" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FileSize Visualizer" "NoRepair" 1
SectionEnd

; Uninstaller Section
Section "Uninstall"
  ; Remove files and uninstaller
  Delete "$INSTDIR\Uninstall.exe"
  RMDir /r "$INSTDIR"
  
  ; Remove shortcuts
  Delete "$SMPROGRAMS\FileSize Visualizer\*.*"
  RMDir "$SMPROGRAMS\FileSize Visualizer"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FileSize Visualizer"
  DeleteRegKey HKLM "Software\FileSize Visualizer"
SectionEnd
