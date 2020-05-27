using System ;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// 网络下载模块
/// </summary>
/// <author>zhulin</author>
public class DownLoadTool  : MonoBehaviour{
	/// <summary>
	/// 获取www
	/// </summary>
	private UnityWebRequest m_www;
	public UnityWebRequest www
	{
		get{return m_www ;}
	}
		
	private DownLoadTask m_task = null;



	/// <summary>
	/// 设置下载参数
	/// </summary>
	public void SetDownLoadTask(DownLoadTask task)
	{
		if (task == null)
			return;
		m_task = task;
	}


	void Start () 
	{
		StartCoroutine(DownLoadFiles());
	}
	/// <summary>
	/// 下载文件
	/// </summary>
	private  IEnumerator DownLoadFiles()
	{
		try
		{
			Debug.Log("download:" + m_task.m_ServerPath + m_task.m_ResName);
			m_www = UnityWebRequest.Get(m_task.m_ServerPath + m_task.m_ResName);
            yield return m_www.SendWebRequest();


            if (m_www.isNetworkError)
			{
                Debug.Log(m_task.m_ServerPath + m_task.m_ResName);
				DownLoadFailHook(www.error);
			}
			else if (m_www.downloadedBytes > 0)
			{
				SaveFile(m_www);
			}
            m_www.Dispose();

        } finally {
		}
	}

	/// <summary>
	/// 下载失败
	/// </summary>
	private void DownLoadFailHook(string err)
	{
		Debug.Log("err:" + err);
		RemoveDownTask() ;
	}

	/// <summary>
	/// 保存文件
	/// </summary>
	private void SaveFile(UnityWebRequest www)
	{
		if (m_task != null) 
		{
			File.WriteAllBytes(m_task.m_localPath + m_task.m_ResName, www.downloadHandler.data);
			if(m_task.Progress != 1.0f)
			   m_task.Progress = 1.0f;
		}
		RemoveDownTask();
	}


	private void RemoveDownTask()
	{
		if (m_task != null) 
		{
			DownLoadQueue.FinishTask(m_task.m_DownID);
		}
		Destroy(this);
	}
		
	void Update()
	{
		if(www != null && m_task != null && m_task.Progress != 1.0f)
		{
			if(www.downloadProgress <1.0f)
			   m_task.Progress = www.downloadProgress;
		}
	}




}
