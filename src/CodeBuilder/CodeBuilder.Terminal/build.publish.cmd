@echo off

set SolutionDir=..\..\..\..\..\

set BinDir=%SolutionDir%bin\

set SrcDir=%SolutionDir%src\

set DistDir=%SolutionDir%dist\codebuilder\1.0.0\

set TemplateDir=%SrcDir%CodeBuilder\CodeBuilder.Templates\

set ProjectDir=%1

set TargetDir=%2

set TargetName=%3

set ConfigurationName=%4

echo Deploy project files - %TargetName%

if not exist "%ProjectDir%App.config" copy "%ProjectDir%App.example.config" "%ProjectDir%App.config" /y

copy "%ProjectDir%App.config" "%TargetDir%%TargetName%.exe.config" /y

if exist "%TargetDir%%TargetName%.dll" copy "%TargetDir%%TargetName%.dll" %BinDir% /y
if exist "%TargetDir%%TargetName%.pdb" copy "%TargetDir%%TargetName%.pdb" %BinDir% /y

call :CopyTemplateFiles

if %ConfigurationName% == Release call :CopyDistFiles

@rem =========================================================
@rem Build Project Function
@rem =========================================================

:CopyTargetFiles

	if exist "%TargetDir%%TargetName%.dll" copy "%TargetDir%%TargetName%.dll" %BinDir% /y
	if exist "%TargetDir%%TargetName%.pdb" copy "%TargetDir%%TargetName%.pdb" %BinDir% /y
	@rem if exist "%TargetDir%%TargetName%.xml" copy "%TargetDir%%TargetName%.xml" %BinDir% /y
	
goto :EOF

:CopyTemplateFiles
 
	xcopy "%TemplateDir%*.*" "%TargetDir%templates\" /y /s /exclude:%SrcDir%CodeBuilder\CodeBuilder.Terminal\build.publish.exclude.txt

goto :EOF       

:CopyDistFiles
    
    copy "%TargetDir%%TargetName%.exe" "%DistDir%" /y
	copy "%ProjectDir%App.example.config" "%DistDir%%TargetName%.exe.example.config" /y
	xcopy "%TargetDir%*.dll" "%DistDir%bin\" /y
    xcopy "%TargetDir%templates\*.*" "%DistDir%templates\" /y /s
    
goto :EOF       
