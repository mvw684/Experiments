cls
SETLOCAL ENABLEEXTENSIONS
@pushd "%~dp0"
@set location=%~dp0
@set name=%~n0
@echo %DATE% %TIME%
@set EXIT_CODE=0

@call a 
@if ERRORLEVEL 1 call :ERROR "a failed"
@call b
@if ERRORLEVEL 1 call :ERROR "b failed"
@call c
@if ERRORLEVEL 1 call :ERROR "c failed"
@goto :SUCCESS

:ERROR
set EXIT_CODE=1
call log %name%: exited with error %1
echo %name%: %EXIT_CODE%

:SUCCESS
@popd

@echo %DATE% %TIME% %name%
@exit /b %EXIT_CODE%
