@echo off

@set Define="Release;ServerClient;BEHAVIAC_RELEASE"
@set ServerCodePath=.\Assets\Scripts\Core\GameServer
@set ConfigCodePath=.\Assets\Scripts\Core\Data\Config
@set OutDllPath=.\Tools\CoreDll

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" ^
/define:%Define% ^
/optimize /target:library ^
/out:%OutDllPath%\GSServer.dll ^
/unsafe /recurse:%ServerCodePath%\*.cs ^
/recurse:%ConfigCodePath%\*.cs 

echo done!
pause