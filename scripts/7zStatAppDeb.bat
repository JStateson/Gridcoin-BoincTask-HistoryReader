rem removed echos to allow other users to build in their own environment (no D drive)
rem but they need 7z in program files
echo.> D:\Projects\VSrepository\CSresultsDeb.txt
set SRC=%2
set ARC=%1CreditStatistics\CSappbinsDeb.7z
set DOC=%1CreditStatistics
del %ARC%
echo %SRC%>> D:\Projects\VSrepository\CSresultsDeb.txt
set IS_64=%SRC:~-10,3%
echo IS_64 %IS_64% >>  D:\Projects\VSrepository\CSresultsDeb.txt
if %IS_64% == x64 (
echo %2%364Deb.exe >>  D:\Projects\VSrepository\CSresultsDeb.txt
set PGM=%2%364Deb.exe
) else (
echo %2%332Deb.exe >>  D:\Projects\VSrepository\CSresultsDeb.txt
set PGM=%2%332Deb.exe
)
echo copy /y "%4" "%PGM%" >>  D:\Projects\VSrepository\CSresultsDeb.txt
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM% >>  D:\Projects\VSrepository\CSresultsDeb.txt
echo 4 and PGM follow>> D:\Projects\VSrepository\CSresultsDeb.txt
copy /y "%4" "%PGM%"
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM% >>  D:\Projects\VSrepository\CSresultsDeb.txt
"C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM%
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% *.rtf >>  D:\Projects\VSrepository\CSresultsDeb.txt
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% %DOC%\*.rtf %DOC%\*.txt >> D:\Projects\VSrepository\CSresultsDeb.txt
"C:\Program Files\7-Zip\7z.exe" u %ARC% %DOC%\*.rtf %DOC%\*.txt
copy /y %DOC%\*.rtf %SRC%
copy /y %DOC%\*.txt %SRC%
echo copy /y %DOC%\*.rtf %SRC%>> D:\Projects\VSrepository\CSresultsDeb.txt
echo copy /y %DOC%\*.txt %SRC%>> D:\Projects\VSrepository\CSresultsDeb.txt