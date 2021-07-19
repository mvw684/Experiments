pushd "%~dp0"
set location %~dp0
set name %~n0
@echo %DATA% %TIME%
set EXIT_CODE=0

call a 
if %ERROR%  call :ERROR "a failed"
call b
if %ERROR%  call :ERROR "b failed"
call c
if %ERROR%  call :ERROR "c failed"
goto :SUCCESS

:ERROR
set EXIT_CODE=1
echo %0: exited with error %1
echo %0: %1

:SUCCESS
popd

@echo %DATA% %TIME%
exit /b %EXIT_CODE%
