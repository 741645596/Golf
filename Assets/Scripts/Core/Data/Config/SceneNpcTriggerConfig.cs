using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    scene_npc_trigger配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SceneNpcTriggerConfig : IConfig<uint>
    {
        /// <summary>
        /// NPC管理器id
        /// </summary>
        public uint NpcCreateId;

        /// <summary>
        /// 接近事件
        /// </summary>
        public uint NearEvent;

        /// <summary>
        /// 远离事件
        /// </summary>
        public uint LeaveEvent;

        /// <summary>
        /// 接近表情泡泡
        /// </summary>
        public uint NearExpression;

        /// <summary>
        /// 接近动作
        /// </summary>
        public string NearMotion;

        /// <summary>
        /// 触发事件
        /// </summary>
        public uint TriggerEvent;

        /// <summary>
        /// 触发表情泡泡
        /// </summary>
        public uint TriggerExpression;

        /// <summary>
        /// 触发动作
        /// </summary>
        public string TriggerMotion;


        public uint GetKey()
        {
            return NpcCreateId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    scene_npc_trigger配置文件访问接口
    /// </summary>
    public partial class SceneNpcTriggerDao:BaseDao<SceneNpcTriggerDao,uint,SceneNpcTriggerConfig>
    {
        public override string GetName()
        {
            return "scene_npc_trigger";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    scene_npc_trigger配置文件解码器
    /// </summary>
    public partial class SceneNpcTriggerDecoder : BaseCfgDecoder<SceneNpcTriggerConfig, SceneNpcTriggerCfgData>
    {
        public override string GetName()
        {
            return "scene_npc_trigger";
        }

        protected override void ProcessRow(SceneNpcTriggerConfig excel)
        {
            GetU32("#npc_create_id", out excel.NpcCreateId);
            GetU32("near_event", out excel.NearEvent);
            GetU32("leave_event", out excel.LeaveEvent);
            GetU32("near_expression", out excel.NearExpression);
            GetString("near_motion", out excel.NearMotion);
            GetU32("trigger_event", out excel.TriggerEvent);
            GetU32("trigger_expression", out excel.TriggerExpression);
            GetString("trigger_motion", out excel.TriggerMotion);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}