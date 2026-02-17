@echo off
start "" "C:\Program Files\Google\Chrome\Application\chrome.exe" ^
--unsafely-treat-insecure-origin-as-secure="http://192.168.25.25" ^
--user-data-dir="C:\chrome-dev"
exit
