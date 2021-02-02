cls
@ECHO OFF
title Run - BackEnd

:run
dotnet run --project=ICT-151 --configuration=Release --launch-profile ICT_151

:end
pause
exit