using System.Collections.Generic;
using System;

// <summary>
// 客户端事件中心。
// </summary>
// <author>zhulin</author>

namespace IGG.Core
{
    /// <summary>
    /// 服务端时间中心实现
    /// </summary>
    public partial class EventCenter
    {
        private static int g_nHookIndex = 0;
        private static Dictionary<eMsgTypes, List<int>> g_DataHook = new Dictionary<eMsgTypes, List<int>>(new msgtypeMsgTypeComparer());
        private static Dictionary<int, DataHook> g_IndexDataHook = new Dictionary<int, DataHook>();
        private static Dictionary<eMsgTypes, List<int>> g_DataHookAddTemp = new Dictionary<eMsgTypes, List<int>>(new msgtypeMsgTypeComparer());
        private static Dictionary<eMsgTypes, List<int>> g_DataHookDelTemp = new Dictionary<eMsgTypes, List<int>>(new msgtypeMsgTypeComparer());
        /// <summary>
        /// 注册服务端触发事件
        /// </summary>
        public static void RegisterHooks(eMsgTypes type, DataHook pf)
        {
            if (pf == null)
            {
                IGGDebug.Log("注册的Hook 非法");
                return;
            }

            bool bHaveDataHook = false;
            int nAddHookIndex = 0;

            Dictionary<int, DataHook>.Enumerator iter = g_IndexDataHook.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value == pf)
                {
                    bHaveDataHook = true;
                    nAddHookIndex = iter.Current.Key;
                    break;
                }
            }
            iter.Dispose();



            if (!bHaveDataHook)
            {
                g_IndexDataHook[g_nHookIndex] = pf;
                nAddHookIndex = g_nHookIndex;
                g_nHookIndex++;
            }

            if (g_DataHookAddTemp == null)
                g_DataHookAddTemp = new Dictionary<eMsgTypes, List<int>>(new msgtypeMsgTypeComparer());


            if (!g_DataHookAddTemp.ContainsKey(type))
            {
                g_DataHookAddTemp[type] = new List<int>();
            }
            List<int> listHookTemp = g_DataHookAddTemp[type];
            if (!listHookTemp.Contains(nAddHookIndex))
            {
                listHookTemp.Add(nAddHookIndex);
            }
        }

        /// <summary>
        /// 反注册服务端通知事件
        /// </summary>
        public static void AntiRegisterHooks(eMsgTypes type, DataHook pfRemove)
        {
            if (pfRemove == null)
            {
                return;
            }

            bool bHaveDataHook = false;
            int nRemovHookIndex = 0;
            Dictionary<int, DataHook>.Enumerator iter = g_IndexDataHook.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value == pfRemove)
                {
                    bHaveDataHook = true;
                    nRemovHookIndex = iter.Current.Key;
                    break;
                }
            }
            iter.Dispose();


            if (!bHaveDataHook)
                return;
            g_IndexDataHook.Remove(nRemovHookIndex);

            if (g_DataHookDelTemp == null)
                g_DataHookDelTemp = new Dictionary<eMsgTypes, List<int>>(new msgtypeMsgTypeComparer());

            if (!g_DataHookDelTemp.ContainsKey(type))
            {
                g_DataHookDelTemp[type] = new List<int>();
            }
            List<int> listHookTemp = g_DataHookDelTemp[type];
            if (listHookTemp.Contains(nRemovHookIndex) == false)
            {
                listHookTemp.Add(nRemovHookIndex);
            }
        }

    /// <summary>
    /// 派发服务端事件，也可以用于模拟服务端触发事件
    /// </summary>
    /// <param name="type">proto 枚举类型</param>
    /// <param name="obj">proto协议结构</param>
        public static void DispatchEvent(eMsgTypes type, object obj)
        {
            RegisterHooksTempToRealHooks();
            if (g_DataHook.ContainsKey(type))
            {

                List<int> listHook = g_DataHook[type];
                for (int nCnt = 0; nCnt < listHook.Count; ++nCnt)
                {
                    int nHookIndex = listHook[nCnt];
#if UNITY_EDITOR
                    if (g_IndexDataHook.ContainsKey(nHookIndex))
                        g_IndexDataHook[nHookIndex](type, obj);
#else

                    try
                    {
                        if (g_IndexDataHook.ContainsKey(nHookIndex))
					        g_IndexDataHook[nHookIndex](type, obj);
                    }
                    catch (Exception e)
                    {
						IGGDebug.Log(e.ToString());
                        //当上面s_IndexDataHook回调出现问题时，此回调就会被移除，小心成为坑
                        listHook.Remove(nHookIndex);
                        -- nCnt;
                        if (listHook.Count == 0)
                        {
                            g_DataHook.Remove(type);
                            break;
                        }
                    }
#endif
                }
            }
            AntiRegisterHooksFrDelTemp();

        }


        // 缓存回调列表移到真实回调列表
        private static void RegisterHooksTempToRealHooks()
        {
            if (g_DataHook == null)
                g_DataHook = new Dictionary<eMsgTypes, List<int>>();

            Dictionary<eMsgTypes, List<int>>.Enumerator iter = g_DataHookAddTemp.GetEnumerator();
            while (iter.MoveNext())
            {
                List<int> listHookTemp = iter.Current.Value;
                eMsgTypes key = iter.Current.Key;
                int nTempCount = listHookTemp.Count;
                for (int nCntTemp = 0; nCntTemp < nTempCount; nCntTemp++)
                {
                    int nHookIndex = listHookTemp[nCntTemp];
                    if (!g_DataHook.ContainsKey(key))
                    {
                        g_DataHook[key] = new List<int>();
                    }
                    List<int> listHook = g_DataHook[key];
                    if (listHook.Contains(nHookIndex) == false)
                    {
                        listHook.Add(nHookIndex);
                    }
                }
            }
            iter.Dispose();
            g_DataHookAddTemp.Clear();
        }








        private static void AntiRegisterHooksFrDelTemp()
        {
            Dictionary<eMsgTypes, List<int>>.Enumerator iter = g_DataHookDelTemp.GetEnumerator();
            while (iter.MoveNext())
            {
                List<int> listDelHook = iter.Current.Value;
                eMsgTypes key = iter.Current.Key;
                int nDelCount = listDelHook.Count;
                for (int nDelCnt = 0; nDelCnt < nDelCount; nDelCnt++)
                {
                    int nDelHookIndex = listDelHook[nDelCnt];
                    if (g_DataHook.ContainsKey(key))
                    {
                        List<int> listHook = g_DataHook[key];
                        if (listHook.Contains(nDelHookIndex) == true)
                        {
                            listHook.Remove(nDelHookIndex);
                        }
                        int nCount = listHook.Count;
                        if (nCount == 0)
                        {
                            g_DataHook.Remove(key);
                        }
                    }
                    else
                        g_DataHook.Remove(key);
                    if (g_DataHookAddTemp.ContainsKey(key))
                    {
                        List<int> listHookTemp = g_DataHookAddTemp[key];
                        if (listHookTemp.Contains(nDelHookIndex) == true)
                        {
                            listHookTemp.Remove(nDelHookIndex);
                        }
                        int nCount = listHookTemp.Count;
                        if (nCount == 0)
                        {
                            g_DataHookAddTemp.Remove(key);
                        }
                    }
                    else
                        g_DataHookAddTemp.Remove(key);
                }
            }
            iter.Dispose();
            g_DataHookDelTemp.Clear();

        }
    }
}


