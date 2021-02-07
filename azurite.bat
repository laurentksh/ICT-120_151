@ECHO OFF
title Run - Azurite

:azurite
IF NOT EXIST Azurite mkdir Azurite

azurite --silent --location Azurite --debug Azurite/debug.log
goto end

:end
pause
exit