@pushd "%~dp0"
@set location=%~dp0
@set EXIT_CODE=0

@call log b b b
@setx script_b b

@goto :SUCCESS
:ERROR
@set EXIT_CODE=1
@call log %~n0: exited with error %1

:SUCCESS
@popd
echo %DATE% %TIME% %~n0
@exit /b %EXIT_CODE%
