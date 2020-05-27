using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AABB辅助计算
/// </summary>
namespace IGG.Core.Utils {

    public static class AABB {
        /// <summary>
        /// 计算三角形的AABB
        /// </summary>
        /// <param name="a">顶点a</param>
        /// <param name="b">顶点b</param>
        /// <param name="c">顶点c</param>
        /// <param name="AA"></param>
        /// <param name="BB"></param>
        public static void CalcAABB(Vector3 a, Vector3 b, Vector3 c, ref Vector3 AA,ref Vector3 BB)
        {
            AA = new Vector3(GetMin(a.x,b.x, c.x), GetMin(a.y, b.y, c.y), GetMin(a.z, b.z, c.z));
            BB = new Vector3(GetMax(a.x, b.x, c.x), GetMax(a.y, b.y, c.y), GetMax(a.z, b.z, c.z));
        }
        /// <summary>
        /// 获取三个数的最小值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float GetMax(float a, float b, float c)
        {
            float max = a >= b ? a:b;
            max = max >= c ? max : c;
            return max;
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float GetMax(List<float> listValue)
        {
            if (listValue == null || listValue.Count == 0)
            {
                return 0.0f;
            }
            float max = listValue[0];
            for (int i = 1; i < listValue.Count; i++)
            {
                max = listValue[i] > max ? listValue[i] : max;
            }
            return max;
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float GetMin(List<float> listValue)
        {
            if (listValue == null || listValue.Count == 0)
            {
                return 0.0f;
            }
            float min = listValue[0];
            for (int i = 1; i < listValue.Count; i++)
            {
                min = listValue[i] < min ? listValue[i] : min;
            }
            return min;
        }
        /// <summary>
        /// 获取三个数的最小值.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float GetMin(float a, float b, float c)
        {
            float min = a <= b ? a : b;
            min = min <= c ? min : c;
            return min;
        }

        /// <summary>
        /// 获取AABB 中的AA
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public static Vector3 GetAA(List<Vector3> lp)
        {
            Vector3 v = Vector3.zero;
            if (lp == null || lp.Count == 0)
                return v;
            v = lp[0];
            for (int i = 1; i < lp.Count; i++)
            {
                v.x = v.x <= lp[i].x ? v.x : lp[i].x;
                v.y = v.y <= lp[i].y ? v.y : lp[i].y;
                v.z = v.z <= lp[i].z ? v.z : lp[i].z;
            }
            return v;
        }

        /// <summary>
        /// 获取AABB 中的AA
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public static Vector2 GetAA(List<Vector2> lp)
        {
            Vector2 v = Vector2.zero;
            if (lp == null || lp.Count == 0)
                return v;
            v = lp[0];
            for (int i = 1; i < lp.Count; i++)
            {
                v.x = v.x <= lp[i].x ? v.x : lp[i].x;
                v.y = v.y <= lp[i].y ? v.y : lp[i].y;
            }
            return v;
        }

        /// <summary>
        /// 获取AABB 中的AA
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public static Vector2 GetAA2(List<Vector3> lp)
        {
            Vector3 v = GetAA(lp);
            return new Vector2(v.x, v.z);
        }


        /// <summary>
        /// 获取aabb 中的BB
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public static Vector3 GetBB(List<Vector3> lp)
        {
            Vector3 v = Vector3.zero;
            if (lp == null || lp.Count == 0)
                return v;
            v = lp[0];
            for (int i = 1; i < lp.Count; i++)
            {
                v.x = v.x >= lp[i].x ? v.x : lp[i].x;
                v.y = v.y >= lp[i].y ? v.y : lp[i].y;
                v.z = v.z >= lp[i].z ? v.z : lp[i].z;
            }
            return v;
        }

        /// <summary>
        /// 获取aabb 中的BB
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public static Vector2 GetBB(List<Vector2> lp)
        {
            Vector2 v = Vector2.zero;
            if (lp == null || lp.Count == 0)
                return v;
            v = lp[0];
            for (int i = 1; i < lp.Count; i++)
            {
                v.x = v.x >= lp[i].x ? v.x : lp[i].x;
                v.y = v.y >= lp[i].y ? v.y : lp[i].y;
            }
            return v;
        }

        /// <summary>
        /// 获取AABB 中的BB
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public static Vector2 GetBB2(List<Vector3> lp)
        {
            Vector3 v = GetBB(lp);
            return new Vector2(v.x, v.z);
        }

        /// <summary>
        /// p,q 两个AABB 是否相交
        /// </summary>
        /// <param name="pAA">p的AA</param>
        /// <param name="pBB">p的BB</param>
        /// <param name="qAA">q的AA</param>
        /// <param name="qBB">q的BB</param>
        /// <returns></returns>
        public static bool CheckContains(Vector3 pAA,  Vector3 pBB, Vector3 qAA, Vector3 qBB)
        {
            Vector3 pCenter = (pAA + pBB) * 0.5f;
            Vector3 qCenter = (qAA + qBB) * 0.5f;
            Vector3 vd = qCenter - pCenter;
            Vector3 pdis = pBB - pAA;
            Vector3 qdis = qBB - qAA;
            // aabb盒子有相交的条件是,中心的距离在,x,y,z的投影小于等于他们边长之和的一半.
            if (Mathf.Abs(vd.x) * 2 <= Mathf.Abs(pdis.x) + Mathf.Abs(qdis.x) &&
                Mathf.Abs(vd.y) * 2 <= Mathf.Abs(pdis.y) + Mathf.Abs(qdis.y) &&
                Mathf.Abs(vd.z) * 2 <= Mathf.Abs(pdis.z) + Mathf.Abs(qdis.z))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// p,q 两个AABB 是否相交
        /// </summary>
        /// <param name="pAA">p的AA</param>
        /// <param name="pBB">p的BB</param>
        /// <param name="qAA">q的AA</param>
        /// <param name="qBB">q的BB</param>
        /// <returns></returns>
        public static bool CheckContains(Vector2 pAA, Vector2 pBB, Vector2 qAA, Vector2 qBB)
        {
            Vector2 pCenter = (pAA + pBB) * 0.5f;
            Vector2 qCenter = (qAA + qBB) * 0.5f;
            Vector2 vd = qCenter - pCenter;
            Vector2 pdis = pBB - pAA;
            Vector2 qdis = qBB - qAA;
            // aabb盒子有相交的条件是,中心的距离在,x,y,z的投影小于等于他们边长之和的一半.
            if (Mathf.Abs(vd.x) * 2 <= Mathf.Abs(pdis.x) + Mathf.Abs(qdis.x) &&
                Mathf.Abs(vd.y) * 2 <= Mathf.Abs(pdis.y) + Mathf.Abs(qdis.y))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// p,两个AABB 中
        /// </summary>
        /// <param name="pAA">p的AA</param>
        /// <param name="pBB">p的BB</pa怕ram>
        /// <param name="qAA">q的AA</param>
        /// <param name="qBB">q的BB</param>
        /// <returns></returns>
        public static bool CheckContains(Vector2 pAA, Vector2 pBB, Vector2 p)
        {
            Vector2 pCenter = (pAA + pBB) * 0.5f;
            Vector2 vd = pCenter - p;
            Vector2 pdis = (pBB - pAA) * 0.5f;
            if (Mathf.Abs(vd.x)  <= Mathf.Abs(pdis.x) &&
                Mathf.Abs(vd.y)  <= Mathf.Abs(pdis.y) )
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// p,两个AABB 中
        /// </summary>
        /// <param name="pAA">p的AA</param>
        /// <param name="pBB">p的BB</pa怕ram>
        /// <param name="p">测试用的点</param>
        /// <returns></returns>
        public static bool CheckContains(Vector3 pAA, Vector3 pBB, Vector3 p)
        {
            Vector3 pCenter = (pAA + pBB) * 0.5f;
            Vector3 vd = pCenter - p;
            Vector3 pdis = (pBB - pAA) * 0.5f;
            if (Mathf.Abs(vd.x) <= Mathf.Abs(pdis.x) &&
                Mathf.Abs(vd.y) <= Mathf.Abs(pdis.y) &&
                Mathf.Abs(vd.z) <= Mathf.Abs(pdis.z))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断点P在三角形内部
        /// </summary>
        /// <param name="A">三角形A顶点</param>
        /// <param name="B">三角形B顶点</param>
        /// <param name="C">三角形C顶点</param>
        /// <param name="P">三角形平面点</param>
        /// <returns></returns>
        public static bool CheckPointinTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            return SameSide(A, B, C, P) &&
                SameSide(B, C, A, P) &&
                SameSide(C, A, B, P);
        }
        /// <summary>
        /// 判断C，P在AB的同一侧。
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        private static bool SameSide(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            Vector3 AB = B - A;
            Vector3 AC = C - A;
            Vector3 AP = P - A;

            Vector3 v1 = Vector3.Cross(AB, AC);
            Vector3 v2 = Vector3.Cross(AB, AP);
            // v1 and v2 should point to the same direction
            return Vector3.Dot(v1, v2) >= -LogicConstantData.FLOAT_EP;
            //return Vector3.Dot(v1, v2) >= 0;
        }


        /// <summary>
        /// 判断C，P在AB的异侧。
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        private static bool NoSameSide(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            Vector3 AB = B - A;
            Vector3 AC = C - A;
            Vector3 AP = P - A;

            Vector3 v1 = Vector3.Cross(AB, AC);
            Vector3 v2 = Vector3.Cross(AB, AP);

            // v1 and v2 should point to the same direction
            return Vector3.Dot(v1.normalized, v2.normalized) <= LogicConstantData.FLOAT_EP;
        }

        /// <summary>
        /// 判断点P在xz的投影是否在在三角形投影在xz平面的三角形内部
        /// </summary>
        /// <param name="A">三角形A顶点</param>
        /// <param name="B">三角形B顶点</param>
        /// <param name="C">三角形C顶点</param>
        /// <param name="P">三角形平面点</param>
        /// <returns></returns>
        public static bool CheckProjectionPointinTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            Vector3 A1 = new Vector3(A.x, 0, A.z);
            Vector3 B1 = new Vector3(B.x, 0, B.z);
            Vector3 C1 = new Vector3(C.x, 0, C.z);
            Vector3 P1 = new Vector3(P.x, 0, P.z);
            return CheckPointinTriangle(A1, B1, C1, P1);
        }


        /// <summary>
        /// 求平面线段的交点
        /// </summary>
        /// <param name="normal">平面法向量</param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns>true 有交点，false 无交点</returns>
        public static bool CheckIntersection(Vector3 normal, Vector3 a, Vector3 b, Vector3 c, Vector3 d, ref Vector3 ret)
        {
            if (JudgeIntersect(a, b, c, d) == true)
            {   //保证交点在ab上。
                return CalcIntersection(normal, c, d, a, b, ref ret);
            }

            return false;
        }


        /// <summary>
        /// 判断线段是否相交
        /// </summary>
        private static bool JudgeIntersect(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            bool isab = NoSameSide(a, b, c, d);
            bool iscd = NoSameSide(c, d, a , b);

            if (isab == true && iscd == true)
            {
                return true;
            }
            else return false;
        }



        /// <summary>
        /// 判断向量是否平行
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
            public static bool IsParallel(Vector3 lhs, Vector3 rhs)
        {
            float value = Vector3.Dot(lhs.normalized, rhs.normalized);
            if (Mathf.Abs(value) == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 判断向量是否平行
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool IsParallel(Vector2 lhs, Vector2 rhs)
        {
            float value = Vector2.Dot(lhs.normalized, rhs.normalized);
            if (Mathf.Abs(value) == 1)
                return true;
            return false;
        }
        /// <summary>
        /// 已确认一定会相交了。ab, cd不会平行。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static bool CalcIntersection(Vector3 normal,Vector3 a, Vector3 b, Vector3 c, Vector3 d, ref Vector3 ret)
        {
            float t = 0.0f;
            Vector3 cd = d - c;
            if (Vector3.Dot(normal, new Vector3(0, 1, 0)) != 0)
            {
                // 投影到xz平面进行计算。
                t = CalcIntersection(new Vector2(a.x, a.z), new Vector2(b.x, b.z), new Vector2(c.x, c.z), new Vector2(d.x, d.z));
            }
            else if (Vector3.Dot(normal, new Vector3(1, 0, 0)) != 0)
            {
                // 投影到yz平面进行计算。
                t = CalcIntersection(new Vector2(a.y, a.z), new Vector2(b.y, b.z), new Vector2(c.y, c.z), new Vector2(d.y, d.z));
            }
            else if (Vector3.Dot(normal, new Vector3(0, 0, 1)) != 0)
            {
                // 投影到xy平面进行计算。
                t = CalcIntersection(new Vector2(a.x, a.y), new Vector2(b.x, b.y), new Vector2(c.x, c.y), new Vector2(d.x, d.y));
            }
            //Debug.Log("t:" + t);
            if (t >= 0 && t <= 1)
            {
                ret = c + cd * t;
                return true;
            }
            else
            {
                return false;
            }
            

            //return c + cd * t;
        }
        /// <summary>
        /// 已确认一定会相交了。ab, cd不会平行。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static float CalcIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            Vector2 ab = b - a;
            Vector2 cd = d - c;
            Vector2 ac = c - a;
            float t1 = Cross(ac, ab);
            float t2 = Cross(ab, cd);
            if (Mathf.Abs(t2) < LogicConstantData.FLOAT_EP)
            {
                return -1;
            }
            return t1 / t2;
        }


        /// <summary>
        /// 向量内积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }


        /// <summary>
        /// 向量内积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static float Cross(Vector2 a, Vector2 b, Vector2 c)
        {
            return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
        }
    }
}
