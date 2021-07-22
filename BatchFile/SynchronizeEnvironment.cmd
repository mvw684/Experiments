@echo off
pushd "%~dp0"
set EXIT_CODE=0

rem Take care with setlocal, as that will limit scope of the values being set
rem first get misisng values form user
call :SYNCHRONIZE "HKCU\Environment"
rem anything not per current user take from system
call :SYNCHRONIZE "HKLM\System\CurrentControlSet\Control\Session Manager\Environment"
goto :SUCCESS

:SYNCHRONIZE
set MYFILE=%TEMP%\SynchronizeEnvironment.%RANDOM%.txt
reg query %1
reg query %1 > %MYFILE%
for /f "usebackq skip=2 tokens=1,3" %%A IN ("%MYFILE%") do (
    if defined %%A (
        echo Already defined: %%A %%B
    ) else (
        echo Defining: %%A %%B
        set %%A=%%B
    )
)
del %MYFILE%
goto :EOF

@goto :SUCCESS
:ERROR
set EXIT_CODE=1
call log %~n0: exited with error %1

:SUCCESS
popd
echo %DATE% %TIME% %~n0
exit /b %EXIT_CODE%
