using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public enum VerContent {
    FullVersion  = 0,  //完整版本
    IncreVersion = 1,  //增量版本
}


public class ConstantData
{
    public static VerContent g_VerContent = VerContent.FullVersion;
    
    public static string ABSavePath{
        get {
#if UNITY_STANDALONE_WIN
            return Application.streamingAssetsPath + "/ab/" + LogicConstantData.g_version + "/";
#else
            return Application.persistentDataPath + "/ab/";
#endif
        }
    }
    
    
    /*private static string StreamingAssetsPath {
        get
        {
    #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            return "file:///" + Application.streamingAssetsPath + "/";
    #elif UNITY_ANDROID
            return  Application.streamingAssetsPath + "/" ;
            //return "jar:file://" + Application.dataPath + "!/assets/";
    #elif UNITY_IPHONE
            return  "file:///" + Application.dataPath + "/Raw/";
    #else
            return "file:///" + Application.streamingAssetsPath + "/";
    #endif
        }
    }*/
    
    
    /*public static string ABServerPath{
        get {
            if (g_VerContent == VerContent.FullVersion)
            {
                return StreamingAssetsPath;
            } else
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer) {
                    return "http://col3abpc.nos-eastchina1.126.net/" + LogicConstantData.g_version + "%2F";
                } else if (Application.platform == RuntimePlatform.Android) {
                    return "http://col3abandroid.nos-eastchina1.126.net/" + LogicConstantData.g_version + "%2F";
                } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                    return "http://col3abios.nos-eastchina1.126.net/" + LogicConstantData.g_version + "%2F";
                } else {
                    return "http://col3abpc.nos-eastchina1.126.net/" + LogicConstantData.g_version + "%2F";
                }
            }
        }
    }*/
    
    
    public static string ABEXT{
        get{ return ".ab";}
    }
    /// <summary>
    /// 默认分辨率
    /// </summary>
    ///
    public static float g_fDefaultResolutionWidth = 1920;
    public static float g_fDefaultResolutionHeight = 1080;
    
    
    public static string ServerListServerPath {
        get
        {
            return "http://config.nos-eastchina1.126.net/";
        }
    }
    
    
    public static string ServerListSavePath {
        get
        {
            return Application.persistentDataPath + "/";
        }
    }
    
    public static string ServerListFile {
        get
        {
            return "serverlist.txt";
        }
    }
    
    // AssetBundle缓存时间
    public static float AssetBundleCacheTime = 10;
    
    // 开启AssetBundle
#if UNITY_EDITOR
    public static bool EnableAssetBundle = false;
#else
    public static bool EnableAssetBundle = true;
#endif
    
    // 打AssetBundle前,清除旧的AssetBundle(全部重新打)
    public static bool ClearAssetBundleBeforeBuild = false;
    
    // 打AssetBundle前,重置AssetBundle Name
    public static bool ResetAssetBundleBeforeBuild = false;
    
    // 开启冗余资源检测
    public static bool EnableAssetBundleRedundance = true;
    
    // 开启缓存
    public static bool EnableCache = true;
    
    // 使用自定义压缩
    public static bool EnableCustomCompress = false;
    
    // 开启资源解压
    public static bool EnableUnpack = false;
    
    // 开启检测更新
    public static bool EnablePatch = false;
    
    // AssetBundle使用md5名
    public static bool EnableMd5Name = true;
    
    // AssetBundle描述文件名
    public const string AssetbundleManifest = "data";
    
    // AssetBundle资源映射文件名
    public const string AssetbundleMapping = "ab_mapping";
    
    // AssetBundle后缀
    public const string AssetBundleExt = ".ab";
    
    // 开启Bugly
    public static bool EnableBugly = false;
    
    public static bool UseSkinData = true;
    
    // Bugy AppID
#if UNITY_IOS
    public const string BuglyAppId = "6f446c435a";
#else
    public const string BuglyAppId = "44b5c1c4e2";
#endif
    
    public static string AssetBundleSavePath {
        get
        {
#if UNITY_STANDALONE_WIN
            return Application.streamingAssetsPath + "/ab/";
#else
            return Application.persistentDataPath + "/ab/";
#endif
        }
    }
    
    /// <summary>
    /// 默认分辨率
    /// </summary>
    ///
    public static float DefaultResolutionWidth = 1920;
    
    public static float DefaultResolutionHeight = 1080;
    
    
    
    
    private static string g_msUpdateUrl;
    
    
    
    // 服务条款
    public const string UrlAgreement = "https://www.igg.com/about/agreement.php";
    
    // 应用商店
    public static string UrlAppStore {
        get
        {
#if UNITY_IOS
            // AppStore
            return "http://www.baidu.com";
#else
            // GooglePlay
            return "http://www.baidu.com";
#endif
        }
    }
    
    // 退到后台
    public static bool EnterBackgroundForReconnect = false; // 是否退到后台
    public static float EnterBackgroundTime = 0f; // 退到后台的时间
    public static float MaxReconnectTimeFromBackground = 30 * 60; // 退到后台时间小于30分钟,断线重连,否则重新登录
    
    public const string DataPath = "Data"; // 资源路径(Assets下的相对路径)
    public const string AssetBundlePath = "ab"; // AssetBundle相对路径
    
    private static string g_dataPath; // 资源绝对路径
    private static string g_streamingAssetsPath; // 资源的ab包绝对路径
    private static string g_unpackPath; // 解压绝对路径
    private static string g_patchPath; // 补丁绝对路径
    private static string g_wwisePatchPath; // Wwise补丁绝对路径
    private static string g_tempPath; // 临时文件绝对路径
    
    // 资源绝对路径
    public static string DataFullPath {
        get
        {
            if (string.IsNullOrEmpty(g_dataPath))
            {
                g_dataPath = string.Format("{0}/{1}", Application.dataPath, DataPath);
            }
            
            return g_dataPath;
        }
    }
    
    // 资源的ab包绝对路径
    public static string StreamingAssetsPath {
        get
        {
            if (string.IsNullOrEmpty(g_streamingAssetsPath))
            {
                g_streamingAssetsPath = string.Format("{0}/{1}", Application.streamingAssetsPath, AssetBundlePath);
            }
            
            return g_streamingAssetsPath;
        }
    }
    
    // 解压绝对路径
    public static string UnpackPath {
        get
        {
            if (string.IsNullOrEmpty(g_unpackPath))
            {
                g_unpackPath = string.Format("{0}/{1}", Application.persistentDataPath, AssetBundlePath);
            }
            
            return g_unpackPath;
        }
    }
    
    // 更新资源绝对路径
    public static string PatchPath {
        get
        {
            if (string.IsNullOrEmpty(g_patchPath))
            {
                g_patchPath = string.Format("{0}/patch", Application.persistentDataPath);
            }
            
            return g_patchPath;
        }
    }
    
    
    // 临时文件绝对路径
    public static string TempPath {
        get
        {
            if (string.IsNullOrEmpty(g_tempPath))
            {
                g_tempPath = string.Format("{0}/temp", Application.persistentDataPath);
            }
            
            return g_tempPath;
        }
    }
    
    
    
}
