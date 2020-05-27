using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using IGG.Core;
using IGG.Core.Data.DataCenter;
using IGG.Core.Data.DataCenter.Login;
using System.Net;
using System.Text;

public class LoginM {

	private static string g_strUserName;
	private static string g_strPassWorld;
	private static string g_ip;
	private static int g_port;

	private static ConnectFail g_failLiknfp = null;


	public static void InitFailNetLink(ConnectFail pfun)
	{
		g_failLiknfp = pfun;
	}

	// 设置网络信息
	public static void SetNetInfo(string ip, int port){
		g_ip = ip;
		g_port = port;
	}

	// 设置账号信息
	public static void SetUserInfo(string userName, string PassWorld){
		g_strUserName = userName;
		g_strPassWorld = PassWorld;
	}

	// 服务器列表
	private static List<ServerInfo> m_ServerList = new List<ServerInfo>();
	public static List<string> GetServerListName(){
		List<string> lServerNmae = new List<string>();
		for (int i = 0; i < m_ServerList.Count; i ++) {
			lServerNmae.Add(m_ServerList[i].ServerName);
		}
		return lServerNmae;
	}

	// 根据名称获取服务器信息
	public static ServerInfo GetServerInfo(string ServerName){
		for (int i = 0; i < m_ServerList.Count; i ++) {
			if (ServerName == m_ServerList[i].ServerName) {
				return m_ServerList[i];
			}
		}
		return null;
	}

    // 获取第一个服务器配置信息
    public static ServerInfo GetServerInfo()
    {
        if (m_ServerList.Count > 0)
        {
            return m_ServerList[0];
        }
        return null;
    }

    // 零时用途
    public delegate void ConnectFail();


	private static ConnectFail g_failfp = null;


	private static bool g_HaveInit = false;
	// 只被初始化一次
	public static void Init(ConnectFail fp){
		g_failfp = fp;
		if (g_HaveInit == false) {
			NetConnectState.eventDisconnect += OnDisConnect;
			NetConnectState.eventConnect += OnConnect;
			RegisterHooks();
			g_HaveInit = true;
		}
	}


	// 清理数据
	public static void Clear()
	{
		NetConnectState.eventDisconnect -= OnDisConnect;
		NetConnectState.eventConnect -= OnConnect;
		AntiRegisterHooks();
		g_HaveInit = false;
	}


	// 登录服务器
	public static void ConnectLoginServer()
	{
		// 发送登录请求
		if (Communicate.IsConnected() == false)
		{
			Communicate.Disconnect();
			Communicate.SetGSConnetorGame(false);
			NetConnectState.SetConnetOverTime(3.0f, OnConnectOverTime);
            //Communicate.Connect2GS(g_ip, g_port);
		    ServerInfo sInfo = GetServerInfo();
		    if (null != sInfo)
		    {
		        Communicate.Connect2GS(sInfo.iP, sInfo.Port);
		    }
		    else
		    {
                IGGDebug.Log("未获取登入服务器ip");
            }
		}
	}

    // 游戏服务器
    public static void ConnectGameServer()
	{
        ProtoMsg.MsgNetAddress address = LoginDC.GetGameServerNetInfo();
		if (address == null) {
			IGGDebug.Log("未获取游戏服务器ip");
			return;
		}
        // 发送登录请求
        Communicate.Disconnect();
        if (Communicate.IsConnected() == false)
		{
			Communicate.SetGSConnetorGame(true);
			NetConnectState.SetConnetOverTime(3.0f, OnConnectOverTime);

            string ip = address.u32Ip.ToString();
            if (!isDomainAddrIp(address.u32Ip.ToString().ToCharArray()))
            {
                // data.netAdd.ip  转  ip 地址
                ip = IntToIp((Int32)address.u32Ip);
            }

            IGGDebug.Log("游戏服务器:" + ip + "," + address.u32Port);
			Communicate.Connect2GS(ip, (int)address.u32Port);
		}
	}


	private static void OnConnectOverTime(){
		IGGDebug.Log("连接超时");
		if (g_failfp != null) {
			g_failfp();
			g_failfp = null;
		}
	}

	private static void OnDisConnect()
	{
		IGGDebug.Log("失去连接");
		if (g_failLiknfp != null) {
			g_failLiknfp();
		}
	}


	private static void OnConnect(bool succ)
	{
		if (succ == true) {

			if (Communicate.IsConnectedGS () == true) {
				IGGDebug.Log ("连接游戏服务器");
			    LoginDC.SendGSMsgCL2GS();
			} else {
				IGGDebug.Log ("连接登录服务器");
                LoginDC.SendLoginMsgCL2LS();
			}
		} else {
			IGGDebug.Log("连接服务器失败");
			if (g_failfp != null) {
				g_failfp();
				g_failfp = null;
			}
		}
	}


	private static void RegisterHooks()
	{
		
	}

	private static void AntiRegisterHooks()
	{
        
	}

	public static bool LoadServerListFromText(string Path)
	{
		m_ServerList.Clear();
		StreamReader sr ;
		try{
			sr = File.OpenText(Path);
		}
		catch(Exception e)
		{
			//路径与名称未找到文件则直接返回空
			return false;
		}

		string line;
		Server_Info Info = new Server_Info();
		while ((line = sr.ReadLine()) != null)
		{
			if(line.StartsWith("ServerName"))
			{
				Info.ServerName = line.Replace("ServerName = ","");
			}
			if(line.StartsWith("ServerId"))
			{
				Info.ServerID = int.Parse(line.Replace("ServerId = ",""));
			}
			if(line.StartsWith("ServerIp"))
			{
				Info.iP = line.Replace("ServerIp = ","");
			}
			if(line.StartsWith("ServerPort"))
			{
				Info.Port = int.Parse(line.Replace("ServerPort = ",""));
				ServerInfo v = new ServerInfo();
				v.iP = Info.iP;
				v.Port = Info.Port;
				v.ServerID = Info.ServerID;
				v.ServerName = Info.ServerName;
				m_ServerList.Add(v);
			}
		}

		//关闭流
		sr.Close();
		//销毁流
		sr.Dispose();
		return true;
	}

    public static bool isDomainAddrIp(char[] sAddrIP)
    {
        // 检测字符串是不是IPV4的格式

        bool isDomainIp = false;
        int nSize = sAddrIP.Length;
        for (int i = 0; i < nSize; ++i)
        {
            if (char.IsLetter(sAddrIP[i]))
            {
                isDomainIp = true;
                break;
            }
        }

        return isDomainIp;
    }
    public static string IntToIp(Int32 ipInt)
    {
        uint netInt = (uint)IPAddress.HostToNetworkOrder(ipInt);
        IPAddress ipaddr = new IPAddress((long)netInt);

        string[] str = ipaddr.ToString().Split('.');
        string ip = "";
        for (int i = str.Length - 1; i >= 0; --i)
        {
            ip += str[i];
            if (i != 0)
            {
                ip += ".";
            }
        }

        return ip;

        StringBuilder sb = new StringBuilder();
        sb.Append((ipInt >> 24) & 0xFF).Append(".");
        sb.Append((ipInt >> 16) & 0xFF).Append(".");
        sb.Append((ipInt >> 8) & 0xFF).Append(".");
        sb.Append(ipInt & 0xFF);
        return sb.ToString();
    }
}



public class ServerInfo  {

	public int ServerID;
	public string ServerName;
	public string iP;
	public int  Port; 
}

public struct Server_Info  {

	public int ServerID;
	public string ServerName;
	public string iP;
	public int  Port; 
}