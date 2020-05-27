@echo off

set ProjPath=%~dp0..\..

::proto生成器
set CompilerName=%ProjPath%\Tools\ProtobufGen\protoc

::协议文件夹路径
set InputFolder=%ProjPath%\ProtoDefine

::输出文件夹名
set OutputFolder=%ProjPath%\Assets\Scripts\Core\GameServer\dependence\Message

::删除之前创建的文件
del %OutputFolder%\*.cs /f /s /q

cd %InputFolder%

::遍历所有文件
for /f "delims=" %%i in ('dir /b "*.proto"') do (
    if Not "%%i" == "msgdefine_server_pt.proto" (
       echo %CompilerName% %%i --csharp_out=%OutputFolder%
       %CompilerName% %%i --csharp_out=%OutputFolder%
    )
)

echo 协议生成完毕。
pause