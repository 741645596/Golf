using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    random_reward配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class RandomRewardConfig : IConfig<uint>
    {
        /// <summary>
        /// 编号
        /// </summary>
        public uint Id;

        /// <summary>
        /// 策划备注字段
        /// </summary>
        public string DesignerRemarks;

        /// <summary>
        /// 固定掉落,0随机掉落，1固定掉落。同一个奖励id只会是同一个值。
        /// </summary>
        public uint FixedDrop;

        /// <summary>
        /// 随机库总掉落次数，非固定掉落时，同一个奖励id只会是同一个值。代表这个奖励库随机多少次。
        /// </summary>
        public uint DropTime;

        /// <summary>
        /// 是否重复掉落，该奖励库是否重复掉落。0不重复，1可重复。不重复的情况下，当随机到某物品时，该物品接下来不会再被随机出来
        /// </summary>
        public uint IsRepeat;

        /// <summary>
        /// 奖励大类型（1道具，2英雄）
        /// </summary>
        public uint Type;

        /// <summary>
        /// 奖励小类型（如果大类型是道具1，具体关联到item表的具体id。如果大类型是英雄2，具体关联到hero_info内的英雄id）
        /// </summary>
        public uint SubType;

        /// <summary>
        /// 数量最小值，奖励的数值区间
        /// </summary>
        public uint NumMin;

        /// <summary>
        /// 数量最大值
        /// </summary>
        public uint NumMax ;

        /// <summary>
        /// 权重，用于物品的掉落概率。
        /// </summary>
        public uint Weight;

        /// <summary>
        /// 是否在关卡或奖励界面显示，0不显示，1显示。
        /// </summary>
        public uint IsShow;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    random_reward配置文件访问接口
    /// </summary>
    public partial class RandomRewardDao:BaseDao<RandomRewardDao,uint,RandomRewardConfig>
    {
        public override string GetName()
        {
            return "random_reward";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    random_reward配置文件解码器
    /// </summary>
    public partial class RandomRewardDecoder : BaseCfgDecoder<RandomRewardConfig, RandomRewardCfgData>
    {
        public override string GetName()
        {
            return "random_reward";
        }

        protected override void ProcessRow(RandomRewardConfig excel)
        {
            GetU32("id", out excel.Id);
            GetString("designer_remarks", out excel.DesignerRemarks);
            GetU32("fixed_drop", out excel.FixedDrop);
            GetU32("drop_time", out excel.DropTime);
            GetU32("is_repeat", out excel.IsRepeat);
            GetU32("type", out excel.Type);
            GetU32("sub_type", out excel.SubType);
            GetU32("num_min", out excel.NumMin);
            GetU32("num_max ", out excel.NumMax );
            GetU32("weight", out excel.Weight);
            GetU32("is_show", out excel.IsShow);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}