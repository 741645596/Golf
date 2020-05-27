@echo off
@set GameFolder=".\Assets\Scripts\Core"
@set PluginFolder=".\Assets\Plugins\Core"
@set GameDll=".\Tools\CoreDll\Core.dll"

TortoiseProc /command:update /path:".\" /closeonend:0

if exist %GameFolder% (
	TortoiseProc /command:update /path:%GameFolder% /closeonend:0
) else (
	if not exist %PluginFolder% (
		md %PluginFolder% 
	)
	copy %GameDll% %PluginFolder%\Core.dll
	echo ¸üÐÂÍê±Ï
)