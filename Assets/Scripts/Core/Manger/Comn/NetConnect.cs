using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace IGG.Core
{
	/// <summary>
    /// 网络连接通信类
    /// </summary>
    public class NetConnect
    {
		byte[] g_headbuffer = new byte[2];
		byte[] g_recvBuffer = new byte[65536]; 
		// 网络数据的响应者
		NetResponsorImpl _responsor = null;
		
        /// <summary>
        /// 统计信息
        /// </summary>
		int g_TotalReadBytes = 0;

        /// <summary>
        /// 网络数据的响应者 
        /// </summary>
		public NetResponsorImpl Responser
        {
            get { return _responsor; }
            set { _responsor = value; }
        }

		// 处理连接结果
		private void NotifyConnectResult(bool success)
		{
            if (_responsor != null)
            {
                if (success)
                    _responsor.OnConnectSuccess();
                else
                    _responsor.OnConnectFailure();
            }
                
		}

		// 处理断开连接结果
		private void NotifyDisconnectResult(bool success)
		{
            if (_responsor != null)
                _responsor.OnDisconnect();
		}


		private void NotifyPacketResult(byte[] message, int length)
		{
			if (_responsor != null)
				_responsor.OnRead(message, length);
		}

		private void AsyncConnectCallback(IAsyncResult ar)
		{
			Socket s = (Socket) ar.AsyncState;
			try
			{
				s.EndConnect(ar);
			}
			catch (SocketException e)
			{
				IGGDebug.Log(e.ToString());
			}

			if (! mIsConnecting)
			{
				this.Disconnect();
                NotifyConnectResult(false);
				return;
			}

			bool success = s.Connected;
			mIsConnected = success;
			NotifyConnectResult(success);

			if (! success) 
			{
				this.Disconnect ();
			} 
		}
		
		/// <summary>
		/// 异步连接 
		/// </summary>
		/// <param name="ip">目标IP</param>
		/// <param name="port">目标端口</param>
		public void NonblockConnect(string ip, int port)
		{
			if (mSocket == null)
			{
 				mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
				mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 512 * 1024);               
                mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 512 * 1024);
			}

			if (mSocket.Connected)
			{
				// 已经处于连接状态了
				return;
			}

			try
			{
				IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
				mSocket.Blocking = false;
                // 标记正处于连接中
                mIsConnecting = true;
				mSocket.BeginConnect(remoteEndPoint, new AsyncCallback(AsyncConnectCallback), mSocket);

            	
			}
			catch (Exception e)
			{
				IGGDebug.Log(e.ToString());
			}
		}
		
		/// <summary>
		/// 同步连接 
		/// </summary>
		/// <param name="ip">目标IP</param>
		/// <param name="port">目标端口</param>
		public void Connect(string ip, int port)
		{
			if (mSocket == null)
			{
 				mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
				mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 512 * 1024);               
                mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 512 * 1024);
			}

			if (mSocket.Connected)
			{
				return;
			}

        	// 标记正处于连接中
        	mIsConnecting = true;
			mSocket.Blocking = false;
			try
			{
				mSocket.Connect(ip, port);
			}
			catch (SocketException e)
			{
				IGGDebug.Log(e.ToString());
			}

			bool success = mSocket.Connected;
			mIsConnected = success;
			NotifyConnectResult(success);

			if (! success)
			{
				this.Disconnect();
			}
		}

        /// <summary>
        /// 断开当前和服务器的连接。本函数可以被重复调用
        /// </summary>
        public void Disconnect()
        {
            // 断开连接，重置状态
            if ((mSocket != null) && (mIsConnected))
            {
                try
                {
    				mSocket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                    // 这里什么都不干，保证异常发生后下面的代码依然被执行
                }
                mSocket.Disconnect(true);

				mSocket.Close();
				mSocket = null;
            }
            mIsConnected = false;
            mIsConnecting = false;
        }


		public void SendRawPkt(NetBuffer nb)
		{
			//非法判断
			if (mSocket == null)
			{
				IGGDebug.Log("socket is invalid.");
				return;
			}
			
			// 如果根本就没有连接，那么不用发送了
			if (!this.IsConnected)
				return;

			
			// 当前还有没发出去的包，为了保证顺序，我们这次也别尝试了，直接插入到Pending列表中
			if (mPendingSendPkt.Count > 0)
			{
				mPendingSendPkt.Add(nb);
			}
			// 尝试发送一次
			else
			{
				// 尝试发送这个数据包
				try
				{
					
					int sentBytes = mSocket.Send(nb.Pbuffer, nb.HaveSendBytes, nb.NeedSendBuffSize, SocketFlags.None);
					
					// 如果没有完整地把包发送出去
					if (sentBytes < nb.NeedSendBuffSize)
					{
						NetConnectState.SendDataState(false);
					}
					else 
					{
						NetConnectState.SendDataState(true);
					}
					nb.CGNetBuffer();
				}
				catch (Exception e)
				{
					IGGDebug.LogError(e);
				}
			}
		}
		
		/// <summary>
		/// 帧回调函数，每帧都要检查是否有Pending的数据需要发送；是否有数据需要读取。
		/// </summary>
		/// <param name="elapse"></param>
		public void Poll(float elapse)
		{
			if (mSocket == null || !mSocket.Connected)
				return;
			
			if (mSocket.Poll(0, SelectMode.SelectRead) &&
			    (mSocket.Available < 1))
			{
				// 触发断开事件
				this.Disconnect();
				NotifyDisconnectResult(false);
				return;
			}

            // 如果有 Pending 住没有发送的数据，我们尝试发送一下
            TryToSendPendingPkt();

            // 尝试读取数据包
			PeekNetworkMessage ();
        }

        #region 属性块

        /// <summary>
        /// 属性：判断当前是否和服务器处于连接状态
        /// </summary>
        public bool IsConnected { get { return mIsConnected && mSocket.Connected; } }

        /// <summary>
        /// 属性：判断当前是否正处于尝试连接状态中
        /// </summary>
        public bool IsConnecting { get { return mIsConnecting; } }

        #endregion

        #region 私有函数，实现细节
        /// <summary>
        /// 尝试发送 Pending 的数据包，直到不能再发送为止
        /// </summary>
        private void TryToSendPendingPkt()
        {
            while (mPendingSendPkt.Count > 0)
            {
                // 尝试发送
				int sentBytes = mSocket.Send(mPendingSendPkt[0].Pbuffer, mPendingSendPkt[0].HaveSendBytes,  mPendingSendPkt[0].NeedSendBuffSize, SocketFlags.None);

				if (sentBytes < mPendingSendPkt[0].NeedSendBuffSize)
                {
                    // 发送不出去了，更新一下还剩余的数据，然后退出循环发送过程
					mPendingSendPkt [0].SetHaveSendBytes(sentBytes);
                    return;
                }
                else
                {
                    // 这个包完整发送出去了，我们不要退出发送循环，尽量尝试发送更多的数据包

					mPendingSendPkt[0].CGNetBuffer();
                    mPendingSendPkt.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// 读取状态机
        /// </summary>
        private enum ReadState
        {
            WAIT_HEADER,
            WAIT_BODY,
        }

		//探查网络封包，是否有完整的消息到达
		private void PeekNetworkMessage()
		{
			int count = 0;
			while(true)
			{
				// 每次最多只读取一定数量的消息
#if UNITY_ANDROID || UNITY_IPHONE
				if (count >= 2)
#else
				if (count >= 10)
#endif
					break;

				if(mReadState == ReadState.WAIT_HEADER)
				{
					if (mSocket == null || mSocket.Available < 2)
						break;
					// 读取两个个字节的数据
					int realRead = mSocket.Receive(g_headbuffer, 0, 2, SocketFlags.None);
					System.Diagnostics.Debug.Assert(realRead == 2);

					//大小端转换
					mMsgWaitBodyBytes = (ushort)g_headbuffer[0];
					mMsgWaitBodyBytes += (ushort)(g_headbuffer[1] << 8);

                    mMsgWaitBodyBytes -= 2;

                    mReadState = ReadState.WAIT_BODY;
					g_TotalReadBytes = 0;
					//mMsgBodyData = null;
				}
				else if(mReadState == ReadState.WAIT_BODY)
				{
					if (mSocket.Available <= 0 && mMsgWaitBodyBytes > 0)
						// 没有数据可以读
						return;
					ushort realRead = 0;
					if (mMsgWaitBodyBytes > 0)
					{
						// 读取我们希望等待的消息体
						realRead = (ushort)mSocket.Receive(g_recvBuffer, mMsgWaitBodyBytes, SocketFlags.None);

						if(realRead > 0)
						{
							NetUtil.ByteArrayTake(g_recvBuffer, mMsgBodyData, g_TotalReadBytes, realRead);
							g_TotalReadBytes += realRead;
							mMsgWaitBodyBytes -= realRead;
						}

						if(mMsgWaitBodyBytes == 0)
						{
							// 切换到初始状态，处理下一个包
							mReadState = ReadState.WAIT_HEADER;
							
							count++;

							NotifyPacketResult(mMsgBodyData, g_TotalReadBytes);


						}
					}

				}
				else
				{
				}
			}
		}



        #endregion

        /// <summary>
        /// 当前是否处于连接状态
        /// </summary>
        private bool mIsConnected = false;

        /// <summary>
        /// 当前是否处于尝试连接中
        /// </summary>
        private bool mIsConnecting = false;

        #region 发送包相关逻辑的数据

        /// <summary>
        /// 当前pending住没有发送出去的包列表
        /// </summary>
		private List<NetBuffer> mPendingSendPkt = new List<NetBuffer>();

        #endregion

        #region 读取包相关逻辑的数据

        /// <summary>
        /// 读取状态
        /// </summary>
        private ReadState mReadState = ReadState.WAIT_HEADER;
        /// <summary>
        /// 读取消息体
        /// </summary>
		private byte[] mMsgBodyData = new byte[65536];

        /// <summary>
        /// 等待的消息体还有多少个字节
        /// </summary>
        private ushort mMsgWaitBodyBytes = 0;
        #endregion

        /// <summary>
        /// 用于通信的socket对象
        /// </summary>
        private Socket mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
    }}
