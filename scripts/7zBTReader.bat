set SRC=%2
set ARC=%1..\BTexecutables.7z
set IS_64=%Src:~-12,-9%
rem "C:\Program Files\7-Zip\7z.exe -u"
echo %IS_64% >>  D:\Projects\VSrepository\BTresults.txt
if %IS_64% == x64 (
rem echo %2%364.exe >>  D:\Projects\VSrepository\BTresults.txt
set PGM=%2%364.exe
) else (
rem echo %2%332.exe >>  D:\Projects\VSrepository\BTresults.txt
set PGM=%2%332.exe
)
echo copy /y "%4" "%PGM%" >>  D:\Projects\VSrepository\BTresults.txt
echo "C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM% >>  D:\Projects\VSrepository\BTresults.txt
copy /y "%4" "%PGM%" >>  D:\Projects\VSrepository\BTresults.txt
"C:\Program Files\7-Zip\7z.exe" u %ARC% %PGM% >>  D:\Projects\VSrepository\BYresults.txt