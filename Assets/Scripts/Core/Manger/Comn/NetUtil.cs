using System;
using System.Collections.Generic;

namespace IGG.Core
{
	/// <summary>
	/// 网络相关的工具 
	/// </summary>
	public class NetUtil
	{
		/// <summary>
		/// 抽取子字节串
		/// </summary>
		public static void ByteArrayTake(byte[] srcBytes, byte[] destBytes, int destIndex, int takeCount)
		{
			if ((takeCount < 0) || (srcBytes.Length < takeCount))
			{
				throw new Exception(string.Format("Bad ByteArrayTake {0} {1}.", srcBytes.Length, takeCount));
			}
			try
			{
				Array.Copy(srcBytes, 0, destBytes, destIndex, takeCount);
			}
			catch (Exception e)
			{
				IGGDebug.Log(e.ToString());
			}
		}
	}
}
