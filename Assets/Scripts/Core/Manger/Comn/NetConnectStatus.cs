using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;


// 网络连接状态 by zhulin
namespace IGG.Core
{

	public delegate void ConnectOverTimeCallback();

	public class NetConnectState {

		// 连接特定msg id
		private const  int g_ConnectMsgID = 999999;

		// sockete层面判断 网络断开
		private static bool g_bNetDisconnect = false;
		public static bool bNetDisconnect{
			get{ return g_bNetDisconnect;}
		}

		// 连续发送失败数据次数
		private static int g_ContinueSendFail = 0; 
		public static int ContinueSendFail{
			get{ return g_ContinueSendFail;}
		}

		// 心跳包超时
		private static bool g_bKeepLiveOverTime = false;
		public static bool bKeepLiveOverTime{
			get{ return g_bKeepLiveOverTime;}
		}

		// 网络状态切换
		private static bool g_bNetWorkStateChange = false;
		public static bool bNetWorkStateChange{
			get{ return g_bNetWorkStateChange;}
		}

		// 等待消息的列表
		private static Dictionary<int, WaitMsgNode> msgWainList = new Dictionary<int, WaitMsgNode>();

		// 消息到达队列
		private static List<int> arrivalList = new List<int>();
		/// <summary>
		/// 掉线的回调处理
		/// </summary>
		public delegate void DisconnectHook();
		public static event DisconnectHook eventDisconnect;

		/// <summary>
		/// 连接状态的回调
		/// </summary>
		public delegate void ConnectCallback(bool success);
		public static event ConnectCallback eventConnect;
		/// <summary>
		/// 超时连接
		/// </summary>
		private static ConnectOverTimeCallback g_eventConnectOverTime = null;
		/// <summary>
		/// 每帧需要调度，以对网络数据进行处理 
		/// </summary>
		/// <param name="deltaTime">帧与帧之间的流逝时间</param>
        private static List<int> expireList = new List<int>();
        public static void Update(float deltaTime)
		{
			int i = 0;
			// 处理已经到达的消息
			for (i = 0; i < arrivalList.Count; i++)
			{
				int  msg = arrivalList[i];

				if (! msgWainList.ContainsKey(msg))
					// 无人等待，直接忽略掉
					continue;

				WaitMsgNode node = msgWainList[msg];
				node.OnSuccess();
				msgWainList.Remove(msg);
			}
			arrivalList.Clear();

            // 处理超时时间
            expireList.Clear();

			Dictionary<int, WaitMsgNode>.Enumerator iter = msgWainList.GetEnumerator();
            while (iter.MoveNext ()){
				WaitMsgNode node = iter.Current.Value;
				if (node.IsExpired ())
					expireList.Add (iter.Current.Key);
			}
			iter.Dispose();


			for (i = 0; i < expireList.Count; i ++)
			{
				WaitMsgNode node = msgWainList[expireList[i]];
				node.OnFail();
				msgWainList.Remove(expireList[i]);
			}
		}
		/// <summary>
		/// 取消消息等待
		/// </summary>
		/// <param name="msg"> 待取消的消息名称</param>
		public static void RemoveWaitMsg(int msg)
		{
			if (! msgWainList.ContainsKey(msg))
				// 无人等待此消息
				return;
			msgWainList.Remove(msg);
		}

		/// <summary>
		/// 清空繁存数据 
		/// </summary>
		public static void ClearMsg()
		{
			arrivalList.Clear();
			msgWainList.Clear ();

		}

		/// <summary>
		/// 等待某消息到达 
		/// </summary>
		/// <param name="msg">等待的消息名称</param>
		/// <param name="waitTime">超时时间</param>
		/// <param name="success">成功的回调</param>
		/// <param name="fail">失败的回调</param>
		/// <returns>操作成功则返回true</returns>
		public static bool WaitMsgArrival(int MsgID, float waitTime, taskFunc success, taskFunc fail)
		{
			if (msgWainList.ContainsKey(MsgID))
			{
				RemoveWaitMsg(MsgID);
			}
			// 记录起来
			WaitMsgNode node = new WaitMsgNode(waitTime, success, fail);
			msgWainList.Add(MsgID, node);

			return true;
		}


		/// <summary>
		/// 消息到达的处理 
		/// </summary>
		/// <param name="msg">到达的消息</param>
		public static void MsgArrival(int msg)
		{
			eMsgTypes Type = (eMsgTypes)msg;
			//IGGDebug.Log("MsgArrival prev:" + Type);
			if (! msgWainList.ContainsKey(msg))
			{
				// 无人等待此消息
				return;
			}
			//IGGDebug.Log("MsgArrival:" + Type);
			// 添加到列表中
			arrivalList.Add(msg);
		}

		/// <summary>
		/// 掉线的处理
		/// </summary>
		public static void OnDisconnect()
		{
			g_bNetDisconnect = true;
			// 回调
			if (eventDisconnect != null)
				eventDisconnect();
		}
		/// <summary>
		/// 连接状态的处理
		/// </summary>
		public static void OnConnectNotify(bool success)
		{
			MsgArrival(g_ConnectMsgID);
			// 回调
			if (eventConnect != null)
				eventConnect(success);
		}

		/// <summary>
		/// 设置连接超时处理
		/// </summary>
		public static void SetConnetOverTime(float time, ConnectOverTimeCallback pf){
			g_eventConnectOverTime = pf;
			WaitMsgArrival(g_ConnectMsgID, time, null, ConnectOverTimeTask);
		}

		/// <summary>
		/// 连接超时处理
		/// </summary>
		private static void ConnectOverTimeTask(){
			if (g_eventConnectOverTime != null) 
				g_eventConnectOverTime();
			}

		// 网络连接方式切换
		public static void NetWorkStateChange(NetworkReachability oldState , NetworkReachability  newState){
			IGGDebug.Log("网络连接方式切换");
			g_bNetWorkStateChange = true;
		}


		// 网络断开
		public static void NetDisConnect(){
			g_bNetDisconnect = true;
		}


		// 心跳包反馈超时。
		public static void KeepLiveOverTime(){
			//IGGDebug.Log("心跳包超时");
		}

		// 数据包反馈超时
		public static void WaitDataRespOverTime(int cmd){
			IGGDebug.Log("数据反馈包超时");
		}


		// 发送数据包状况， true 失败， false ： 失败
		public static void SendDataState(bool succ){
			if (succ == false) {
				g_ContinueSendFail++;
			} else {
				g_ContinueSendFail = 0;
			}

		}
			
		// 清理网络数据
		private static void EmptyNetData(){
			g_bNetDisconnect = false;
			g_ContinueSendFail = 0; 
			g_bKeepLiveOverTime = false;
			g_bNetWorkStateChange = false;
		}


		}
}



