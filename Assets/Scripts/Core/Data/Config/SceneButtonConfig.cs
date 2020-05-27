using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    scene_button配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SceneButtonConfig : IConfig<uint>
    {
        /// <summary>
        /// 虚拟按钮状态id
        /// </summary>
        public uint ButtonStateId;

        /// <summary>
        /// 状态启动事件
        /// </summary>
        public uint OnEvent;

        /// <summary>
        /// 状态关闭事件
        /// </summary>
        public uint OffEvent;

        /// <summary>
        /// 按钮状态的优先级
        /// </summary>
        public uint StatePriority;

        /// <summary>
        /// 状态触发事件
        /// </summary>
        public uint TriggerEvent;

        /// <summary>
        /// 状态触发动作
        /// </summary>
        public string TriggerMotion;

        /// <summary>
        /// 状态响应冷却时间
        /// </summary>
        public uint CooldownTime;


        public uint GetKey()
        {
            return ButtonStateId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    scene_button配置文件访问接口
    /// </summary>
    public partial class SceneButtonDao:BaseDao<SceneButtonDao,uint,SceneButtonConfig>
    {
        public override string GetName()
        {
            return "scene_button";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    scene_button配置文件解码器
    /// </summary>
    public partial class SceneButtonDecoder : BaseCfgDecoder<SceneButtonConfig, SceneButtonCfgData>
    {
        public override string GetName()
        {
            return "scene_button";
        }

        protected override void ProcessRow(SceneButtonConfig excel)
        {
            GetU32("#button_state_id", out excel.ButtonStateId);
            GetU32("on_event", out excel.OnEvent);
            GetU32("off_event", out excel.OffEvent);
            GetU32("state_priority", out excel.StatePriority);
            GetU32("trigger_event", out excel.TriggerEvent);
            GetString("trigger_motion", out excel.TriggerMotion);
            GetU32("cooldown_time", out excel.CooldownTime);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}