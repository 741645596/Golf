using System;

namespace IGG.Core
{
	/// <summary>
	/// 实现网络响应者接口 
	/// </summary>
	public class NetResponsorImpl 
	{
		// 连接请求者
		private NetConnectorImpl connector = null;

		private bool isGameServer = false;
		/// <summary>
		/// 连接请求者 
		/// </summary>
		public NetConnectorImpl GetConnector() { return this.connector; }
		public void SetConnector(NetConnectorImpl connector)
		{
			this.connector = connector;
		}

		public void SetConnectGame(bool isGame)
		{
			this.isGameServer = isGame; 
		}


        public bool IsConnectedGS() {
            return isGameServer;
        }

        /// <summary>
        /// 连接成功的处理
        /// </summary>
        public void OnConnectSuccess()
		{
			NetConnectState.OnConnectNotify(true);
        }
		
		/// <summary>
		/// 连接失败的处理
		/// </summary>
        public void OnConnectFailure()
		{
			NetConnectState.OnConnectNotify(false);
		}
		
		/// <summary>
		/// 掉线的处理
		/// </summary>
        public void OnDisconnect()
		{
			NetConnectState.OnDisconnect();
		}


		public void OnRead(byte[] message, int length)
		{
			ProcessPacket (message, length);
		}

		private void ProcessPacket(byte[] message, int length)
		{
			//byte[] data = new byte[length - 2]; 
			//Array.Copy(message, 2, data, 0, length - 2);

            //大小端转换
            ushort cmd = (ushort)message[0];
            cmd += (ushort)(message[1] << 8);

            eMsgTypes Type = (eMsgTypes)cmd;
			//IGGDebug.Log("Revice:" + Type);
			bool result = MsgM.Dispatch(Type, message, 6, length - 6);
			if (result == true) {
				NetConnectState.MsgArrival(cmd);
			}
		}

	}
}
