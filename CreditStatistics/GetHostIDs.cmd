rem the boinccmd.exe here is NOT the one supplied by BOINC
rem it was modified so that the project status returned the host IDs and names for each project
rem this was used to verify that the RPC tool I used matched what boinccmd provided
rem my RPC method in CreditStatistics.exe does not use passwords. If you use passwords 
rem then this script can be used but you need a copy of my mod'ed boinccmd.exe
del systems.txt
echo shire2>> systems.txt
echo asusx299>> systems.txt
echo jysx299>> systems.txt
echo jysomen>> systems.txt
echo jysevga3>> systems.txt
rem echo jysdell435mt>> systems.txt
echo jys-rtx-3070>> systems.txt
echo h110btc>> systems.txt
echo dualx5675>> systems.txt
echo dual-linux>> systems.txt
rem echo dell7010win>> systems.txt
echo jysevga>> systems.txt

del MakeIDs.cmd
del RawList.txt

for /f "delims=" %%i in (systems.txt) do (
 echo echo %%i^>^>RawList.txt >>MakeIDs.cmd
 echo boinccmd.exe --host %%i --get_project_status ^>^>RawList.txt>>MakeIDs.cmd
 echo echo.^>^>RawList.txt >>MakeIDs.cmd
)
del systems.txt
MakeIDs.cmd