rem RMDIR /S /Q "output"

FOR /D %%P IN (.\packages\*) DO RMDIR /S /Q "%%P"

FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

powershell .\Build.ps1
pause