cls
SETLOCAL ENABLEEXTENSIONS
@pushd "%~dp0"
@set location=%~dp0
@set name=%~n0
@echo %DATE% %TIME%
@set EXIT_CODE=0

@call reset

@cmd.exe /C a.cmd
@if ERRORLEVEL 1 call :ERROR "a failed"
@cmd.exe /C b
@if ERRORLEVEL 1 call :ERROR "b failed"
@cmd.exe /C c
@if ERRORLEVEL 1 call :ERROR "c failed"

@call report

@goto :SUCCESS

:ERROR
set EXIT_CODE=1
call log %name%: exited with error %1
echo %name%: %EXIT_CODE%

:SUCCESS
@popd

@echo %DATE% %TIME% %name%
@exit /b %EXIT_CODE%
