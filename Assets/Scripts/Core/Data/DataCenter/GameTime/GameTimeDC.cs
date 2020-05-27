using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;



namespace IGG.Core.Data.DataCenter.GameTime
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.1.30
    /// Desc    GameTime模块动态数据集
    /// </summary>
    public static class GameTimeDC
    {
        //下面定义GameTime模块需要用到的所有动态数据字段
        private static long m_LastGameTime = 0;
        private static long m_LastTime = 0; // 上一次


        public static void Update(float deltaTime)
        {

        }

        public static long GetMillisecondGameTime()
        {
            long time = System.DateTime.Now.Ticks / 10000 - m_LastTime;
            return m_LastGameTime + time;
        }

        public static long GetSecondGameTime()
        {
            return m_LastGameTime / 1000 + System.DateTime.Now.Ticks / 10000000 - m_LastTime / 1000;
        }
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


        public static void RecvMsgGS2CLServerTimeNotice(eMsgTypes Type, object Info)
        {
            if (Info == null)
            {
                return;
            }
            /*MsgGS2CLServerTimeNotice data = (MsgGS2CLServerTimeNotice)Info;
            m_LastGameTime = data.server_time * 1000;
            m_LastTime = System.DateTime.Now.Ticks / 10000;*/
        }
    }
}