using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    npc配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class NpcConfig : IConfig<uint>
    {
        /// <summary>
        /// 表字段唯一id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 创建事件
        /// 1加载创建
        /// 2战斗获胜后创建
        /// 3成就后创建
        /// 4事件创建
        /// </summary>
        public uint CreateEnum;

        /// <summary>
        /// 创建加载关联id
        /// </summary>
        public uint CreateRelevanceId;

        /// <summary>
        /// 刷新个数
        /// </summary>
        public uint RefreshNum;

        /// <summary>
        /// 刷新范围
        /// </summary>
        public uint RefreshRange;

        /// <summary>
        /// 销毁事件
        /// 1不销毁
        /// 2战斗获胜后销毁
        /// 3成就后销毁
        /// 4事件销毁
        /// </summary>
        public uint VanishEnum;

        /// <summary>
        /// 销毁模型关联id
        /// </summary>
        public uint VanishRelevanceId;

        /// <summary>
        /// npc模型
        /// </summary>
        public string NpcModelPath;

        /// <summary>
        /// npc的AI
        /// </summary>
        public uint NpcAi;

        /// <summary>
        /// npc警戒范围
        /// </summary>
        public uint VigilanceRange;

        /// <summary>
        /// npc基础移动速度
        /// </summary>
        public uint NpcSpeed;

        /// <summary>
        /// npc跑动速度
        /// </summary>
        public uint NpcRunSpeed;

        /// <summary>
        /// npc巡逻半径范围
        /// </summary>
        public uint NpcPatrolRadius;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    npc配置文件访问接口
    /// </summary>
    public partial class NpcDao:BaseDao<NpcDao,uint,NpcConfig>
    {
        public override string GetName()
        {
            return "npc";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    npc配置文件解码器
    /// </summary>
    public partial class NpcDecoder : BaseCfgDecoder<NpcConfig, NpcCfgData>
    {
        public override string GetName()
        {
            return "npc";
        }

        protected override void ProcessRow(NpcConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetU32("create_enum", out excel.CreateEnum);
            GetU32("create_relevance_id", out excel.CreateRelevanceId);
            GetU32("refresh_num", out excel.RefreshNum);
            GetU32("refresh_range", out excel.RefreshRange);
            GetU32("vanish_enum", out excel.VanishEnum);
            GetU32("vanish_relevance_id", out excel.VanishRelevanceId);
            GetString("npc_model_path", out excel.NpcModelPath);
            GetU32("npc_ai", out excel.NpcAi);
            GetU32("vigilance_range", out excel.VigilanceRange);
            GetU32("npc_speed", out excel.NpcSpeed);
            GetU32("npc_run_speed", out excel.NpcRunSpeed);
            GetU32("npc_patrol_radius", out excel.NpcPatrolRadius);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}