#region Namespace

using UnityEngine;

#endregion

#if UNITY_ANDROID
public class AndroidHelper
{
    // --------------------------------------------------------------------------------------------
    // MainActivity
    private const string g_androidMainActivity = "com.unity3d.player.UnityPlayer";

    // --------------------------------------------------------------------------------------------
    // Android帮助库
    private const string g_androidFileHelper = "com.igg.game.util.FileHelper";

    private static AndroidJavaObject g_mainActivity;

    private static AndroidJavaClass g_fileHelper;

    public static AndroidJavaObject MainActivity
    {
        get
        {
            if (g_mainActivity == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass(g_androidMainActivity);
                g_mainActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }

            return g_mainActivity;
        }
    }

    public static AndroidJavaClass FileHelper
    {
        get
        {
            if (g_fileHelper == null)
            {
                g_fileHelper = new AndroidJavaClass(g_androidFileHelper);
            }

            return g_fileHelper;
        }
    }

    // --------------------------------------------------------------------------------------------
}
#endif