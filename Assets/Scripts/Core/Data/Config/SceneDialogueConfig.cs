using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.12
    /// Desc    scene_dialogue配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SceneDialogueConfig : IConfig<uint>
    {
        /// <summary>
        /// 剧情id
        /// </summary>
        public uint DialogueId;

        /// <summary>
        /// 下一剧情id
        /// </summary>
        public uint NextDialogueId;

        /// <summary>
        /// 左右方向
        /// </summary>
        public uint Direction;

        /// <summary>
        /// npc标志
        /// </summary>
        public uint NpcFlag;

        /// <summary>
        /// 姓名
        /// </summary>
        public string NameTid;

        /// <summary>
        /// 对白
        /// </summary>
        public string DialogueTid;

        /// <summary>
        /// 语音
        /// </summary>
        public uint DialogueVoice;

        /// <summary>
        /// 对白框样式
        /// 0-NPC
        /// 1-玩家
        /// 2-旁白
        /// </summary>
        public uint DialogueFrame;

        /// <summary>
        /// 全身像
        /// </summary>
        public string Image;

        /// <summary>
        /// 全身像位置x
        /// </summary>
        public int ImagePosX;

        /// <summary>
        /// 全身像位置y
        /// </summary>
        public int ImagePosY;

        /// <summary>
        /// 说话动画id
        /// </summary>
        public uint TalkAnim;

        /// <summary>
        /// 情绪泡泡id
        /// </summary>
        public uint Emotion;

        /// <summary>
        /// 全身像进入动画
        /// 0-不做动画
        /// 1-划入
        /// 2-淡入
        /// </summary>
        public uint ImageEnter;

        /// <summary>
        /// 全身像退出动画
        /// 0-不做动画
        /// 1-划出
        /// 2-淡出
        /// </summary>
        public uint ImageExit;

        /// <summary>
        /// 对话框进入动画
        /// 0-不做动画
        /// 1-划入
        /// </summary>
        public uint DialogueEnter;

        /// <summary>
        /// 对话框退出动画
        /// 0-不做动画
        /// 1-划出
        /// </summary>
        public uint DialogueExit;

        /// <summary>
        /// 滚动速度
        /// </summary>
        public uint DialogueSpeed;

        /// <summary>
        /// 对话时长
        /// </summary>
        public uint DialogueTime;

        /// <summary>
        /// 跳过简介
        /// </summary>
        public string SkipSummary;

        /// <summary>
        /// 玩家选项1
        /// </summary>
        public string OptionTid1;

        /// <summary>
        /// 剧情id1
        /// </summary>
        public uint OptionDialogue1;

        /// <summary>
        /// 玩家选项2
        /// </summary>
        public string OptionTid2;

        /// <summary>
        /// 剧情id2
        /// </summary>
        public uint OptionDialogue2;

        /// <summary>
        /// 玩家选项3
        /// </summary>
        public string OptionTid3;

        /// <summary>
        /// 剧情id3
        /// </summary>
        public uint OptionDialogue3;

        /// <summary>
        /// 剧情对白音乐id
        /// </summary>
        public uint DialogueMusic;


        public uint GetKey()
        {
            return DialogueId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.12
    /// Desc    scene_dialogue配置文件访问接口
    /// </summary>
    public partial class SceneDialogueDao:BaseDao<SceneDialogueDao,uint,SceneDialogueConfig>
    {
        public override string GetName()
        {
            return "scene_dialogue";
        }

        protected override void OnChangeLang(ref SceneDialogueConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                SceneDialogueConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.NameTid, language, ref cfg.NameTid);
                tDao.TryGetText(cfg.DialogueTid, language, ref cfg.DialogueTid);
                tDao.TryGetText(cfg.SkipSummary, language, ref cfg.SkipSummary);
                tDao.TryGetText(cfg.OptionTid1, language, ref cfg.OptionTid1);
                tDao.TryGetText(cfg.OptionTid2, language, ref cfg.OptionTid2);
                tDao.TryGetText(cfg.OptionTid3, language, ref cfg.OptionTid3);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.12
    /// Desc    scene_dialogue配置文件解码器
    /// </summary>
    public partial class SceneDialogueDecoder : BaseCfgDecoder<SceneDialogueConfig, SceneDialogueCfgData>
    {
        public override string GetName()
        {
            return "scene_dialogue";
        }

        protected override void ProcessRow(SceneDialogueConfig excel)
        {
            GetU32("#dialogue_id", out excel.DialogueId);
            GetU32("next_dialogue_id", out excel.NextDialogueId);
            GetU32("direction", out excel.Direction);
            GetU32("npc_flag", out excel.NpcFlag);
            GetString("$name_tid", out excel.NameTid);
            GetString("$dialogue_tid", out excel.DialogueTid);
            GetU32("dialogue_voice", out excel.DialogueVoice);
            GetU32("dialogue_frame", out excel.DialogueFrame);
            GetString("image", out excel.Image);
            GetI32("image_pos_x", out excel.ImagePosX);
            GetI32("image_pos_y", out excel.ImagePosY);
            GetU32("talk_anim", out excel.TalkAnim);
            GetU32("emotion", out excel.Emotion);
            GetU32("image_enter", out excel.ImageEnter);
            GetU32("image_exit", out excel.ImageExit);
            GetU32("dialogue_enter", out excel.DialogueEnter);
            GetU32("dialogue_exit", out excel.DialogueExit);
            GetU32("dialogue_speed", out excel.DialogueSpeed);
            GetU32("dialogue_time", out excel.DialogueTime);
            GetString("$skip_summary", out excel.SkipSummary);
            GetString("$option_tid1", out excel.OptionTid1);
            GetU32("option_dialogue1", out excel.OptionDialogue1);
            GetString("$option_tid2", out excel.OptionTid2);
            GetU32("option_dialogue2", out excel.OptionDialogue2);
            GetString("$option_tid3", out excel.OptionTid3);
            GetU32("option_dialogue3", out excel.OptionDialogue3);
            GetU32("dialogue_music", out excel.DialogueMusic);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}