using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System.IO;
using System;

// 网络缓冲池 by zhulin
public class NetCache {
	// 接受缓冲池。
	private static List<NetBuffer> g_SendCache =  null;

	// 缓冲池初始化
	public static void Init(){
		g_SendCache = new List<NetBuffer> (32);
		for (int i = 0; i < 20; i++) {
			g_SendCache.Add(new NetBuffer(1 * 1024));
		}
		for (int i = 0; i < 10; i++) {
			g_SendCache.Add(new NetBuffer(4 * 1024));
		}
		g_SendCache.Add(new NetBuffer(8 * 1024));
		g_SendCache.Add(new NetBuffer(64 * 1024));
	}

	// 获取一个缓冲
	public static NetBuffer GetNetBuffer(int BufferSize){
		for (int i = 0; i < g_SendCache.Count; i++) {
			if (g_SendCache [i].IsUsed == false && BufferSize < g_SendCache [i].BufferSize) {
				return g_SendCache[i];
			}
		}

		IGGDebug.Log("缓冲池开小了，请扩容");
		return null;
	}
}
	

/// <summary>
/// 统计信息
/// </summary>
public class NetBuffer
{
	public byte[] Pbuffer = null; 
	public int HaveSendBytes = 0;



	private int mBufferSize = 0;
	public int BufferSize{
		get{  return mBufferSize;}
	}


	private int BufferLength = 0;




	private bool mIsUsed = false;
	public  bool IsUsed{
		get{ return mIsUsed;}
		
	}


	public int NeedSendBuffSize{
		get {  return BufferLength - HaveSendBytes;}
	}

	public NetBuffer(){
	}


	public NetBuffer(int bufferSize){
		this.mBufferSize = bufferSize;
		this.Pbuffer = new byte[this.BufferSize];
		this.HaveSendBytes = 0;
		this.BufferLength = 0;
	}


	public void SetHaveSendBytes(int SendBytes){
		this.HaveSendBytes += SendBytes;
	}

	public void CGNetBuffer(){
		this.mIsUsed = false;
		this.HaveSendBytes = 0;
		this.BufferLength = 0;
	}


	// 序列化
	public void  PackBuf(eMsgTypes type, byte[] msg)
	{
		ushort msgNo = (ushort)type;

		this.BufferLength = msg.Length + 8;
		// 先封包头2个字节。
		short size = (short)BufferLength;
		short netsize = System.Net.IPAddress.HostToNetworkOrder(size);
		this.Pbuffer[0] = (byte)(netsize >> 8);
		this.Pbuffer[1] = (byte)(netsize);
		// 再封命令ID
		short id = (short)type;
		netsize = System.Net.IPAddress.HostToNetworkOrder(id);
		this.Pbuffer[2] = (byte)(netsize >> 8);
		this.Pbuffer[3] = (byte)(netsize);
		// 再封装协议体
		Array.Copy(msg, 0, this.Pbuffer, 8, msg.Length);
		this.mIsUsed = true;
		this.HaveSendBytes = 0;
	}
}
