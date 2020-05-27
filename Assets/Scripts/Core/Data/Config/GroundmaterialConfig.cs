using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.13
    /// Desc    groundmaterial配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class GroundmaterialConfig : IConfig<uint>
    {
        /// <summary>
        /// Id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 地质类型
        /// </summary>
        public int GroundType;

        /// <summary>
        /// 地质名称
        /// </summary>
        public int NameTid;

        /// <summary>
        /// 速度衰减斜率
        /// </summary>
        public int DecreaseSlope;

        /// <summary>
        /// 速度衰减偏差
        /// </summary>
        public int DecreaseBias;

        /// <summary>
        /// 最小反弹速度
        /// </summary>
        public int MinBounceVelocity;

        /// <summary>
        /// 滚动摩擦力
        /// </summary>
        public int Friction;

        /// <summary>
        /// 旋球校正速度_1
        /// </summary>
        public int SpinAddition1;

        /// <summary>
        /// 旋球校正速度_2
        /// </summary>
        public int SpinAddition2;

        /// <summary>
        /// 旋球校正速度_3
        /// </summary>
        public int SpinAddition3;

        /// <summary>
        /// 旋球校正速度_4
        /// </summary>
        public int SpinAddition4;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.13
    /// Desc    groundmaterial配置文件访问接口
    /// </summary>
    public partial class GroundmaterialDao:BaseDao<GroundmaterialDao,uint,GroundmaterialConfig>
    {
        public override string GetName()
        {
            return "groundmaterial";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.13
    /// Desc    groundmaterial配置文件解码器
    /// </summary>
    public partial class GroundmaterialDecoder : BaseCfgDecoder<GroundmaterialConfig, GroundmaterialCfgData>
    {
        public override string GetName()
        {
            return "groundmaterial";
        }

        protected override void ProcessRow(GroundmaterialConfig excel)
        {
            GetU32("id", out excel.Id);
            GetI32("ground_type", out excel.GroundType);
            GetI32("name_tid", out excel.NameTid);
            GetI32("decrease_slope", out excel.DecreaseSlope);
            GetI32("decrease_bias", out excel.DecreaseBias);
            GetI32("min_bounce_velocity", out excel.MinBounceVelocity);
            GetI32("friction", out excel.Friction);
            GetI32("spin_addition_1", out excel.SpinAddition1);
            GetI32("spin_addition_2", out excel.SpinAddition2);
            GetI32("spin_addition_3", out excel.SpinAddition3);
            GetI32("spin_addition_4", out excel.SpinAddition4);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}