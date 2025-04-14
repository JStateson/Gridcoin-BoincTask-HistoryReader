rem removed echos to allow other users to build in their own environment (no D drive)
rem but they need 7z in program files
echo.> D:\Projects\VSrepository\CSresults.txt
set SRC=%2
set ARC=%1CreditStatistics\CSappbins.7z
set DOC=%1CreditStatistics
del %ARC%
echo %SRC%>> D:\Projects\VSrepository\CSresults.txt
set IS_64=%Src:~-12,3%
echo IS_64 %IS_64% >>  D:\Projects\VSrepository\CSresults.txt
if %IS_64% == x64 (
echo %2%364.exe >>  D:\Projects\VSrepository\CSresults.txt
set PGM=%2%364.exe
) else (
echo %2%332.exe >>  D:\Projects\VSrepository\CSresults.txt
set PGM=%2%332.exe
)
echo copy /y "%4" "%PGM%" >>  D:\Projects\VSrepository\CSresults.txt
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM% >>  D:\Projects\VSrepository\CSresults.txt
echo 4 and PGM follow>> D:\Projects\VSrepository\CSresults.txt
copy /y "%4" "%PGM%"
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM% >>  D:\Projects\VSrepository\CSresults.txt
"C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM%
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% *.rtf >>  D:\Projects\VSrepository\CSresults.txt
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% %DOC%\*.rtf %DOC%\*.txt >> D:\Projects\VSrepository\CSresults.txt
"C:\Program Files\7-Zip\7z.exe" u %ARC% %DOC%\*.rtf %DOC%\*.txt
copy /y %DOC%\*.rtf %SRC%
copy /y %DOC%\*.txt %SRC%
echo copy /y %DOC%\*.rtf %SRC%>> D:\Projects\VSrepository\CSresults.txt
echo copy /y %DOC%\*.txt %SRC%>> D:\Projects\VSrepository\CSresults.txt