using System.Collections;

public class LogicConstantData
{
    //浮点误差精度修正
    public static float FLOAT_PERCISION_FIX = 0.1f;
    public static float FLOAT_EP = 0.000001f;
    public static float FLOAT_DIS = 0.0005f;

    public static string g_version = "0.1.0";
    public static int g_ValueUnit = 1000000;
    public static  int SecondFrame = 30;

    /// <summary>
    /// 小球半径
    /// </summary>
    public static float BallRadius = 0.0215f;
    /// <summary>
    /// 重力，带方向
    /// </summary>
    public static float Gravity = -1f;
    /// <summary>
    /// 风力换算 1MPH = 0.447 m/s
    /// </summary>
    public static float WindConvert = 0.447f;
    /// <summary>
    /// 最大飞行曲线数
    /// </summary>
    public static int MaxFlyCurveCount = 4;
    /// <summary>
    /// 最大飞行路径点的数量
    /// </summary>
    public static int MaxFlyPointCount = 300;
    /// <summary>
    /// 最大滚动路径点的数量
    /// </summary>
    public static int MaxRollPointCount = 300;
}
