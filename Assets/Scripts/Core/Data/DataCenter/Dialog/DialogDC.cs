using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;



namespace IGG.Core.Data.DataCenter.Dialog
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.2.20
    /// Desc    Dialog模块动态数据集
    /// </summary>
    public static class DialogDC
    {
        //下面定义Dialog模块需要用到的所有动态数据字段
        // 自动剧情，客户端每次启动默认关闭
        private static bool g_AutoDialog = false;
        
        public static void ClearDC()
        {
            //退到登陆界面时执行
        }
        
        /// <summary>
        /// 自动剧情设置
        /// </summary>
        public static bool AutoDialog {
            get { return g_AutoDialog; }
            set { g_AutoDialog = value; }
        }
        /// <summary>
        /// 模拟数据
        /// </summary>
        public static void SimulationData()
        {
        }
    }
}