@echo off
pushd "%~dp0"

del /Q /S DataCenter\Generate\ProtocolProtobuf\*.cs
rem ===================================================================
rem PlatCommon
rem upa ../PlatCommon git@git.code4.in:mobilegameserver/platcommon.git master


protobuf\bin\CodeGenerator\Debug\CodeGenerator --fix-nameclash --preserve-names --ctor --net2 --utc --skip-default ../PlatCommon/*.proto --output DataCenter\Generate\ProtocolProtobuf\PlatCommonProtocol.cs
		






rem ===================================================================
rem Common
rem upa ../BwtCommon git@git.code4.in:bwt/common.git

 protobuf\bin\CodeGenerator\Debug\CodeGenerator --fix-nameclash --preserve-names --ctor --net2 --utc --skip-default ..\BwtCommon\user\*.proto --output DataCenter\Generate\ProtocolProtobuf\UserCommonProtocol.cs


del /Q /S DataCenter\Generate\ProtocolProtobuf\ProtocolParser.cs
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