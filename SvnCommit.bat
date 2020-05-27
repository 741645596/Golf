@echo off

@set CoreFolder=".\Assets\Scripts\Core"

if exist %CoreFolder% (
 echo -----------------------
 echo 更新game svn
 echo -----------------------
 TortoiseProc /command:update /path:".\Assets\Plugins\Core" /closeonend:2

 echo -----------------------
 echo 编译Core.dll
 echo -----------------------
 rem call GenCoreDLL no yes
 
 echo -----------------------
 echo 提交Core svn
 echo -----------------------
 TortoiseProc /command:commit /path:%CoreFolder% /closeonend:0
)

echo -----------------------
echo 提交项目 svn
echo -----------------------
TortoiseProc /command:commit /path:".\" /closeonend:0

echo 完成