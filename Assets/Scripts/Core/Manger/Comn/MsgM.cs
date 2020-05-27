using System;
using System.Collections.Generic;
using IGG.Core.Data.DataCenter;

namespace IGG.Core
{
	/// <summary>
	/// 消息处理 
	/// </summary>
	public class MsgM
	{
		private static uint g_UniqueID = 0;
		private static int g_CurNum = -1;
		private static int PackageSize = 0;
		private static List<byte[]> g_ListContent = new List<byte[]>();
		/// <summary>
		/// 消息处理入口 
		/// </summary>
		/// <param name="cmd">消息ID</param>
		/// <param name="para">消息参数</param>
		public static bool Dispatch(eMsgTypes Type, byte[] data, int index, int length)
		{
            //处理消息
            Execute(Type, data, index, length);
            return true;
		}
		// 执行命令
		public static void Execute(eMsgTypes Type, byte[] data, int index, int length)
		{
			DataCenter.ProcessData(Type, data, index, length);
		}
        
		private static void Clear() {
			g_UniqueID = 0;
			g_CurNum = -1;
			PackageSize = 0;
			g_ListContent.Clear();
		}
	}
}
