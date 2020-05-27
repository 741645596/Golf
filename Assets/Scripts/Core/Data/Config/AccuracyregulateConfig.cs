using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.15
    /// Desc    accuracyregulate配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class AccuracyregulateConfig : IConfig<int>
    {
        /// <summary>
        /// Id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 精准度
        /// </summary>
        public int Accuracy;

        /// <summary>
        /// 风切校正
        /// </summary>
        public int WindRegulate;


        public int GetKey()
        {
            return Accuracy;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.15
    /// Desc    accuracyregulate配置文件访问接口
    /// </summary>
    public partial class AccuracyregulateDao:BaseDao<AccuracyregulateDao,int,AccuracyregulateConfig>
    {
        public override string GetName()
        {
            return "accuracyregulate";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.15
    /// Desc    accuracyregulate配置文件解码器
    /// </summary>
    public partial class AccuracyregulateDecoder : BaseCfgDecoder<AccuracyregulateConfig, AccuracyregulateCfgData>
    {
        public override string GetName()
        {
            return "accuracyregulate";
        }

        protected override void ProcessRow(AccuracyregulateConfig excel)
        {
            GetU32("id", out excel.Id);
            GetI32("#accuracy", out excel.Accuracy);
            GetI32("wind_regulate", out excel.WindRegulate);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}