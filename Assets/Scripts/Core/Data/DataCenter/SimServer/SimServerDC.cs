using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;
using IGG.Data.DataStorage;
using IGG.Core.Data.Config;

namespace IGG.Core.Data.DataCenter.SimServer
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.2.14
    /// Desc    SimServer模块动态数据集
    /// </summary>
    public static class SimServerDC
    {
        private static int g_prevLev = 1;
        private static int g_lev = 1;
        private static int g_exp = 0;
        private static int g_upExp = 0;
        private static int g_constaddExp = 120;  // 赢一场120
        
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
        /// 增加的经验
        /// </summary>
        /// <returns></returns>
        public static int GetAddExp()
        {
            return g_upExp;
        }
        
        /// <summary>
        /// 获取战斗之前的经验
        /// </summary>
        /// <returns></returns>
        public static int GetPrevExp()
        {
            return g_exp - g_upExp;
        }
        /// <summary>
        /// 总经验
        /// </summary>
        /// <returns></returns>
        public static int GetTotalExp()
        {
            return g_exp;
        }
        
        
        /// <summary>
        ///  获取玩家战斗之前等级
        /// </summary>
        /// <returns></returns>
        public static int GetPrevLev()
        {
            return g_prevLev;
        }
        /// <summary>
        ///  获取玩家当前等级
        /// </summary>
        /// <returns></returns>
        public static int GetLev()
        {
            return g_lev;
        }
    }
}