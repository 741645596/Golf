
using UnityEngine;
using System.Collections;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_Instance = null;
    private static bool m_bApplicationIsQuitting = false;
    public static T Inst
    {
		get
		{
            if (m_Instance == null)
			{
                if (m_bApplicationIsQuitting)
                {
                    return null;
                }
#if UNITY_EDITOR 
                m_Instance = new GameObject("SingletonOf" + typeof(T).ToString(), typeof(T)).GetComponent<T>();
#else
                m_Instance = new GameObject(null, typeof(T)).GetComponent<T>();
#endif
                DontDestroyOnLoad(m_Instance);
			}
			return m_Instance;
		}
	}

    public static bool IsApplicationQuit()
    {
        return m_bApplicationIsQuitting;
    }

    /// <summary>
    ///   确保在程序退出时销毁实例。
    /// </summary>
    protected virtual void OnApplicationQuit()
	{
        m_bApplicationIsQuitting = true;
	}
}