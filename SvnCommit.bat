@echo off

@set CoreFolder=".\Assets\Scripts\Core"

if exist %CoreFolder% (
 echo -----------------------
 echo ����game svn
 echo -----------------------
 TortoiseProc /command:update /path:".\Assets\Plugins\Core" /closeonend:2

 echo -----------------------
 echo ����Core.dll
 echo -----------------------
 rem call GenCoreDLL no yes
 
 echo -----------------------
 echo �ύCore svn
 echo -----------------------
 TortoiseProc /command:commit /path:%CoreFolder% /closeonend:0
)

echo -----------------------
echo �ύ��Ŀ svn
echo -----------------------
TortoiseProc /command:commit /path:".\" /closeonend:0

echo ���