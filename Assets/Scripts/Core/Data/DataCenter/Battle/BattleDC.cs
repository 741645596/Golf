using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;
using System;
using IGG.Core.Data.DataCenter.GolfAI;

namespace IGG.Core.Data.DataCenter.Battle
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.5.27
    /// Desc    Battle模块动态数据集
    /// </summary>
    public static class BattleDC
    {
        //下面定义Battle模块需要用到的所有动态数据字段
        //下面定义GolfAI模块需要用到的所有动态数据字段
        private static GolfCourseMap g_BattleMap = null;
        public static GolfCourseMap Map
        {
            get { return g_BattleMap; }
        }

        /// <summary>
        /// 球杆
        /// </summary>
        private static ClubInfo g_SelClub = null;
        public static ClubInfo SelClub
        {
            get { return g_SelClub; }
        }

        /// <summary>
        /// 球
        /// </summary>
        private static BallInfo g_SelBall = null;
        public static BallInfo SelBall
        {
            get { return g_SelBall; }
        }



        public static void ClearDC()
        {
            //退到登陆界面时执行
            if (g_BattleMap != null)
            {
                g_BattleMap.Clear();
                g_BattleMap = null;
            }

            if (g_SelClub != null)
            {
                g_SelClub = null;
            }

            if (g_SelBall != null)
            {
                g_SelBall = null;
            }
        }
		
		
		/// <summary>
        /// 模拟数据
        /// </summary>
        public static void SimulationData()
        {
            MapData data = MapData.Load();
            g_BattleMap = new GolfCourseMap(data);
            g_BattleMap.BuildMap();


            g_SelClub = new ClubInfo();
            g_SelClub.ID = 10001;
            g_SelClub.Type = 5;
            g_SelClub.Strength = 190;
            g_SelClub.Accuracy = 10;
            g_SelClub.ForwardSpin = 0;
            g_SelClub.BackSpin = 0;
            g_SelClub.Hook = 5;

            g_SelBall = new BallInfo();
            g_SelBall.ID = 10001;
            g_SelBall.StrengthPercent = 0;
            g_SelBall.windRevise = 0;
            g_SelBall.LRSpin = 0;

            //GolfMapFlyCollision collision = null;
            //Vector3 s = new Vector3(250.0f, 0.0f, 25.0f);
            //Vector3 e = new Vector3(250.0f, -0.01125f, 25.0f);
            //g_BattleMap.CheckCollisionPoint(s, e, ref collision);
        }
    }
}