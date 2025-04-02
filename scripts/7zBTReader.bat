set HIS=D:\Projects\VSrepository\BThistory
rem removed echos to allow other users to build in their own environment (no D drive)
rem but they need 7z in program files
rem echo Starting build  > %HIS%\BTresults.txt
set SRC=%2
set ARC=%1BTHistoryReader\BTexecutables.7z
set IS_64=%Src:~-12,-9%
rem "C:\Program Files\7-Zip\7z.exe -u"
rem echo %IS_64% >>  %HIS%\BTresults.txt
if %IS_64% == x64 (
rem echo %2%364.exe >>  %HIS%\BTresults.txt
copy /Y %2%364.exe %HIS%
set PGM=%2%364.exe
) else (
rem echo %2%332.exe >>  %HIS%\BTresults.txt
copy /Y %2%332.exe %HIS%
set PGM=%2%332.exe
)
rem echo copy /y "%4" "%PGM%" >>  %HIS%\BTresults.txt
copy /y "%4" "%PGM%"
rem echo "C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM% >> %HIS%\BTresults.txt
"C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM%