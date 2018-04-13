@echo off
pushd "%~dp0"

del /Q /S DataCenter\Common\*.cs
del /Q /S DataCenter\JsonPmd\*.cs
rem ===================================================================
rem PlatCommon
::upa ../PlatCommon git@git.code4.in:mobilegameserver/platcommon.git 0524

call protogx csharp ../PlatCommon -ns:PmdProtobuf
move ..\PlatCommon\*.cs DataCenter\Common

call protogx  unity-json ../PlatCommon 
move ..\PlatCommon\*.cs DataCenter\JsonPmd

rem ===================================================================
rem Common
upa ../BwtCommon git@git.code4.in:bwt/common.git
del /Q /S ../BwtCommon\user\*.cs
call protogx csharp ../BwtCommon\user
move ..\BwtCommon\user\*.cs DataCenter\Common


call:clearMeta DataCenter\Common
popd
pause
GOTO:EOF

rem ===================================================================
rem 递归删除给定目录中所有孤立的*.meta文件
rem 参数: 路径
:clearMeta
echo clear meta: %~1
for /f "tokens=* delims=" %%i in ('dir /b /s "%~1\*.meta"') do (
	if not exist "%%~dpni" (
		echo %%i
		del /Q "%%i"
	)
)
GOTO:EOF