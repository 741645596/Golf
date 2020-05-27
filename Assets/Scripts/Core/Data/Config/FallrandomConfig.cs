using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.13
    /// Desc    fallrandom配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class FallrandomConfig : IConfig<uint>
    {
        /// <summary>
        /// 珠子掉落概率模板ID
        /// </summary>
        public uint Fallid;

        /// <summary>
        /// 珠子掉落概率模板
        /// </summary>
        public UintArray[] Fallrandom;

        /// <summary>
        /// 珠子生成概率模板
        /// </summary>
        public uint[] Generaterandom;


        public uint GetKey()
        {
            return Fallid;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.13
    /// Desc    fallrandom配置文件访问接口
    /// </summary>
    public partial class FallrandomDao:BaseDao<FallrandomDao,uint,FallrandomConfig>
    {
        public override string GetName()
        {
            return "fallrandom";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.13
    /// Desc    fallrandom配置文件解码器
    /// </summary>
    public partial class FallrandomDecoder : BaseCfgDecoder<FallrandomConfig, FallrandomCfgData>
    {
        public override string GetName()
        {
            return "fallrandom";
        }

        protected override void ProcessRow(FallrandomConfig excel)
        {
            GetU32("fallid", out excel.Fallid);
            GetArr("fallrandom", StrHelper.ArrSplitLv1, out excel.Fallrandom, ParseArr<UintArray, uint>);
            GetArr("generaterandom", StrHelper.ArrSplitLv1, out excel.Generaterandom, ParseU32);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}