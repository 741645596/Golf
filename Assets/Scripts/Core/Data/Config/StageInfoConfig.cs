using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.22
    /// Desc    stage_info配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class StageInfoConfig : IConfig<uint>
    {
        /// <summary>
        /// 章节ID
        /// </summary>
        public uint Chapter;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 区域
        /// </summary>
        public uint Area;

        /// <summary>
        /// 所在地图UI位置点
        /// </summary>
        public uint Pos;

        /// <summary>
        /// 前置关卡0表示无前置章节
        /// </summary>
        public uint FrontChapter;

        /// <summary>
        /// 进入关卡最低等级要求
        /// </summary>
        public uint LevelRequest;

        /// <summary>
        /// 关卡浮空图标路径
        /// </summary>
        public string Image;

        /// <summary>
        /// 场景配置资源名称
        /// </summary>
        public string Scene;

        /// <summary>
        /// 场景prefab配置资源名称
        /// </summary>
        public string ScenePerfab;

        /// <summary>
        /// 横条背景图配置资源名称
        /// </summary>
        public string Backdrop;

        /// <summary>
        /// 怪物图片配置怪物ID索引怪物表
        /// </summary>
        public uint[] CreaturePreview;

        /// <summary>
        /// 奖励库索引到item
        /// </summary>
        public uint[] RewardList;

        /// <summary>
        /// 事件id组界面预览用
        /// </summary>
        public uint[] AchievementId;

        /// <summary>
        /// 宝箱事件计数
        /// </summary>
        public uint BoxNumStatistics;

        /// <summary>
        /// 冒险场景音乐id
        /// </summary>
        public uint SceneMusic;


        public uint GetKey()
        {
            return Chapter;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.22
    /// Desc    stage_info配置文件访问接口
    /// </summary>
    public partial class StageInfoDao:BaseDao<StageInfoDao,uint,StageInfoConfig>
    {
        public override string GetName()
        {
            return "stage_info";
        }

        protected override void OnChangeLang(ref StageInfoConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                StageInfoConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.Name, language, ref cfg.Name);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.22
    /// Desc    stage_info配置文件解码器
    /// </summary>
    public partial class StageInfoDecoder : BaseCfgDecoder<StageInfoConfig, StageInfoCfgData>
    {
        public override string GetName()
        {
            return "stage_info";
        }

        protected override void ProcessRow(StageInfoConfig excel)
        {
            GetU32("chapter", out excel.Chapter);
            GetString("$name", out excel.Name);
            GetU32("area", out excel.Area);
            GetU32("pos", out excel.Pos);
            GetU32("front_chapter", out excel.FrontChapter);
            GetU32("level_request", out excel.LevelRequest);
            GetString("image", out excel.Image);
            GetString("scene", out excel.Scene);
            GetString("scene_perfab", out excel.ScenePerfab);
            GetString("backdrop", out excel.Backdrop);
            GetArr("creature_preview", StrHelper.ArrSplitLv1, out excel.CreaturePreview, ParseU32);
            GetArr("reward_list", StrHelper.ArrSplitLv1, out excel.RewardList, ParseU32);
            GetArr("achievement_id", StrHelper.ArrSplitLv1, out excel.AchievementId, ParseU32);
            GetU32("box_num_statistics", out excel.BoxNumStatistics);
            GetU32("scene_music", out excel.SceneMusic);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}