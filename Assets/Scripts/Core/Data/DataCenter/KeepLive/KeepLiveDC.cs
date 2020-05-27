using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;
using ProtoMsg;

namespace IGG.Core.Data.DataCenter.KeepLive
{
    /// <summary>
    /// Author  cgj
    /// Date    2019.2.13
    /// Desc    KeepLive模块动态数据集
    /// </summary>
    public static class KeepLiveDC
    {
        //下面定义KeepLive模块需要用到的所有动态数据字段
        
        public static void ClearDC()
        {
            //退到登陆界面时执行
        }
        
        
        /// <summary>
        /// 模拟数据
        /// </summary>
        public static void SimulationData()
        {
        }
        
        
        /// <summary>
        ///   心跳包
        /// </summary>
        public static void SendKeepLiveMsg()
        {
            ProtoMsg.MsgClientKeepLiveAck requestKeepLive = new ProtoMsg.MsgClientKeepLiveAck();
            
            Communicate.Send2GS(eMsgTypes.MsgClientKeepLiveAck, protobufM.Serializerobject<ProtoMsg.MsgClientKeepLiveAck>(requestKeepLive));
            
        }
        
        public static void RecvMsgClientKeepLive(eMsgTypes Type, object Info)
        {
            if (Info == null) {
                return;
            }
            MsgClientKeepLive g_KeepLive = Info as ProtoMsg.MsgClientKeepLive;
            
            SendKeepLiveMsg();
        }
    }
}