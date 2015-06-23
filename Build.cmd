@ECHO off

PUSHD "%~dp0"

ECHO.
ECHO Cleaning output
REM RMDIR /S /Q "output"

ECHO.
ECHO Cleaning packages
REM FOR /D %%P IN (.\packages\*) DO RMDIR /S /Q "%%P"

ECHO.
ECHO Cleaning intermediate and build files
REM FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
REM FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

ECHO.
ECHO Executing FullBuid.ps1
powershell .\shared\build\FullBuild.ps1 ^
  -BuildRunner  "Local" ^
  -MsBuildDir   "C:\Windows\Microsoft.NET\Framework\v4.0.30319" ^
  -NuGetDir     ".\.." ^
  -DotCoverDir  "C:\Users\%USERNAME%\AppData\Local\JetBrains\Installations\dotCover02"

POPD

PAUSE