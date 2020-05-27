using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 重连机制  by zhulin
namespace IGG.Core
{

	public class NetReConnet {

		// 重连机制：
		// 发现断线后，静默重连3次
		// 显示重连
		// 服务器故障重连
		// 静默重连

		// 判断网络是否断开
		public static bool CheckNetDisConnect(){
			if (NetConnectState.bNetDisconnect == true)
				return true;
			if (NetConnectState.bKeepLiveOverTime == true)
				return true;
			// 待讨论，
			/*if (g_bNetWorkStateChange == true)
				return true;
			*/

			if (NetConnectState.ContinueSendFail >= 3)
				return true;

			return false;
		}


		public static void HideReConnect(){
			LoginM.ConnectLoginServer();
		}

		// 显式重连
		public static void ShowReConnect(){
			LoginM.ConnectLoginServer();
		}

	}

}


