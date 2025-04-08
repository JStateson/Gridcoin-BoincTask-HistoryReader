echo asusx299> systems.txt
echo h110btc>> systems.txt
echo jys-rtx-3070>> systems.txt
echo dual-linux>> systems.txt
echo dualx5675>> systems.txt
echo jysevga --passwd jys>> systems.txt
echo jysevga3 --passwd RatDogTrip>> systems.txt
echo jysomen>> systems.txt
echo jysx299 --passwd jys>> systems.txt
echo shire2 --passwd jys>> systems.txt

rem echo dell7010win>> systems.txt
rem echo jysdell435mt>> systems.txt

del MakeIDs.cmd
del RawList.txt

for /f "delims=" %%i in (systems.txt) do (
 echo echo %%i^>^>RawList.txt >>MakeIDs.cmd
 echo boinccmd.exe --host %%i --get_project_status ^>^>RawList.txt>>MakeIDs.cmd 
)

rem MakeIDs.cmd
