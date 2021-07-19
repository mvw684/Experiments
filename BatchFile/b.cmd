pushd "%~dp0"
set location %~dp0
set name %~n0
@echo %DATA% %TIME%
set EXIT_CODE=0

call log b b b

goto :SUCCESS
:ERROR
set EXIT_CODE=1
echo %0: exited with error %1
echo %0: %1

:SUCCESS
popd

@echo %DATA% %TIME%
exit /b %EXIT_CODE%
