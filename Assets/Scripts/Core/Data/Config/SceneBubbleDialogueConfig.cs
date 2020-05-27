using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.1
    /// Desc    scene_bubble_dialogue配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SceneBubbleDialogueConfig : IConfig<uint>
    {
        /// <summary>
        /// 泡泡id
        /// </summary>
        public uint BubbleId;

        /// <summary>
        /// 触发事件
        /// </summary>
        public uint TriggerEvent;

        /// <summary>
        /// 左右方向
        /// 0-左
        /// 1-右
        /// </summary>
        public uint Direction;

        /// <summary>
        /// 头像
        /// </summary>
        public string Image;

        /// <summary>
        /// 对白
        /// </summary>
        public string DialogueTid;

        /// <summary>
        /// 语音
        /// </summary>
        public uint DialogueVoice;


        public uint GetKey()
        {
            return BubbleId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.1
    /// Desc    scene_bubble_dialogue配置文件访问接口
    /// </summary>
    public partial class SceneBubbleDialogueDao:BaseDao<SceneBubbleDialogueDao,uint,SceneBubbleDialogueConfig>
    {
        public override string GetName()
        {
            return "scene_bubble_dialogue";
        }

        protected override void OnChangeLang(ref SceneBubbleDialogueConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                SceneBubbleDialogueConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.DialogueTid, language, ref cfg.DialogueTid);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.1
    /// Desc    scene_bubble_dialogue配置文件解码器
    /// </summary>
    public partial class SceneBubbleDialogueDecoder : BaseCfgDecoder<SceneBubbleDialogueConfig, SceneBubbleDialogueCfgData>
    {
        public override string GetName()
        {
            return "scene_bubble_dialogue";
        }

        protected override void ProcessRow(SceneBubbleDialogueConfig excel)
        {
            GetU32("#bubble_id", out excel.BubbleId);
            GetU32("trigger_event", out excel.TriggerEvent);
            GetU32("direction", out excel.Direction);
            GetString("image", out excel.Image);
            GetString("$dialogue_tid", out excel.DialogueTid);
            GetU32("dialogue_voice", out excel.DialogueVoice);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}