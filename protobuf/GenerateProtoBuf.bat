bin\CodeGenerator\Debug\CodeGenerator --fix-nameclash --preserve-names --ctor --net2 --utc --skip-default ..\..\platcommon\ImportAll.proto --output ..\DataCenter\Generate\GeneratedCommon.cs
bin\CodeGenerator\Debug\CodeGenerator --fix-nameclash --preserve-names --ctor --net2 --utc --skip-default ..\..\common\user\ImportAll.proto --output ..\DataCenter\Generate\GeneratedUser.cs
bin\CodeGenerator\Debug\CodeGenerator --fix-nameclash --preserve-names --ctor --net2 --utc --skip-default ..\..\BwtData\table\client\ImportAll.proto --output ..\DataCenter\Generate\GeneratedConfig.cs
pause