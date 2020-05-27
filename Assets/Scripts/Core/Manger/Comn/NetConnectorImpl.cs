using UnityEngine;
using System;
using System.Collections.Generic;

namespace IGG.Core
{
	/// <summary>
	/// 网络连接请求者的实现 
	/// </summary>
	public class NetConnectorImpl 
	{
		// 连接实例
		private NetConnect connect = new NetConnect();
        /// <summary>
        /// 连接对象 
        /// </summary>
        public NetConnect GetConnect() { return this.connect; }
		public void SetConnect(NetConnect connect)
		{
			this.connect = connect;
		}
		
		/// <summary>
		/// 是否处于连接中 
		/// </summary>
		/// <returns>如果是连接中则返回true</returns>
		public bool IsConnected()
		{
			return this.connect.IsConnected;
		}
		
		/// <summary>
		/// 连接到服务器 
		/// </summary>
		/// <param name="ip_address">IP地址</param>
		/// <param name="port">端口号</param>
		public void ConnectToServer(string ip_address, int port)
		{
			this.connect.NonblockConnect(ip_address, port);
		}
		
		/// <summary>
		/// 从服务器断开连接 
		/// </summary>
		public void DisconnectFromServer()
		{
			this.connect.Disconnect();
		}
		/// <summary>
		/// 发送字节流到服务端
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="data">Data.</param>
		public bool SendToServer(NetBuffer pbuff)
		{
			if (! IsConnected() || pbuff == null)
				return false;
			
			try
			{
				this.connect.SendRawPkt(pbuff);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// 每帧需要调度，以对网络数据进行处理 
		/// </summary>
		/// <param name="deltaTime">帧与帧之间的流逝时间</param>
		public void Update(float deltaTime)
		{
			// 处理消息的读取和发送
			this.connect.Poll(deltaTime);
			NetConnectState.Update(deltaTime);
		}
	}
}
