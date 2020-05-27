using System;
using System.Collections.Generic;
using IGG.Core.Module;
using IGG.Core;



namespace IGG.Core.Data.DataCenter
{


    public class DataCenter
    {
    
        private static Dictionary<eMsgTypes, DataHook> g_DcHook = new Dictionary<eMsgTypes, DataHook>(new msgtypeMsgTypeComparer());
        
        
        public static bool ProcessData(eMsgTypes Type, byte[] data, int index, int length)
        {
            object obj = protobufM.Deserialize(Type, data, index, length);
            if (obj == null) {
                return false;
            }
            //  派发到DC中进行存储
            DispatchEvent(Type, obj);
            // 派发消息用
            EventCenter.DispatchEvent(Type, obj);
            
            return true;
        }
        
        
        
        /// <summary>
        /// 注册DC到数据中心
        /// </summary>
        public static void RegisterDCHooks(eMsgTypes type, DataHook pf)
        {
            if (pf == null) {
                IGGDebug.Log("注册的Hook 非法");
                return;
            }
            
            
            if (g_DcHook.ContainsKey(type) == false) {
                g_DcHook.Add(type, pf);
            } else {
                IGGDebug.Log("不同dc有注册到相同的存储函数");
            }
        }
        
        
        /// <summary>
        /// 反注册DC
        /// </summary>
        public static void AntiRegisterDCHooks(eMsgTypes type)
        {
            if (g_DcHook.ContainsKey(type) == false) {
                g_DcHook.Remove(type);
            }
        }
        
        
        // 回调事件
        private static void DispatchEvent(eMsgTypes type, object Info)
        {
            if (g_DcHook.ContainsKey(type) == true) {
                g_DcHook[type](type, Info);
            }
            
        }
        
    }
}
