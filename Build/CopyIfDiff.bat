REM CopyIfDiff.bat
REM Author: Scott Hanselman, September 21, 2007
REM Source: http://www.hanselman.com/blog/ManagingMultipleConfigurationFileEnvironmentsWithPreBuildEvents.aspx
REM Last Modified: Chris Andrews, February 15, 2012
REM Usage:
REM "$(ProjectDir)CopyIfDiff.bat" "$(SolutionDir)Config\WebService\$(ConfigurationName)\web.config" "$(ProjectDir)web.config"

@echo off
echo Comparing files.

if not exist %1 goto File1NotFound
if not exist %2 goto File2NotFound

fc %1 %2 > null
if %ERRORLEVEL%==0 GOTO NoCopy

echo Files are not the same.  Copying.
echo %1
echo %2
copy /Y %1 %2 & goto END

:NoCopy
echo Files are the same.  Did nothing.
goto END

:File1NotFound
echo Missing %1
goto END

:File2NotFound
echo Missing %2
copy %1 %2 /y
goto END

:END
echo Done.

