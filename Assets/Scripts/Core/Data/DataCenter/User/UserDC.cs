using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;
using System;
using IGG.Core.Module;
using IGG.Core.Data.DataCenter.SimServer;

namespace IGG.Core.Data.DataCenter.User
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.1.30
    /// Desc    User模块动态数据集
    /// </summary>
    public static class UserDC
    {
        //玩家钻石
        public static ulong Gem;
        //玩家等级
        public static uint Level;
        //玩家经验
        public static uint Exp;
        //能量
        public static uint Energy;
        //铁矿
        private static ulong iron;
        //食物
        private static ulong food;
        //碎片合计 大类型
        private static ulong chip;
        public static ulong Iron {
            get => iron;
            set => iron = value;
        }
        public static ulong Food {
            get => food;
            set => food = value;
        }
        public static ulong Chip {
            get => chip;
            set => chip = value;
        }
        
        //下面定义User模块需要用到的所有动态数据字段
        
        public static void ClearDC()
        {
            //退到登陆界面时执行
            
            
            //{ 退出游戏会进入 }
            return;
        }
        static UserDC()
        {
            Level = 1;
            Exp = 60;
            Gem = 2000;
            Energy = 8;
            Iron = 500000;
            Food = 500000;
            Chip = 0;
        }
        
        public static void UpdateData(int nSender, object param) {
            Exp = (uint)SimServerDC.GetTotalExp();
            Level = (uint)SimServerDC.GetLev();
            //发送事件
            EventCenter.DispatchEvent(EventCenterType.UserUpdateDataEvent, -1, null);
            return;
        }
        
        /// <summary>
        /// 模拟数据
        /// </summary>
        public static void SimulationData()
        {
        }
    }
}