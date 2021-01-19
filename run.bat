cls
@ECHO OFF

:run-front
start cmd /k cd .\src\front\ICT120\; start run.bat

:run-back
%~dp0\src\back\ICT-151\run.bat

:end
pause
exit