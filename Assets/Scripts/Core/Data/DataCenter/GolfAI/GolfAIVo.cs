using IGG.Core.Data.Config;

    /// <summary>
    /// Author  zhulin
    /// Date    2019.5.6
    /// Desc    GolfAI模块动态数据集
    /// </summary>
namespace IGG.Core.Data.DataCenter.GolfAI {
    // 高尔夫球场地图数据结构
    public class GolfLinksMap
    {
        public uint ChapterID = 0;
        public uint BattleID = 0;
        public bool BattleResult = false;
    }

    // 高尔夫运动路线
    public class GolfBallRunRoad
    {
        public uint ChapterID = 0;
        public uint BattleID = 0;
        public bool BattleResult = false;
    }

}
