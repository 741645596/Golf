using UnityEngine;
using System.Collections.Generic;
using System.IO;
using GOE.Scene;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

/// <summary>
/// 描述：动态烘焙场景光照模型，并产生对应的Prefab文件
/// <para>创建时间：2016-06-15</para>
/// </summary>
public sealed class LightMapEditor
{
    private static List<RemapTexture2D> sceneLightmaps = new List<RemapTexture2D>();
    
    #region Menu
    
    //[MenuItem("LightMap/Remove Prefab Lightmaps")]
    public static void RemoveLightData()
    {
        PrefabLightmapData[] pldArr = GameObject.FindObjectsOfType<PrefabLightmapData>();
        if (pldArr != null) {
            foreach (var data in pldArr) {
                var target = data.gameObject;
                GameObject.DestroyImmediate(data);
                
                GameObject targetPrefab = PrefabUtility.GetPrefabParent(target) as GameObject;
                //GameObject targetPrefab =  PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(target);
                PrefabUtility.ReplacePrefab(target, targetPrefab);
                //PrefabUtility.SaveAsPrefabAsset(target,)
            }
        }
    }
    
    //[MenuItem("LightMap/Update Scene with Prefab Lightmaps")]
    public static void UpdateLightmaps()
    {
        UpdateSceneLightmap();
    }
    
    
    
    [MenuItem("LightMap/Bake Prefab Lightmaps[烘焙完成后再bake到prefab]")]
    public static void GenLightmap()
    {
        genBakeLightmapAndPrefab();
        
        //        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("----------------Update to Prefab Lightmap Finish -------------------------");
        
        
    }
    
    
    #endregion
    
    
    static string sceneName = null;
    static string scenePath = null;
    static string scenePlatFormPath = null;
    
    //static string scenePath_PC = null;
    //static string resourcePath_PC = null;
    
    //static string scenePath_ANDROID = null;
    //static string resourcePath_ANDROID = null;
    
    //static string scenePath_IOS = null;
    //static string resourcePath_IOS = null;
    
    static string afterFix = "_All";
    
    static void CopyFolder(string srcPath, string tarPath)
    {
        if (!Directory.Exists(srcPath)) {
            Directory.CreateDirectory(srcPath);
        }
        if (!Directory.Exists(tarPath)) {
            Directory.CreateDirectory(tarPath);
        }
        CopyFile(srcPath, tarPath);
        string[] directionName = Directory.GetDirectories(srcPath);
        foreach (string dirPath in directionName) {
            string directionPathTemp = tarPath + "\\" + dirPath.Substring(srcPath.Length + 1);
            CopyFolder(dirPath, directionPathTemp);
        }
    }
    static void CopyFile(string srcPath, string tarPath)
    {
        string[] filesList = Directory.GetFiles(srcPath);
        foreach (string f in filesList) {
            string fTarPath = tarPath + "\\" + f.Substring(srcPath.Length + 1);
            if (File.Exists(fTarPath)) {
                File.Copy(f, fTarPath, true);
            } else {
                File.Copy(f, fTarPath);
            }
        }
    }
    
    /// <summary>
    /// 生成lightmap和prefab资源
    /// </summary>
    ///
    private static void genBakeLightmapAndPrefab()
    {
        if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand) {
            Debug.LogError("ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.");
            return;
        }
        Debug.ClearDeveloperConsole();
        
        PrefabLightmapData[] pldArr = GameObject.FindObjectsOfType<PrefabLightmapData>();
        if (pldArr == null || pldArr.Length <= 0) {
            EditorUtility.DisplayDialog("提示", "没有找到必要的脚本PrefabLightmapData，请检查场景", "OK");
            return;
        }
        
        //        Lightmapping.Bake();
        sceneLightmaps.Clear();
        
        
        Scene curScene = EditorSceneManager.GetActiveScene();
        sceneName = Path.GetFileNameWithoutExtension(curScene.name);
        
        //scenePath = Path.GetDirectoryName(curScene.path) + "/" + sceneName + "/";
        scenePath = "Assets/Scene/" + sceneName + "/";
        // 源目录
        string sceneOriginPath = scenePath;
        // 平台目录
        //scenePlatFormPath = Path.GetDirectoryName(curScene.path) + "/" + sceneName + afterFix ;
        scenePlatFormPath = sceneOriginPath;
        
        //CopyFolder(sceneOriginPath, scenePlatFormPath);
        AssetDatabase.Refresh();
        
        
        string resourcesPath = "Assets/Data/Lightmap/" + sceneName;
        //Path.GetDirectoryName(curScene.path) + "/" + sceneName + afterFix +  "_lightmap/" + sceneName;
        
        foreach (PrefabLightmapData pld in pldArr) {
            GameObject gObj = pld.gameObject;
            List<RendererInfo> renderers = new List<RendererInfo>();
            List<Texture2D> lightmapFars = new List<Texture2D>();
            List<Texture2D> lightmapNears = new List<Texture2D>();
            
            // scenePath copy至scenePlatFormPath中，之后在resourcesPath下生产asset
            genLightmapInfo(scenePlatFormPath, resourcesPath, gObj, renderers, lightmapFars, lightmapNears);
            
            pld.mRendererInfos = renderers.ToArray();
            pld.mLightmapFars = lightmapFars.ToArray();
            pld.mLightmapNears = lightmapNears.ToArray();
            
            //改变当前场景中的光照贴图信息
            PrefabLightmapData.ApplyLightmaps(pld.mRendererInfos, pld.mLightmapFars, pld.mLightmapNears);
        }
    }
    
    private static void genLightmapInfo(string scenePath, string resourcePath, GameObject root,
        List<RendererInfo> renderers, List<Texture2D> lightmapFars,
        List<Texture2D> lightmapNears)
    {
        MeshRenderer[] subRenderers = root.GetComponentsInChildren<MeshRenderer>();
        
        LightmapData[] srcLightData = LightmapSettings.lightmaps;
        
        foreach (MeshRenderer meshRenderer in subRenderers) {
            if (meshRenderer.lightmapIndex == -1) {
                continue;
            }
            
            RendererInfo renderInfo = new RendererInfo();
            renderInfo.renderer = meshRenderer;
            renderInfo.LightmapIndex = meshRenderer.lightmapIndex;
            renderInfo.LightmapOffsetScale = meshRenderer.lightmapScaleOffset;
            
            Texture2D lightmapFar = srcLightData[meshRenderer.lightmapIndex].lightmapColor;
            Texture2D lightmapNear = srcLightData[meshRenderer.lightmapIndex].lightmapDir;
            
            int sceneCacheIndex = addLightmap(scenePath, resourcePath, renderInfo.LightmapIndex, lightmapFar,
                    lightmapNear);
                    
            renderInfo.LightmapIndex = lightmapFars.IndexOf(sceneLightmaps[sceneCacheIndex].LightmapFar);
            if (renderInfo.LightmapIndex == -1) {
                renderInfo.LightmapIndex = lightmapFars.Count;
                lightmapFars.Add(sceneLightmaps[sceneCacheIndex].LightmapFar);
                lightmapNears.Add(sceneLightmaps[sceneCacheIndex].LightmapNear);
            }
            
            renderers.Add(renderInfo);
        }
    }
    
    
    private static int addLightmap(string scenePath, string resourcePath, int originalLightmapIndex,
        Texture2D lightmapFar,
        Texture2D lightmapNear)
    {
    
        for (int i = 0; i < sceneLightmaps.Count; i++) {
            if (sceneLightmaps[i].OriginalLightmapIndex == originalLightmapIndex) {
                return i;
            }
        }
        
        
        RemapTexture2D remapTex = new RemapTexture2D();
        remapTex.OriginalLightmapIndex = originalLightmapIndex;
        remapTex.OrginalLightmap = lightmapFar;
        
        string fileName = scenePath;
        remapTex.LightmapFar = getLightmapAsset(fileName + lightmapFar.name, resourcePath  + "-" + originalLightmapIndex.ToString(), false);
        
        if (lightmapNear != null) {
            remapTex.LightmapNear = getLightmapAsset(fileName + lightmapNear.name, resourcePath + "-" + originalLightmapIndex.ToString(), true);
        }
        
        sceneLightmaps.Add(remapTex);
        
        return sceneLightmaps.Count - 1;
    }
    
    
    private static Texture2D getLightmapAsset(string fileName, string resourecPath, bool ifDir)
    {
        string assetPath = resourecPath;
        if (!ifDir) {
            fileName += ".exr";
            assetPath += ".exr";
        } else {
            fileName += ".png";
            assetPath += ".png";
        }
        
        string dir = Path.GetDirectoryName(assetPath);
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        
        if (File.Exists(assetPath)) {
            File.Copy(fileName, assetPath, true);
        } else {
        
            File.Copy(fileName, assetPath);
            
        }
        
        //File.Copy(fileName, assetPath, true);
        AssetDatabase.Refresh();
        Texture2D newLightmap = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        //newLightmap.
        
        
        return newLightmap;
    }
    
    //according to the current platform, update all the lightmap that assets refrence
    private static void UpdateSceneLightmap()
    {
    
        PrefabLightmapData[] pldArr = GameObject.FindObjectsOfType<PrefabLightmapData>();
        if (pldArr == null || pldArr.Length <= 0) {
            EditorUtility.DisplayDialog("提示", "没有找到必要的脚本PrefabLightmapData，请检查场景", "OK");
            return;
        }
        
        //        Lightmapping.Bake();
        sceneLightmaps.Clear();
        
        
        Scene curScene = EditorSceneManager.GetActiveScene();
        sceneName = Path.GetFileNameWithoutExtension(curScene.name);
        
        string resourcesPath = Path.GetDirectoryName(curScene.path) + "/" + sceneName + afterFix + "_lightmap/" ;
        
        List<string> farNames = new List<string>();
        List<string> nearNames = new List<string>();
        foreach (PrefabLightmapData pld in pldArr) {
            GameObject gObj = pld.gameObject;
            List<RendererInfo> renderers = new List<RendererInfo>();
            List<Texture2D> lightmapFars = new List<Texture2D>();
            List<Texture2D> lightmapNears = new List<Texture2D>();
            
            RemapTexture2D remapTex = new RemapTexture2D();
            
            var originFars = pld.mLightmapFars;
            var originNears = pld.mLightmapNears;
            
            if (originFars != null && originFars.Length > 0) {
            
                foreach (var tex in originFars) {
                    if (tex == null) {
                        continue;
                    }
                    
                    string fullpath = AssetDatabase.GetAssetPath(tex);
                    int lastVal = fullpath.LastIndexOf("_ANDROID");
                    if (lastVal > 0) {
                        fullpath = fullpath.Replace("_ANDROID", afterFix);
                    } else if (fullpath.LastIndexOf("_IOS") > 0) {
                        fullpath = fullpath.Replace("_IOS", afterFix);
                    } else {
                        fullpath = fullpath.Replace("_WIN", afterFix);
                    }
                    
                    
                    string fileName = fullpath;
                    
                    Debug.Log(fileName);
                    if (farNames.Contains(fileName)) {
                        continue;
                    } else {
                        farNames.Add(fileName);
                    }
                    
                    remapTex.LightmapFar = UpdateLightmapAsset(fileName);
                    lightmapFars.Add(remapTex.LightmapFar);
                    
                }
                
            }
            
            if (originNears != null && originNears.Length > 0) {
            
                foreach (var tex in originNears) {
                
                    if (tex == null) {
                        continue;
                    }
                    
                    string fullpath = AssetDatabase.GetAssetPath(tex);
                    int lastVal = fullpath.LastIndexOf("_ANDROID");
                    if (lastVal > 0) {
                        fullpath = fullpath.Replace("_ANDROID", afterFix);
                    } else if (fullpath.LastIndexOf("_IOS") > 0) {
                        fullpath = fullpath.Replace("_IOS", afterFix);
                    } else {
                        fullpath = fullpath.Replace("_WIN", afterFix);
                    }
                    
                    string fileName = fullpath;
                    
                    if (nearNames.Contains(fileName)) {
                        continue;
                    } else {
                        nearNames.Add(fileName);
                    }
                    
                    remapTex.LightmapNear = UpdateLightmapAsset(fileName);
                    lightmapNears.Add(remapTex.LightmapNear);
                }
            }
            sceneLightmaps.Add(remapTex);
            
            //pld.mRendererInfos = renderers.ToArray();
            pld.mLightmapFars = lightmapFars.ToArray();
            pld.mLightmapNears = lightmapNears.ToArray();
            
            GameObject targetPrefab = PrefabUtility.GetPrefabParent(gObj) as GameObject;
            
            if (targetPrefab != null) {
                //自定义存放的路径
                PrefabUtility.ReplacePrefab(gObj, targetPrefab);
            } else {
                //默认路径
                //                string prefabPath = Path.GetDirectoryName(curScene.path) + "/" + sceneName + ".prefab";
                string prefabPath = Path.GetDirectoryName(curScene.path) + "/" + sceneName + "/" + gObj.name + ".prefab";
                PrefabUtility.CreatePrefab(prefabPath, gObj, ReplacePrefabOptions.ConnectToPrefab);
            }
            
            //改变当前场景中的光照贴图信息
            PrefabLightmapData.ApplyLightmaps(pld.mRendererInfos, pld.mLightmapFars, pld.mLightmapNears);
        }
        
        Debug.Log("*******************update finish*****************************");
    }
    
    private static Texture2D UpdateLightmapAsset(string fileName)
    {
        string assetPath = fileName;
        Texture2D newLightmap = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        return newLightmap;
    }
}