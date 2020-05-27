@echo off

::如果传no则不暂停
@set IsPause=%1%
::如果传yes则是测试
@set IsTest=%2%

@set Define="Release;UNITY_EDITOR;UNITY_EDITOR_WIN;UNITY_2018_3_OR_NEWER"

@set SAPath=.\Library\ScriptAssemblies
@set DllPath=.\Tools\UnityDll
@set GameSrcPath=.\Assets\Scripts\Core

if "%IsTest%" equ "yes" (
	@set Output=.\Tools\CoreDll\Core_test.dll
) else (
	@set Output=.\Tools\CoreDll\Core.dll
)

"C:\Program Files\Unity\Hub\Editor\2018.3.12f1\Editor\Data\MonoBleedingEdge\bin\mcs" ^
/r:%DllPath%\UnityEditor.dll ^
/r:%DllPath%\UnityEngine.dll ^
/r:%DllPath%\UnityEngine.CoreModule.dll ^
/r:%DllPath%\UnityEngine.AccessibilityModule.dll ^
/r:%DllPath%\UnityEngine.ParticleSystemModule.dll ^
/r:%DllPath%\UnityEngine.PhysicsModule.dll ^
/r:%DllPath%\UnityEngine.VehiclesModule.dll ^
/r:%DllPath%\UnityEngine.ClothModule.dll ^
/r:%DllPath%\UnityEngine.AIModule.dll ^
/r:%DllPath%\UnityEngine.AnimationModule.dll ^
/r:%DllPath%\UnityEngine.TextRenderingModule.dll ^
/r:%DllPath%\UnityEngine.UIModule.dll ^
/r:%DllPath%\UnityEngine.TerrainPhysicsModule.dll ^
/r:%DllPath%\UnityEngine.IMGUIModule.dll ^
/r:%DllPath%\UnityEngine.UnityWebRequestModule.dll ^
/r:%DllPath%\UnityEngine.UnityWebRequestAudioModule.dll ^
/r:%DllPath%\UnityEngine.UnityWebRequestTextureModule.dll ^
/r:%DllPath%\UnityEngine.UnityWebRequestWWWModule.dll ^
/r:%DllPath%\UnityEngine.ClusterInputModule.dll ^
/r:%DllPath%\UnityEngine.ClusterRendererModule.dll ^
/r:%DllPath%\UnityEngine.UNETModule.dll ^
/r:%DllPath%\UnityEngine.DirectorModule.dll ^
/r:%DllPath%\UnityEngine.UnityAnalyticsModule.dll ^
/r:%DllPath%\UnityEngine.CrashReportingModule.dll ^
/r:%DllPath%\UnityEngine.PerformanceReportingModule.dll ^
/r:%DllPath%\UnityEngine.UnityConnectModule.dll ^
/r:%DllPath%\UnityEngine.WebModule.dll ^
/r:%DllPath%\UnityEngine.ARModule.dll ^
/r:%DllPath%\UnityEngine.VRModule.dll ^
/r:%DllPath%\UnityEngine.UIElementsModule.dll ^
/r:%DllPath%\UnityEngine.StyleSheetsModule.dll ^
/r:%DllPath%\UnityEngine.AudioModule.dll ^
/r:%DllPath%\UnityEngine.GameCenterModule.dll ^
/r:%DllPath%\UnityEngine.GridModule.dll ^
/r:%DllPath%\UnityEngine.ImageConversionModule.dll ^
/r:%DllPath%\UnityEngine.InputModule.dll ^
/r:%DllPath%\UnityEngine.JSONSerializeModule.dll ^
/r:%DllPath%\UnityEngine.ParticlesLegacyModule.dll ^
/r:%DllPath%\UnityEngine.Physics2DModule.dll ^
/r:%DllPath%\UnityEngine.ScreenCaptureModule.dll ^
/r:%DllPath%\UnityEngine.SpriteMaskModule.dll ^
/r:%DllPath%\UnityEngine.TerrainModule.dll ^
/r:%DllPath%\UnityEngine.TilemapModule.dll ^
/r:%DllPath%\UnityEngine.VideoModule.dll ^
/r:%DllPath%\UnityEngine.WindModule.dll ^
/r:%DllPath%\UnityEngine.UI.dll ^
/r:%DllPath%\UnityEngine.Networking.dll ^
/r:%DllPath%\UnityEngine.TestRunner.dll ^
/r:%DllPath%\nunit.framework.dll ^
/r:%DllPath%\UnityEngine.Timeline.dll ^
/r:%DllPath%\UnityEngine.UIAutomation.dll ^
/r:%DllPath%\UnityEngine.GoogleAudioSpatializer.dll ^
/r:%DllPath%\UnityEngine.HoloLens.dll ^
/r:%DllPath%\UnityEngine.SpatialTracking.dll ^
/r:%DllPath%\DOTween.dll ^
/r:%DllPath%\DOTween43.dll ^
/r:%DllPath%\DOTween46.dll ^
/r:%DllPath%\DOTween50.dll ^
/r:%DllPath%\ICSharpCode.SharpZipLib.dll ^
/r:%DllPath%\Mono.Data.dll ^
/r:%DllPath%\Mono.Data.Sqlite.dll ^
/r:%DllPath%\UnityEngine.Analytics.dll ^
/r:%DllPath%\UnityEngine.Purchasing.dll ^
/r:%DllPath%\Unity.TextMeshPro.dll ^
/r:%DllPath%\UnityEngine.AssetBundleModule.dll ^
/r:%SAPath%\Assembly-CSharp.dll ^
/define:%Define% /optimize /target:library /out:%Output% /unsafe ^
/recurse:%GameSrcPath%\*.cs

echo done!

if "%IsPause%" neq "no" (
pause
)