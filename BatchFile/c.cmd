@pushd "%~dp0"
@set location=%~dp0
@set name=%~n0
@set EXIT_CODE=0

@call log c c c 
@setx script_c c

@goto :SUCCESS
:ERROR
@set EXIT_CODE=1
@call log %~n0: exited with error %1

:SUCCESS
@popd
@echo %DATE% %TIME% %name%
@exit /b %EXIT_CODE%
