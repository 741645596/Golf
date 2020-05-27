using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 加载队列管理
/// </summary>
/// <author>zhulin</author>
public class LoadQueue  {
	//最大支持的加载数量
	private static int g_MaxTask = 8;
	// 加载任务列表
	private static List<LoadTask> g_LoadTask = new List<LoadTask>();

	public static void AddLoadTask(string ABRelativePath, 
		string ObjName, 
		System.Type type, 
		bool IsCacheAsset,
		bool IsCacheAB, 
		bool IsFreeUnUseABRes,
		AssetLoadHook pfun){
		LoadTask info = new LoadTask(ABRelativePath, ObjName, type, IsCacheAsset, IsCacheAB, IsFreeUnUseABRes, pfun);
		AddLoadQueue(info);
	}

	private static void AddLoadQueue(LoadTask info){
		g_LoadTask.Add(info);
		StartRunTask();
	}
		
	// 开始加载任务
	public static void StartRunTask() {

		List<LoadTask> lRun = GetRunTask();
		int downing = lRun.Count;
		if (g_LoadTask.Count < 1 || downing >= g_MaxTask)
			return;
		List<LoadTask> lunRun = GetUnRunTask();
		// 添加加载任务
		foreach (LoadTask Task in lunRun) 
		{
			if (Task == null || Task.IsRun == true)
				continue;
			//
			if (downing < g_MaxTask) 
			{
				if (CheckCanRun(lRun, Task) == true) {
					Task.StartTask();
					downing++;
					lRun.Add(Task);
				}
			} 
			else
				break;
		}
	}

	// 确定该任务能否现在执行，主要是担心2个相同任务加载同一个ab 包，或依赖ab 包的问题。
	private static bool CheckCanRun(List<LoadTask> lRun, LoadTask t){
		if (t == null)
			return false;
		
		foreach (LoadTask Task in lRun) 
		{
			if (Task != null && Task.IsRun == true) {
				if (Task.EqualRunTask (t) == true)
					return false;
			}
		}
		return true;
	}

	// 获取未运行的任务
	private static List<LoadTask> GetUnRunTask(){
		List<LoadTask> l = new List<LoadTask>();
		foreach (LoadTask Task in g_LoadTask) 
		{
			if (Task != null && Task.IsRun == false) {
				l.Add(Task);
			}
		}
		return l;
	}

	//获取已运行的任务
	private static List<LoadTask> GetRunTask(){
		List<LoadTask> l = new List<LoadTask>();
		foreach (LoadTask Task in g_LoadTask) 
		{
			if (Task != null && Task.IsRun == true) {
				l.Add(Task);
			}
		}
		return l;
	}


	// 
	public static void NotifyTaskFinish(LoadTask t){
		if (t != null) {
			g_LoadTask.Remove (t);
		}
		StartRunTask();
	}

}

// 加载任务
public class  LoadTask {
	public string m_ABRelativePath;
	public string m_ObjName;
	public System.Type m_type;
	public bool m_IsCacheAsset;
	public bool m_IsCacheAB;
	public bool m_IsFreeUnUseABRes;
	public AssetLoadHook m_fun;
	private bool  m_IsRun = false;
	public bool IsRun
	{
		get{ return m_IsRun;}
	}
		
	public LoadTask(string ABRelativePath,
		string ObjName,
		System.Type type,
		bool IsCacheAsset,
		bool IsCacheAB, 
		bool IsFreeUnUseABRes,
		AssetLoadHook pfun) {

		m_ABRelativePath = ABRelativePath;
		m_ObjName = ObjName;
		m_type = type;
		m_IsCacheAsset = IsCacheAsset;
		m_IsCacheAB = IsCacheAB;
		m_IsFreeUnUseABRes = IsFreeUnUseABRes;
		m_fun = pfun;
	}


	/// <summary>
	/// 开始任务
	/// </summary>
	public void StartTask()
	{
		m_IsRun = true;
		/*ResourceManger.AsyncGo.StartCoroutine(ABLoad.LoadABasync(m_ABRelativePath, m_IsCacheAB,
			(ab)=>{
				AssetLoadRun run = ResourceManger.gAssetLoad as  AssetLoadRun;
				ResourceManger.AsyncGo.StartCoroutine(run.LoadObjAsync(ab, m_ObjName, m_type, m_IsCacheAB, m_IsFreeUnUseABRes,
					(g)=>{
						run.LoadAssetCallBack(g, m_ABRelativePath + m_ObjName, m_IsCacheAsset, m_fun);
						FinishTask();
					}));
			}));*/
	}


	private void FinishTask(){
		Debug.Log("加载完成！");
		LoadQueue.NotifyTaskFinish(this);
	}
		
	/// <summary>
	///  正在运行的任务不能为相同的ab包。
	/// </summary>
	public bool EqualRunTask(LoadTask task)
	{
		if (task == null)
			return false;
		if (string.Compare (this.m_ABRelativePath, task.m_ABRelativePath) == 0)
			return true;
		
		return false;
	}
}

