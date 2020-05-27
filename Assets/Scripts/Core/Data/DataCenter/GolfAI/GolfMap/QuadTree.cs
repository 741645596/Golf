#region Namespace
using System.Collections.Generic;
using System;
using UnityEngine;
#endregion

/// <summary>
/// Author zhulin
/// Date 2019.05.30
/// Desc 平衡四叉树,包含三角形的数量
/// </summary>

namespace IGG.Core.Data.DataCenter.GolfAI
{
    public class QuadTree : IAB
    {
        protected const int MaxCount = 100;
        /// <summary>
        /// 引用的地图
        /// </summary>
        ///private GolfCourseMap m_Map = null;
        /// <summary>
        /// 自身饱和的三角形
        /// </summary>
        protected List<Index2> m_SelfList = null;
        /// <summary>
        /// 4棵子树
        /// </summary>
        private QuadTree m_UpLeftTree = null;
        private QuadTree m_UpRightTree = null;
        private QuadTree m_DownLeftTree = null;
        private QuadTree m_DownRightTree = null;
        /// <summary>
        /// 创建四叉树
        /// </summary>
        /// 
        public QuadTree() { }
        public QuadTree(Vector3 aa, Vector3 bb)
        {
            this.m_AA3 = aa;
            this.m_BB3 = bb;
            SetAABB2();
        }
        public QuadTree(Vector2 aa, Vector2 bb)
        {
            this.m_AA2 = aa;
            this.m_BB2 = bb;
            SetAABB3();
        }
        /// <summary>
        /// 构建4叉树
        /// </summary>
        public void Create(List<Index2> listAB, GolfCourseMap map)
        {
            if (listAB == null || listAB.Count == 0)
                return;
            if (listAB.Count <= MaxCount)
            {
                m_SelfList = new List<Index2>();
                m_SelfList.AddRange(listAB);
            }
            else
            {
                Vector2 cc = (this.AA2 + this.BB2) * 0.5f;
                m_UpLeftTree = new QuadTree(this.AA2, cc);
                m_UpRightTree = new QuadTree(new Vector2(cc.x, this.AA2.y), new Vector2(this.BB2.x, cc.y));
                m_DownLeftTree = new QuadTree(new Vector2(this.AA2.x, cc.y), new Vector2(cc.x, this.BB2.y));
                m_DownRightTree = new QuadTree(cc, this.BB2);
                List<Index2> lUpLeft = new List<Index2>();
                List<Index2> lUpRight = new List<Index2>();
                List<Index2> lDownLeft = new List<Index2>();
                List<Index2> lDownRight = new List<Index2>();
                for (int i = 0; i < listAB.Count; i++)
                {
                    GolfMaptriangle tri = map.GetMapTriangle(listAB[i]);
                    if (tri == null)
                        continue;
                    if (m_UpLeftTree.CheckProjectionContains(tri) == true)
                    {
                        lUpLeft.Add(listAB[i]);
                    }
                    if (m_UpRightTree.CheckProjectionContains(tri) == true)
                    {
                        lUpRight.Add(listAB[i]);
                    }
                    if (m_DownLeftTree.CheckProjectionContains(tri) == true)
                    {
                        lDownLeft.Add(listAB[i]);
                    }
                    if (m_DownRightTree.CheckProjectionContains(tri) == true)
                    {
                        lDownRight.Add(listAB[i]);
                    }
                }
                CreateSubTree(ref m_UpLeftTree, ref lUpLeft, map);
                CreateSubTree(ref m_UpRightTree, ref lUpRight, map);
                CreateSubTree(ref m_DownLeftTree, ref lDownLeft, map);
                CreateSubTree(ref m_DownRightTree, ref lDownRight, map);
            }
        }

        /// <summary>
        /// 构建4叉树
        /// </summary>
        public void CreateSubTree(ref QuadTree tree, ref List<Index2> listAB, GolfCourseMap map)
        {
            if (tree == null)
                return;
            if (listAB.Count > 0)
            {
                tree.Create(listAB, map);
            }
            else
            {
                tree = null;
            }
            listAB.Clear();
            listAB = null;
        }


        public void Clear()
        {
            if (m_SelfList != null)
            {
                m_SelfList.Clear();
                m_SelfList = null;
            }
            if (m_UpLeftTree != null)
            {
                m_UpLeftTree.Clear();
                m_UpLeftTree = null;
            }
            if (m_UpRightTree != null)
            {
                m_UpRightTree.Clear();
                m_UpRightTree = null;
            }
            if (m_DownLeftTree != null)
            {
                m_DownLeftTree.Clear();
                m_DownLeftTree = null;
            }
            if (m_DownRightTree != null)
            {
                m_DownRightTree.Clear();
                m_DownRightTree = null;
            }
        }
        /// <summary>
        /// 获取包含该点的最小子树
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected QuadTree GetTree(Vector2 s)
        {
            QuadTree tree = null;
            if (this.CheckProjectionInArea(s) == false)
                return null;
            if (m_UpLeftTree != null)
            {
                tree = m_UpLeftTree.GetTree(s);
                if (tree != null)
                    return tree;
            }
            else if (m_UpRightTree != null)
            {
                tree = m_UpRightTree.GetTree(s);
                if (tree != null)
                    return tree;
            }
            else if (m_DownLeftTree != null)
            {
                tree = m_DownLeftTree.GetTree(s);
                if (tree != null)
                    return tree;
            }
            else if (m_DownRightTree != null)
            {
                tree = m_DownRightTree.GetTree(s);
                if (tree != null)
                    return tree;
            }

            return this;
        }
        /// <summary>
        /// 获取包含该点的最小子树
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected QuadTree GetTree(Vector3 s)
        {
            return GetTree(new Vector2(s.x, s.z));
        }

        /// <summary>
        /// 获取包含该线段的最小子树
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected QuadTree GetTree(Vector2 s, Vector2 e)
        {
            QuadTree tree = null;
            if (this.CheckProjectionInArea(s) == false || this.CheckProjectionInArea(e) == false)
                return null;
            if (m_UpLeftTree != null)
            {
                tree = m_UpLeftTree.GetTree(s, e);
                if (tree != null)
                    return tree;
            }
            else if (m_UpRightTree != null)
            {
                tree = m_UpRightTree.GetTree(s, e);
                if (tree != null)
                    return tree;
            }
            else if (m_DownLeftTree != null)
            {
                tree = m_DownLeftTree.GetTree(s, e);
                if (tree != null)
                    return tree;
            }
            else if (m_DownRightTree != null)
            {
                tree = m_DownRightTree.GetTree(s, e);
                if (tree != null)
                    return tree;
            }

            return this;
        }


        /// <summary>
        /// 获取包含该点的最小子树
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public QuadTree GetTree(Vector3 s, Vector3 e)
        {
            return GetTree(new Vector2(s.x, s.z), new Vector2(e.x, e.z));
        }


        /// <summary>
        /// 获取树的所有三角形
        /// </summary>
        /// <returns></returns>
        protected List<Index2> GetAllTri()
        {
            List<Index2> listRet = new List<Index2>();
            if (m_SelfList != null)
            {
                listRet.AddRange(m_SelfList);
            }
            else
            {
                List<Index2> listTemp = null;
                if (m_UpLeftTree != null)
                {
                    listTemp = m_UpLeftTree.GetAllTri();
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        CombineList(listRet, listTemp);
                        listTemp.Clear();
                        listTemp = null;
                    }
                }
                if (m_UpRightTree != null)
                {
                    listTemp = m_UpRightTree.GetAllTri();
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        CombineList(listRet, listTemp);
                        listTemp.Clear();
                        listTemp = null;
                    }
                }
                if (m_DownLeftTree != null)
                {
                    listTemp = m_DownLeftTree.GetAllTri();
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        CombineList(listRet, listTemp);
                        listTemp.Clear();
                        listTemp = null;
                    }
                }
                if (m_DownRightTree != null)
                {
                    listTemp = m_DownRightTree.GetAllTri();
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        CombineList(listRet, listTemp);
                        listTemp.Clear();
                        listTemp = null;
                    }
                }
            }
            return listRet;
        }

        /// <summary>
        /// 合并列表
        /// </summary>
        protected void CombineList(List<Index2> listRet, List<Index2> list2)
        {
            if (listRet == null || list2 == null)
                return;
            if (listRet.Count == 0)
            {
                listRet.AddRange(list2);
            }
            foreach (Index2 v in list2)
            {
                if (listRet.Contains(v) == false)
                {
                    listRet.Add(v);
                }
            }
        }

        /// <summary>
        /// 获取点在树的所有三角形。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected List<Index2> GetAllTri(Vector2 s)
        {
            QuadTree tree = GetTree(s);
            if (tree == null)
            {
                return null;
            }
            else
            {
                return tree.GetAllTri();
            }
        }

        /// <summary>
        /// 获取点在树的所有三角形。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<Index2> GetAllTri(Vector3 s)
        {
            return GetAllTri(new Vector2(s.x, s.z));
        }


        /// <summary>
        /// 获取线段所在树的三角形，前提时这个树已经是最小的树了。调用的时候需先得到最小的树。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected List<Index2> GetAllTri(Vector2 s, Vector2 e)
        {
            if (this.CheckProjectionContains(s, e) == false)
            {
                return null;
            }
            List<Index2> listRet = new List<Index2>();
            if (m_SelfList != null)
            {
                listRet.AddRange(m_SelfList);

            }
            else
            {
                List<Index2> listTemp = null;
                if (m_UpLeftTree != null)
                {
                    listTemp = m_UpLeftTree.GetAllTri(s, e);
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        CombineList(listRet, listTemp);
                        listTemp.Clear();
                        listTemp = null;
                    }
                }
                if (m_UpRightTree != null)
                {
                    listTemp = m_UpRightTree.GetAllTri(s, e);
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        CombineList(listRet, listTemp);
                        listTemp.Clear();
                        listTemp = null;
                    }
                }
                if (m_DownLeftTree != null)
                {
                    listTemp = m_DownLeftTree.GetAllTri(s, e);
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        CombineList(listRet, listTemp);
                        listTemp.Clear();
                        listTemp = null;
                    }
                }
                if (m_DownRightTree != null)
                {
                    listTemp = m_DownRightTree.GetAllTri(s, e);
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        CombineList(listRet, listTemp);
                        listTemp.Clear();
                        listTemp = null;
                    }
                }
            }
            return listRet;
        }

        /// <summary>
        /// 获取线段所在树的三角形，前提时这个树已经是最小的树了。调用的时候需先得到最小的树。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<Index2> GetAllTri(Vector3 s, Vector3 e)
        {
            return GetAllTri(new Vector2(s.x, s.z), new Vector2(e.x, e.z));
        }

    }


    [Serializable]
    public struct Index2
    {
        public uint MapID;
        public uint TriID;
        public Index2(uint mapID, uint TriID)
        {
            this.MapID = mapID;
            this.TriID = TriID;
        }

        public static bool operator ==(Index2 a, Index2 b)
        {
            return a.MapID == b.MapID && a.TriID == b.TriID;
        }

        public static bool operator !=(Index2 a, Index2 b)
        {
            return a.MapID != b.MapID || a.TriID != b.TriID;
        }

        /// <summary>
        ///   <para>Returns true if the given Index2 is exactly equal to this Index2.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            bool result;
            if (!(other is Index2))
            {
                result = false;
            }
            else
            {
                Index2 rhs = (Index2)other;
                result = MapID.Equals(rhs.MapID) && TriID.Equals(rhs.TriID);
            }

            return result;
        }

        public override int GetHashCode()
        {
            return (int)MapID * 49157 + (int)TriID * 98317;
        }
    }
}
