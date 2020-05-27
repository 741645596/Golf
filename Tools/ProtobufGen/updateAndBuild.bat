@echo off

set ProjPath=%~dp0..\..

::proto������
set CompilerName=%ProjPath%\Tools\ProtobufGen\protoc

::Э���ļ���·��
set InputFolder=%ProjPath%\Assets\Scripts\Game\GameServer\dependence\ProtoDefine

::����ļ�����
set OutputFolder=%ProjPath%\Assets\Scripts\Game\GameServer\dependence\Message

::����svn
TortoiseProc /command:update /path:%InputFolder% /closeonend:2

::ɾ��֮ǰ�������ļ�
del %OutputFolder%\*.cs /f /s /q

cd %InputFolder%

::���������ļ�
for /f "delims=" %%i in ('dir /b "*.proto"') do (
    if Not "%%i" == "msgdefine_server_pt.proto" (
       echo %CompilerName% %%i --csharp_out=%OutputFolder%
       %CompilerName% %%i --csharp_out=%OutputFolder%
    )
)

echo Э��������ϡ�
pause