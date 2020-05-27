using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.27
    /// Desc    scene_task配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SceneTaskConfig : IConfig<uint>
    {
        /// <summary>
        /// 事件id
        /// </summary>
        public uint TaskId;

        /// <summary>
        /// 触发事件
        /// </summary>
        public uint TriggerEvent;

        /// <summary>
        /// 任务结束抛出的事件
        /// </summary>
        public uint FinishEvent;

        /// <summary>
        /// 任务完结抛出的事件
        /// </summary>
        public uint EndEvent;

        /// <summary>
        /// 开始条件
        /// 0：无条件
        /// 1：完成指定任务
        /// 2：完成指定战斗
        /// 3：指定NPC管理器处于创建状态
        /// </summary>
        public uint StartCondition;

        /// <summary>
        /// 开始参数
        /// </summary>
        public uint[] StartParameter;

        /// <summary>
        /// 完成条件
        /// 0：结束后完成
        /// 1：结束后不完成
        /// 2：完成指定任务
        /// 3：完成指定战斗
        /// </summary>
        public uint EndCondition;

        /// <summary>
        /// 完成参数
        /// </summary>
        public uint[] EndParameter;

        /// <summary>
        /// 完成有效时间
        /// =0：永久
        /// >0-：xx秒
        /// </summary>
        public uint FinishActiveTime;

        /// <summary>
        /// 任务期间是否限制操作
        /// 0：不限制
        /// 1：限制
        /// </summary>
        public uint LimitMainUi;

        /// <summary>
        /// 任务期间是否隐藏主界面
        /// 0：不隐藏
        /// 1：隐藏
        /// </summary>
        public uint HideMainUi;

        /// <summary>
        /// 任务类型
        /// 0：空任务
        /// 1：战斗任务
        /// 2：宝箱任务
        /// 3：剧情任务
        /// 4：表现任务
        /// </summary>
        public uint EventType;

        /// <summary>
        /// 任务参数
        /// </summary>
        public uint EventParameter;


        public uint GetKey()
        {
            return TaskId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.27
    /// Desc    scene_task配置文件访问接口
    /// </summary>
    public partial class SceneTaskDao:BaseDao<SceneTaskDao,uint,SceneTaskConfig>
    {
        public override string GetName()
        {
            return "scene_task";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.27
    /// Desc    scene_task配置文件解码器
    /// </summary>
    public partial class SceneTaskDecoder : BaseCfgDecoder<SceneTaskConfig, SceneTaskCfgData>
    {
        public override string GetName()
        {
            return "scene_task";
        }

        protected override void ProcessRow(SceneTaskConfig excel)
        {
            GetU32("#task_id", out excel.TaskId);
            GetU32("trigger_event", out excel.TriggerEvent);
            GetU32("finish_event", out excel.FinishEvent);
            GetU32("end_event", out excel.EndEvent);
            GetU32("start_condition", out excel.StartCondition);
            GetArr("start_parameter", StrHelper.ArrSplitLv1, out excel.StartParameter, ParseU32);
            GetU32("end_condition", out excel.EndCondition);
            GetArr("end_parameter", StrHelper.ArrSplitLv1, out excel.EndParameter, ParseU32);
            GetU32("finish_active_time", out excel.FinishActiveTime);
            GetU32("limit_main_ui", out excel.LimitMainUi);
            GetU32("hide_main_ui", out excel.HideMainUi);
            GetU32("event_type", out excel.EventType);
            GetU32("event_parameter", out excel.EventParameter);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}