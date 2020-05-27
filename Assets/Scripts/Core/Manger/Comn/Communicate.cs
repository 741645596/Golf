using System;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core;

namespace IGG.Core
{
	/// <summary>
	/// 负责与GS的通讯逻辑 
	/// </summary>
	public class Communicate
	{		
		// 负责与GS的连接与响应
		private static NetConnectorImpl gsConnector = new NetConnectorImpl();
		private static NetResponsorImpl gsResponsor = new NetResponsorImpl();
		
		static Communicate()
		{
			// 初始化连接信息
			gsResponsor.SetConnector(gsConnector);
			gsConnector.GetConnect().Responser = gsResponsor;
        }
		/// <summary>
		/// 判断目前是否连接到服务器 
		/// </summary>
		public static bool IsConnected()
		{
			return gsConnector.IsConnected();
		}
        /// <summary>
        /// 判断目前是否连接到服务器 
        /// </summary>
        public static bool IsConnectedGS()
        {
            return gsResponsor.IsConnectedGS();
        }
        /// <summary>
        /// gs的连接者 
        /// </summary>
		public static NetConnectorImpl GSConnector
		{
			get { return gsConnector; }
		}

		public static void SetGSConnetorGame(bool isGame)
		{
			gsResponsor.SetConnectGame (isGame);
		}
		/// <summary>
		/// 连接到GS 
		/// </summary>
		public static bool Connect2GS(string ip, int port)
		{
			if (gsConnector.IsConnected())
			{
				return false;
			}

			gsConnector.ConnectToServer(ip, port);
			return true;
		}

        /// <summary>
        /// 断开连接 
        /// </summary>
        public static void Disconnect()
		{
			NetConnectState.ClearMsg();
			if (gsConnector.IsConnected())
				gsConnector.DisconnectFromServer();
        }

        /// <summary>
		/// Send2s the G.
		/// </summary>
		/// <returns><c>true</c>, if G was send2ed, <c>false</c> otherwise.</returns>
		/// <param name="msgNo">Message no.</param>
		/// <param name="data">protobuf object.</param>
		public static bool Send2GS(eMsgTypes type, byte[] msg)
        {
            //IGGDebug.Log("send:" + type);
            NetBuffer Nb = NetCache.GetNetBuffer(msg.Length);
            if (Nb != null)
            {
                Nb.PackBuf(type, msg);
                return gsConnector.SendToServer(Nb);
            }
            else
            {
                return false;
            }
        }
    }
}
