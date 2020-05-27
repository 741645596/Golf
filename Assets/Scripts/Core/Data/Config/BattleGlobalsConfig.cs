using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.14
    /// Desc    battle_globals配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class BattleGlobalsConfig : IConfig<string>
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name;

        /// <summary>
        /// 数据
        /// </summary>
        public string Value;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks;


        public string GetKey()
        {
            return Name;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.14
    /// Desc    battle_globals配置文件访问接口
    /// </summary>
    public partial class BattleGlobalsDao:BaseDao<BattleGlobalsDao,string,BattleGlobalsConfig>
    {
        public override string GetName()
        {
            return "battle_globals";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.14
    /// Desc    battle_globals配置文件解码器
    /// </summary>
    public partial class BattleGlobalsDecoder : BaseCfgDecoder<BattleGlobalsConfig, BattleGlobalsCfgData>
    {
        public override string GetName()
        {
            return "battle_globals";
        }

        protected override void ProcessRow(BattleGlobalsConfig excel)
        {
            GetString("#name", out excel.Name);
            GetString("value", out excel.Value);
            GetString("remarks", out excel.Remarks);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}