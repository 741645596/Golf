using System;
using IGG.Core.Geom;
using UnityEngine;

public class MathUtil {

    public static float Clamp(float value, float max, float min) {
        if (value < min) value = min;
        if (value > max) value = max;
        return value;
    }

    public static int Clamp(int value, int max, int min) {
        if (value < min) value = min;
        if (value > max) value = max;
        return value;
    }

    public static float DistanceNoSqrt(float ax, float ay, float bx, float by) {
        float dx = ax - bx;
        float dy = ay - by;
        return dx * dx + dy * dy;
    }

    public static float Distance(float ax, float ay, float bx, float by) {
        float dx = ax - bx;
        float dy = ay - by;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    public static double DistanceTo(double ax, double ay, double bx, double by) {
        double dx = ax - bx;
        double dy = ay - by;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public static int DistanceTo(int ax, int ay, int bx, int by) {
        int dx = ax - bx;
        int dy = ay - by;
        return (int)Math.Sqrt(dx * dx + dy * dy);
    }

    public static int CalcuIndexByXY(int x, int y, int width) {
        return y * width + x;
    }

    public static int CalcuIndexByXY(Int2 pos, int width) {
        return CalcuIndexByXY(pos.x, pos.y, width);
    }
    /// <summary>
    /// 向量绕轴旋转。
    /// </summary>
    /// <param name="position"></param>
    /// <param name="center"></param>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
    {
        Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
        Vector3 resultVec3 = center + point;
        return resultVec3;
    }

    /// <summary>
    /// 计算缩放，后再旋转的向量
    /// </summary>
    /// <param name="Base">基础向量</param>
    /// <param name="Scale">缩放比例</param>
    /// <param name="angle">旋转角度,角度制</param>
    /// <returns></returns>
    public static Vector3 RotateRound(Vector3 Base, float Scale, float angle)
    {
        Vector3 p = Base * Scale;
        return RotateRound(p, Vector3.zero, Vector3.up, angle);
    }

    /// <summary>
    /// 求垂直于法线的切线方向的向量
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="normal"></param>
    /// <returns></returns>
    public static Vector3 CalcTangentVector(Vector3 vector, Vector3 normal)
    {
        float pSize = Vector3.Dot(vector, normal);
        Vector3 p = Vector3.zero;
        if (pSize != 0)
        {
            p = Vector3.Dot(-vector, normal) * normal + vector;
        }
        else
        {
            return vector;
        }
        return p;
    }

}
