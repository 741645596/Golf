using UnityEngine;
using IGG.Core.Utils;
using System.Collections.Generic;
using IGG.Core.Geom;

namespace IGG.Core.Data.DataCenter.GolfAI
{
    public class IAB
    {
        /// <summary>
        /// xz投影面的AABB
        /// </summary>
        protected Vector2 m_AA2;
        public Vector2 AA2
        {
            get { return m_AA2; }
        }
        protected Vector2 m_BB2;
        public Vector2 BB2
        {
            get { return m_BB2; }
        }
        /// <summary>
        /// 空间中的AABB
        /// </summary>
        protected Vector3 m_AA3;
        public Vector3 AA3
        {
            get { return m_AA3; }
        }
        protected Vector3 m_BB3;
        public Vector3 BB3
        {
            get { return m_BB3; }
        }



        /// <summary>
        /// 计算AABB，
        /// </summary>
        /// <param name="list"></param>
        protected void CalcAABB<T>(List<T> listAB) where T:IAB
        {
            if (listAB == null || listAB.Count == 0)
                return;
            List<Vector3> lAA = new List<Vector3>();
            List<Vector3> lBB = new List<Vector3>();
            foreach (IAB v in listAB)
            {
                if (v != null)
                {
                    lAA.Add(v.AA3);
                    lBB.Add(v.BB3);
                }
            }
            this.m_AA3 = AABB.GetAA(lAA);
            this.m_BB3 = AABB.GetBB(lBB);
            SetAABB2();
            lAA.Clear();
            lBB.Clear();
            lAA = null;
            lBB = null;

        }

        /// <summary>
        /// 计算高度范围
        /// </summary>
        protected void ParseHightRange<T>(List<T> listAB) where T : IAB
        {
            if (listAB == null || listAB.Count == 0)
                return;
            List<float> lmin = new List<float>();
            List<float> lmax = new List<float>();
            foreach (IAB v in listAB)
            {
                if (v != null)
                {
                    lmin.Add(v.AA3.y);
                    lmax.Add(v.BB3.y);
                }
            }
            this.m_AA3.y = AABB.GetMin(lmin);
            this.m_BB3.y = AABB.GetMax(lmax);
            lmin.Clear();
            lmax.Clear();
            lmin = null;
            lmax = null;
        }
        /// <summary>
        /// 判断点在投影AABB
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool CheckProjectionInArea(Vector2 p)
        {
            return AABB.CheckContains(this.AA2, this.BB2, p);
        }


        /// <summary>
        /// 判断点投影在投影AABB
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool CheckProjectionInArea(Vector3 p)
        {
            return AABB.CheckContains(this.AA2, this.BB2, new Vector2(p.x, p.z));
        }

        /// <summary>
        /// 判断线段在投影AABB
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool CheckProjectionInArea(Vector2 s, Vector2 e)
        {
            bool ret1 = CheckProjectionInArea(s);
            bool ret2 = CheckProjectionInArea(e);
            return ret1 && ret2;
        }

        /// <summary>
        /// 判断线段投影在投影AABB中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool CheckProjectionInArea(Vector3 s, Vector3 e)
        {
            bool ret1 = CheckProjectionInArea(s);
            bool ret2 = CheckProjectionInArea(e);
            return ret1 && ret2;
        }

        /// <summary>
        /// 判断点在AABB中
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool CheckInArea(Vector3 p)
        {
            return AABB.CheckContains(this.AA3, this.BB3, p);
        }


        /// <summary>
        /// 判断线段在AABB中
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual bool CheckInArea(Vector3 s, Vector3 e)
        {
            bool ret1 = CheckInArea(s);
            bool ret2 = CheckInArea(e);
            return ret1 && ret2;
        }

        /// <summary>
        /// 2个ab的投影是否包含
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public virtual bool CheckProjectionContains(IAB ab)
        {
            if (ab == null)
                return false;
            return AABB.CheckContains(this.AA2, this.BB2, ab.AA2, ab.BB2);
        }

        /// <summary>
        /// 2个ab的投影是否包含
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public virtual bool CheckProjectionContains(Vector2 AA, Vector2 BB)
        {
            return AABB.CheckContains(this.AA2, this.BB2, AA, BB);
        }

        /// <summary>
        /// 2个ab的投影是否包含
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public virtual bool CheckContains(IAB ab)
        {
            if (ab == null)
                return false;
            return AABB.CheckContains(this.AA3, this.BB3, ab.AA3, ab.BB3);
        }

        /// <summary>
        /// 2个ab的投影是否包含
        /// </summary>
        /// <param name="ab"></param>
        /// <returns></returns>
        public virtual bool CheckContains(Vector3 AA, Vector3 BB)
        {
            return AABB.CheckContains(this.AA3, this.BB3, AA, BB);
        }

        /// <summary>
        /// 设置aabb2
        /// </summary>
        protected void SetAABB2()
        {
            m_AA2.x = m_AA3.x;
            m_AA2.y = m_AA3.z;
            m_BB2.x = m_BB3.x;
            m_BB2.y = m_BB3.z;
        }
        /// <summary>
        /// 设置aabb3
        /// </summary>
        protected void SetAABB3()
        {
            m_AA3.x = m_AA2.x;
            m_AA3.z = m_AA2.y;
            m_BB3.x = m_BB2.x;
            m_BB3.z = m_BB3.y;
        }
    }
}

