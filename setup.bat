@echo off
title permafrost06 personal windows setup script
echo Setting up...
cd /D "%~dp0"

rem 24hr clock
REG ADD "HKCU\Control Panel\International" /v sShortTime /t REG_SZ /D "HH:mm" /f
REG ADD "HKCU\Control Panel\International" /v sTimeFormat /t REG_SZ /D "HH:mm:ss" /f
REG ADD "HKCU\Control Panel\International" /v iTime /t REG_SZ /D "1" /f
REG ADD "HKCU\Control Panel\International" /v iTLZero /t REG_SZ /D "1" /f
REG ADD "HKCU\Control Panel\International" /v iTimePrefix /t REG_SZ /D "0" /f
REG ADD "HKCU\Control Panel\International" /v sTime /t REG_SZ /D ":" /f

rem sat first day of week
REG ADD "HKCU\Control Panel\International" /v iFirstDayOfWeek /t REG_SZ /D "5" /f

rem add us 1st
REG ADD "HKEY_CURRENT_USER\Keyboard Layout\Preload" /v 1 /t REG_SZ /d 00000409 /f
rem add dvorak 2nd
REG ADD "HKEY_CURRENT_USER\Keyboard Layout\Preload" /v 2 /t REG_SZ /d d0010409 /f

rem left alt+shift to change input method
REG ADD "HKEY_CURRENT_USER\Keyboard Layout\Toggle" /v "Language Hotkey" /t REG_SZ /d 3 /f
REG ADD "HKEY_CURRENT_USER\Keyboard Layout\Toggle" /v "Hotkey" /t REG_SZ /d 3 /f
REG ADD "HKEY_CURRENT_USER\Keyboard Layout\Toggle" /v "Layout Hotkey" /t REG_SZ /d 1 /f

rem hide recycle bin from desktop
REG ADD HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel /v {645FF040-5081-101B-9F08-00AA002F954E} /t REG_DWORD /d 1 /f

rem disable mouse acceleration
REG ADD "HKCU\Control Panel\Mouse" /v MouseSpeed /t REG_SZ /d 0 /f
REG ADD "HKCU\Control Panel\Mouse" /v MouseThreshold1 /t REG_SZ /d 0 /f
REG ADD "HKCU\Control Panel\Mouse" /v MouseThreshold2 /t REG_SZ /d 0 /f

rem show file extensions
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced /v HideFileExt /t REG_DWORD /d 0 /f

rem launch "this pc" with explorer
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced /v LaunchTo /t REG_DWORD /d 1 /f

rem display delete confirmation dialog
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer /v ConfirmFileDelete /t REG_DWORD /D 1 /f

rem make taskbar icons small
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced /v TaskbarSmallIcons /t REG_DWORD /D 1 /f

rem show taskbar item labels
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced /v TaskbarGlomLevel /t REG_DWORD /D 1 /f

rem hide cortana button
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced /v ShowCortanaButton /t REG_DWORD /D 0 /f

rem hide task view button
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced /v ShowTaskViewButton /t REG_DWORD /D 0 /f

rem disable aero shake
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced /v DisallowShaking /t REG_DWORD /D 1 /f

rem hide searchbox
REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Search /v SearchboxTaskbarMode /t REG_DWORD /D 0 /f

rem delete taskbar icons
rem DEL /F /S /Q /A "%AppData%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar\*"
rem REG DELETE HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Taskband /F

rem don't move files to recycle bin
for /f %%a in ('REG QUERY "HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket\Volume"') do (
  if "%%a" NEQ "(Default)" if "%%a" NEQ "HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\BitBucket\Volume" (
    REG ADD "%%a" /v NukeOnDelete /t REG_DWORD /D 1 /f
  )
)

rem 7-zip extensions setup
set exts=001 7z arj bz2 bzip2 cpio deb dmg fat gz gzip hfs lha lzh lzma ntfs rar rpm squashfs swm tar taz tbz tbz tgz tpz txz wim xar xz z zip

(for %%a in (%exts%) do (
  assoc .%%a=7-Zip.%%a
  ftype 7-Zip.%%a="D:\Software\7-Zip\7zFM.exe" "%%1"
))

rem set ahk extension
assoc .ahk=AutoHotkey.ahk
ftype AutoHotkey.ahk="D:\Software\AHK\AutoHotkeyU32" "%%1"

rem set everything search
D:\Software\Everything\Everything.exe -install-efu-association
D:\Software\Everything\Everything.exe -install-run-on-system-startup
D:\Software\Everything\Everything.exe -install-service
D:\Software\Everything\Everything.exe -install-folder-context-menu
D:\Software\Everything\Everything.exe -install-url-protocol

rem set startup files
xcopy D:\Software\Startup %APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup /QCY

rem restart explorer
taskkill /f /im explorer.exe
start explorer.exe

echo Setup complete!
pause
