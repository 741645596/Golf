using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;
using ProtoMsg;

namespace IGG.Core.Data.DataCenter.Login
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.1.30
    /// Desc    Login模块动态数据集
    /// </summary>
    public static class LoginDC
    {
        //下面定义Login模块需要用到的所有动态数据字段
        private static ProtoMsg.MsgCl2lgLoginReply g_LSLogin = new ProtoMsg.MsgCl2lgLoginReply();
        private static ProtoMsg.MsgCl2gsLoginReply g_GSLogin = new ProtoMsg.MsgCl2gsLoginReply();
        
        /// <summary>
        /// 模拟数据
        /// </summary>
        public static void SimulationData()
        {
        }
        
        // 清理数据中心数据
        public static void ClearDC()
        {
            g_LSLogin = null;
            g_GSLogin = null;
        }
        
        // 获取游戏服务器信息
        public static ProtoMsg.MsgNetAddress GetGameServerNetInfo()
        {
            return g_LSLogin.netAddr;
        }
        
        /// <summary>
        ///   链接LS成功后 登入流程 开启
        /// </summary>
        public static void SendLoginMsgCL2LS()
        {
            ProtoMsg.MsgCl2lgRegist requestRegist = new ProtoMsg.MsgCl2lgRegist();
            MsgRegServerInfo info = new MsgRegServerInfo();
            MsgNetAddress andress = new MsgNetAddress();
            andress.u32Port = 0;
            andress.u32Ip = 0;
            
            info.uType = 1;
            info.uServerId = 0;
            info.netAddr = andress;
            
            requestRegist.Info = info;
            Communicate.Send2GS(eMsgTypes.MsgCl2lgRegist, protobufM.Serializerobject<ProtoMsg.MsgCl2lgRegist>(requestRegist));
            
            ProtoMsg.MsgCl2lgLogin requestLogin = new ProtoMsg.MsgCl2lgLogin();
            requestLogin.strWebSession = "testtest";
            requestLogin.strAccount = "6685";
            Communicate.Send2GS(eMsgTypes.MsgCl2lgLogin, protobufM.Serializerobject<ProtoMsg.MsgCl2lgLogin>(requestLogin));
        }
        
        
        /// <summary>
        ///   请求登入回调
        /// </summary>
        public static void RecvMsgLS2CLLoginReply(eMsgTypes Type, object Info)
        {
            if (Info == null) {
                return;
            }
            g_LSLogin = Info as ProtoMsg.MsgCl2lgLoginReply;
            IGGDebug.Log("连接登入服务器成功");
            // 请求链接GS
            LoginM.ConnectGameServer();
        }
        
        /// <summary>
        ///   链接LS成功后 登入流程 开启
        /// </summary>
        public static void SendGSMsgCL2GS()
        {
            ProtoMsg.MsgCl2gsRegist requestRegist = new ProtoMsg.MsgCl2gsRegist();
            MsgRegServerInfo info = new MsgRegServerInfo();
            MsgNetAddress andress = new MsgNetAddress();
            andress.u32Port = 0;
            andress.u32Ip = 0;
            
            info.uType = 1;
            info.uServerId = 0;
            info.netAddr = andress;
            
            requestRegist.Info = info;
            Communicate.Send2GS(eMsgTypes.MsgCl2gsRegist, protobufM.Serializerobject<ProtoMsg.MsgCl2gsRegist>(requestRegist));
            
            ProtoMsg.MsgCl2gsLogin requestLogin = new ProtoMsg.MsgCl2gsLogin();
            requestLogin.u64UUID = g_LSLogin.u64UUID;
            requestLogin.strLoginSession = g_LSLogin.strLoginSession;
            requestLogin.strAccount = g_LSLogin.u64UUID.ToString();
            Communicate.Send2GS(eMsgTypes.MsgCl2gsLogin, protobufM.Serializerobject<ProtoMsg.MsgCl2gsLogin>(requestLogin));
        }
        
        /// <summary>
        ///   登入GS 回调
        /// </summary>
        // 保存登录gs 服务器消息
        public static void RecvMsgGS2CLLoginReply(eMsgTypes Type, object Info)
        {
            if (Info == null) {
                return;
            }
            g_GSLogin = Info as ProtoMsg.MsgCl2gsLoginReply;
            IGGDebug.Log("连接游戏服务器成功");
        }
    }
}