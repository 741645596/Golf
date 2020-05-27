using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 下载管理
/// </summary>
/// <author>zhulin</author>
public delegate void DownLoadProgress(int downid,string ResourceName,float progress );
public delegate void DownLoadFinishNotify(string ResourceName, int UnDownLoadNum);

public class DownLoadQueue  {
	/// <summary>
	/// 分配下载任务id
	/// </summary>
	private static int g_DownID = 0;
	public  static int DownID
	{ get { return g_DownID++;}}
	/// <summary>
	/// 同时最大下载任务
	/// </summary>
	private static int MaxTask = 1;
	/// <summary>
	/// 下载任务
	/// </summary>
	public static List<DownLoadTask> m_lTask = new List<DownLoadTask>();
    /// <summary>
    /// 添加下载列表
    /// </summary> 
    private static DownLoadFinishNotify m_funNotifyFinish = null;
    public static void SetFinishNotify(DownLoadFinishNotify fun) {
        m_funNotifyFinish = fun;
    }
    public static void AddDownTask(DownLoadTask Task)
	{
		if (Task == null)
			return;
		if (m_lTask.Contains (Task) == true)
			return;
		m_lTask.Add(Task);
	}
	/// <summary>
	/// 开始下载
	/// </summary>
	public static void StartDown()
	{
		int downing = GetDowning();
		if (downing >= MaxTask)
			return;
		foreach (DownLoadTask Task in m_lTask) 
		{
			if (Task == null)
				continue;
			if (Task.IsStartDown == false) 
			{
				if (downing < MaxTask) 
				{
					Task.StartDown ();
					downing++;
				} 
				else
					break;
			}
		}
	}
	/// <summary>
	/// 获取正在下载的数量
	/// </summary>
	private static  int GetDowning()
	{
		int downing = 0;
		foreach (DownLoadTask Task in m_lTask) 
		{
			if ( Task != null && Task.IsStartDown)
				downing++;
		}
		return downing;
	}
	/// <summary>
	/// 完成指定下载任务
	/// </summary>
	public static void FinishTask(int downid)
	{
		DownLoadTask task = FindTask(downid);
		if (task == null)
			return;
        string ResourceName = task.m_ResName;
		m_lTask.Remove (task);
        if (m_funNotifyFinish != null)
        {
            m_funNotifyFinish(ResourceName, m_lTask.Count);
        }
        StartDown ();
	}
	/// <summary>
	/// 完成指定下载任务
	/// </summary>
	private static  DownLoadTask FindTask(int downid)
	{
		foreach (DownLoadTask Task in m_lTask) 
		{
			if (Task != null && Task.m_DownID == downid) 
			{
				return Task;
			}
		}
		return null;
	}

	/// <summary>
	/// 完成指定下载任务
	/// </summary>
	public static  DownLoadTask FindSameTask(DownLoadTask task)
	{
		foreach (DownLoadTask Task in m_lTask) 
		{
			if (Task != null && Task.EqualTask(task) == true) 
			{
				return Task;
			}
		}
		return null;
	}

	/// <summary>
	/// 清空下载列表
	/// </summary>
	public static void EmptyTask()
	{
		m_lTask.Clear();
        m_funNotifyFinish = null;

    }
	/// <summary>
	/// 确认是否下载完成
	/// </summary>
	public static bool CheckDownLoadFinish()
	{
		if(m_lTask  == null)
			return false;

		if(m_lTask .Count == 0)
			return true;
		else return false;
	}
}

/// <summary>
/// 下载任务
/// </summary>
public class DownLoadTask  
{
	public int m_DownID = 0;
	public string  m_ServerPath= string.Empty;
	public string  m_localPath = string.Empty;
	public string  m_ResName = string.Empty;
	private bool   m_IsStartDown = false;
	public bool    IsStartDown
	{
		get{ return m_IsStartDown;}
	}
	/// <summary>
	/// 下载出发函数
	/// </summary>	
	private DownLoadProgress m_funPress = null;
	public  DownLoadProgress FunPress
	{
		get{ return m_funPress;}
		set{m_funPress = value;}
	}
	/// <summary>
	/// 下载进度
	/// </summary>
	private float m_Progress = 0.0f;
	public float Progress
	{
		get{ return m_Progress;}
		set{
			m_Progress = value;
			if (m_funPress != null) 
			{
				m_funPress (m_DownID, m_ResName, m_Progress);
			}
		}
	}
	public DownLoadTask() 
	{
		this.m_ServerPath = string.Empty;
		this.m_localPath = string.Empty;
		this.m_ResName = string.Empty;
		this.m_DownID = DownLoadQueue.DownID;
		this.m_funPress = null;
	}
	public DownLoadTask(string ServerPath,string localPath,string ResName ,DownLoadProgress func) 
	{
		this.m_ServerPath = ServerPath;
		this.m_localPath = localPath;
		this.m_ResName = ResName;
		this.m_DownID = DownLoadQueue.DownID;
		this.m_funPress = func;
	}

	/// <summary>
	///  下载任务相同
	/// </summary>
	public bool EqualTask(DownLoadTask task)
	{
		if (task == null)
			return false;
		if (string.Compare (this.m_ResName, task.m_ResName) != 0)
			return false;

		if (string.Compare (this.m_localPath, task.m_localPath) != 0)
			return false;

		if (string.Compare (this.m_ServerPath, task.m_ServerPath) != 0)
			return false;

		return true;
	}

	/// <summary>
	/// 下载任务
	/// </summary>
	public void StartDown()
	{
		DownLoadTool tool = SceneM.Schedulergo.AddComponent<DownLoadTool> ();
		if (tool != null) 
		{
			m_IsStartDown = true;
			tool.SetDownLoadTask (this);
		}
	}
}
