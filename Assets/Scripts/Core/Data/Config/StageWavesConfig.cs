using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    stage_waves配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class StageWavesConfig : IConfig<uint>
    {
        /// <summary>
        /// 战斗波次ID
        /// </summary>
        public uint Id;

        /// <summary>
        /// 本波次出现顺序；0为随机顺序；0以上为固定顺序
        /// </summary>
        public uint Order;

        /// <summary>
        /// 随机怪生成点；索引到对应prefab文件的position坐标信息
        /// </summary>
        public uint[] RandomPosition;

        /// <summary>
        /// 随机怪生成库；索引到creature_library.csv
        /// </summary>
        public uint RandomList;

        /// <summary>
        /// 指定怪生成点；索引到对应prefab文件的position坐标信息
        /// </summary>
        public uint[] RegularPosition;

        /// <summary>
        /// 指定怪生成库；索引到creature_library.csv
        /// </summary>
        public uint RegularList;

        /// <summary>
        /// 设定怪物初始普攻CD；格式：x|y|z|；随机填充到各个位置
        /// </summary>
        public uint[] InitialCd;

        /// <summary>
        /// 表示点被攻击的逻辑信息，点*优先级*最小列最大列|
        /// </summary>
        public UintArray[] PositionInfo;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    stage_waves配置文件访问接口
    /// </summary>
    public partial class StageWavesDao:BaseDao<StageWavesDao,uint,StageWavesConfig>
    {
        public override string GetName()
        {
            return "stage_waves";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    stage_waves配置文件解码器
    /// </summary>
    public partial class StageWavesDecoder : BaseCfgDecoder<StageWavesConfig, StageWavesCfgData>
    {
        public override string GetName()
        {
            return "stage_waves";
        }

        protected override void ProcessRow(StageWavesConfig excel)
        {
            GetU32("id", out excel.Id);
            GetU32("order", out excel.Order);
            GetArr("random_position", StrHelper.ArrSplitLv1, out excel.RandomPosition, ParseU32);
            GetU32("random_list", out excel.RandomList);
            GetArr("regular_position", StrHelper.ArrSplitLv1, out excel.RegularPosition, ParseU32);
            GetU32("regular_list", out excel.RegularList);
            GetArr("initial_cd", StrHelper.ArrSplitLv1, out excel.InitialCd, ParseU32);
            GetArr("position_info", StrHelper.ArrSplitLv1, out excel.PositionInfo, ParseArr<UintArray, uint>);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}