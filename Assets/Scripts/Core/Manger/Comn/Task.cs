/// <summary>
/// 任务处理，一般用在回调中 
/// </summary>
public class Task
{
	/// <summary>
	/// 任务处理的代理 
	/// </summary>
	public delegate void taskFunc(object para, object[] list);
	
	private taskFunc cb;
	private object para;
	
	public Task(taskFunc cb)
	{
		this.cb = cb;
	}
	
	public Task(taskFunc cb, object para)
	{
		this.cb = cb;
		this.para = para;
	}
	
	/// <summary>
	/// 调度入口 
	/// </summary>
	public void Go(params object[] list)
	{
		cb(this.para, list);
	}
}
