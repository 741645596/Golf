using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author  zhulin
/// Date    2019.5.6
/// Desc    GolfAI模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI
{
    public class GolfAIPoint
    {
        public GolfAIPoint() { }
        public GolfAIPoint(Vector3 pos)
        {
            this.m_Pos = pos;
        }
        private Vector3 m_Pos;
        public Vector3 Pos
        {
            get { return m_Pos; }
        }



        public void Clear()
        {
        }
    }
}

