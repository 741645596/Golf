using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.21
    /// Desc    player_level配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class PlayerLevelConfig : IConfig<uint>
    {
        /// <summary>
        /// 配置等级，作为配置表的唯一key
        /// </summary>
        public uint Lv;

        /// <summary>
        /// 配置升级到该等级所需的经验值
        /// </summary>
        public uint Experience;

        /// <summary>
        /// 配置等级对应的队伍cost上限
        /// </summary>
        public uint MaxCost;

        /// <summary>
        /// 配置等级对应的体力上限值
        /// </summary>
        public uint MaxPower;

        /// <summary>
        /// 配置等级对应的铁矿上限值
        /// </summary>
        public uint MaxIron;

        /// <summary>
        /// 配置等级对应的食物上限值
        /// </summary>
        public uint MaxFood;


        public uint GetKey()
        {
            return Lv;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.21
    /// Desc    player_level配置文件访问接口
    /// </summary>
    public partial class PlayerLevelDao:BaseDao<PlayerLevelDao,uint,PlayerLevelConfig>
    {
        public override string GetName()
        {
            return "player_level";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.21
    /// Desc    player_level配置文件解码器
    /// </summary>
    public partial class PlayerLevelDecoder : BaseCfgDecoder<PlayerLevelConfig, PlayerLevelCfgData>
    {
        public override string GetName()
        {
            return "player_level";
        }

        protected override void ProcessRow(PlayerLevelConfig excel)
        {
            GetU32("#lv", out excel.Lv);
            GetU32("experience", out excel.Experience);
            GetU32("max_cost", out excel.MaxCost);
            GetU32("max_power", out excel.MaxPower);
            GetU32("max_iron", out excel.MaxIron);
            GetU32("max_food", out excel.MaxFood);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}