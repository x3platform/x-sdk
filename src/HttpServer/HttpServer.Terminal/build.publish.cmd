@echo off

set SolutionDir=..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set SrcDir=%SolutionDir%src\

set DistDir=%SolutionDir%dist\httpserver\1.0.0\

set ProjectDir=%1

set TargetDir=%2

set TargetName=%3

set ConfigurationName=%4

echo Deploy project files - %TargetName%

if %ConfigurationName% == Release call :CopyDistFiles

@rem =========================================================
@rem Build Project Function
@rem =========================================================

:CopyDistFiles
    
    copy "%TargetDir%%TargetName%.exe" "%DistDir%" /y
	copy "%ProjectDir%App.example.config" "%DistDir%%TargetName%.exe.config" /y
	xcopy "%TargetDir%*.dll" "%DistDir%bin\" /y
    
goto :EOF       
