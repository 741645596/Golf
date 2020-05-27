using UnityEngine;
using System.Collections;
using System.Text;

// <summary>
// 资源路径管理
// </summary>
// <author>zhulin</author>

// 集中路径模式
public enum ResourcesPathMode {
    Editor,
    AssetBundle,
}
public class ResourcesPath
{
    // ui
    private static readonly string[] UiWndPath = { "Data/Prefab/UI/Wnd/", "Prefab/UI/Wnd/", ".prefab"};
    private static readonly string[] UiItemsPath = { "Data/Prefab/UI/Item/", "Prefab/UI/Item/", ".prefab"};
    private static readonly string[] UIAltasPath = { "Data/Icon/", "Icon/", ".png" };
    // prefab
    private static readonly string[] MapPath = { "Data/Prefab/Terrian/", "Prefab/Terrian/", ".prefab"};
    private static readonly string[] BallPath = { "Data/Prefab/Ball/", "Prefab/Ball/", ".prefab" };
    private static readonly string[] EffectPath = { "Data/Prefab/Effect/", "Prefab/Effect/", ".prefab"};
    private static readonly string[] HeroPath = { "Data/Prefab/Hero/", "Prefab/Hero/", ".prefab" };
    private static readonly string[] SceneItemPath = { "Data/Prefab/SceneItem/", "Prefab/SceneItem/", ".prefab" };
    // sounc
    private static readonly string[] MusicPath = { "Data/Sound/music/", "Sound/music/", ".wav"};
    private static readonly string[] VoicePath = { "Data/Sound/voice/", "Sound/voice/", ".wav" };
    // shader
    private static readonly string[] ShaderPath = {"Shader/Core/", "shader/", ".shader"};
    private static readonly string[] MaterialPath = { "Resources/Material/", "Material/", ".mat" };
    // scene
    private static readonly string[] ScenePath = {"Scene/", "scene/", ".unity"};
    // config
    private static readonly string[] LuaDataPath = { "Scripts/lua/", "lua/", ".lua"};
    // config
    private static readonly string[] ConfigPath = { "Data/Config/", "Config/", ".asset" };
    
    // 获取资源运行时路径
    public static string GetAssetResourceRunPath(string Type, ResourcesPathMode Mode)
    {
        StringBuilder Builder = new StringBuilder(string.Empty);
        if (Mode == ResourcesPathMode.AssetBundle) {
            Builder.Append(GetABRunDir());
        } else {
            Builder.Append("Assets/");
        }
        string str = GetRelativePath(Type, Mode);
        Builder.Append(str);
        return Builder.ToString();
    }
    
    
    
    
    private static string GetABRunDir()
    {
        return ConstantData.ABSavePath;
    }
    
    
    private static string[] GetResTypePath(string Type)
    {

        if (Type == ResourcesType.UIWnd) {
            return UiWndPath;
        } else if (Type == ResourcesType.UIWndItem) {
            return UiItemsPath;
        } else if (Type == ResourcesType.UIAltas) {
            return UIAltasPath;
        } else if (Type == ResourcesType.Effect) {
            return EffectPath;
        } else if (Type == ResourcesType.Map) {
            return MapPath;
        } else if (Type == ResourcesType.Ball) {
            return BallPath;
        } else if (Type == ResourcesType.Hero) {
            return HeroPath;
        } 
        
        else if (Type == ResourcesType.SceneItem) {
            return SceneItemPath;
        } else if (Type == ResourcesType.Music) {
            return MusicPath;
        } else if (Type == ResourcesType.Voice) {
            return VoicePath;
        } else if (Type == ResourcesType.Scene) {
            return ScenePath;
        }  else if (Type == ResourcesType.Shader) {
            return ShaderPath;
        } else if (Type == ResourcesType.Material) {
            return MaterialPath;
        } else if (Type == ResourcesType.luaData) {
            return LuaDataPath;
        } else if (Type == ResourcesType.Config) {
            return ConfigPath;
        }
        return null;
    }
    
    
    
    /// <summary>
    /// 相对路径
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="Mode"></param>
    /// <returns></returns>
    public static string GetRelativePath(string Type, ResourcesPathMode Mode)
    {
        int index = (int)Mode;
        string[] sArray = GetResTypePath(Type);
        if (sArray == null || sArray.Length == 0) {
            return string.Empty;
        }
        return sArray[index];
    }
    
    public static string GetResPath(string Type)
    {
        string[] sArray = GetResTypePath(Type);
        if (sArray == null || sArray.Length < 2) {
            return string.Empty;
        }
        return sArray[1];
    }
    
    public static string GetFileExt(string Type)
    {
    
        string[] sArray = GetResTypePath(Type);
        if (sArray == null || sArray.Length < 3) {
            return string.Empty;
        }
        return sArray[2];
    }
}




