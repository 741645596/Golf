using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.28
    /// Desc    stage_node配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class StageNodeConfig : IConfig<uint>
    {
        /// <summary>
        /// 章节ID
        /// </summary>
        public uint ChapterId;

        /// <summary>
        /// 战斗节点id
        /// </summary>
        public uint NodeId;

        /// <summary>
        /// 界面显示功能id
        /// </summary>
        public uint UiShowId;

        /// <summary>
        /// 战斗类型
        /// </summary>
        public uint NodeType;

        /// <summary>
        /// 战斗节点名称，配置tid
        /// </summary>
        public string Name;

        /// <summary>
        /// 关卡挑战所需体力消耗
        /// </summary>
        public uint Cost;

        /// <summary>
        /// 前置节点;0表示无前置节点
        /// </summary>
        public uint Frontnode;

        /// <summary>
        /// 进入节点最低等级要求
        /// </summary>
        public uint LevelRequest;

        /// <summary>
        /// 横条背景图；配置资源名称
        /// </summary>
        public string Backdrop;

        /// <summary>
        /// 场景资源名
        /// </summary>
        public string ScenePath;

        /// <summary>
        /// 场景逻辑prefab
        /// </summary>
        public string ScenePrefab;

        /// <summary>
        /// 战斗波次
        /// </summary>
        public uint BattleWaves;

        /// <summary>
        /// 节点开场动画prefab
        /// </summary>
        public string NodePrefab;

        /// <summary>
        /// 战斗开场动画prefab
        /// </summary>
        public string NodePrefabBattle;

        /// <summary>
        /// 奖励库id
        /// </summary>
        public uint RewardList;

        /// <summary>
        /// 每波显示的奖励百分比
        /// </summary>
        public uint[] ShowRewardPercent;

        /// <summary>
        /// 战斗场景音乐id
        /// </summary>
        public uint NodeMusic;


        public uint GetKey()
        {
            return NodeId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.28
    /// Desc    stage_node配置文件访问接口
    /// </summary>
    public partial class StageNodeDao:BaseDao<StageNodeDao,uint,StageNodeConfig>
    {
        public override string GetName()
        {
            return "stage_node";
        }

        protected override void OnChangeLang(ref StageNodeConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                StageNodeConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.Name, language, ref cfg.Name);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.28
    /// Desc    stage_node配置文件解码器
    /// </summary>
    public partial class StageNodeDecoder : BaseCfgDecoder<StageNodeConfig, StageNodeCfgData>
    {
        public override string GetName()
        {
            return "stage_node";
        }

        protected override void ProcessRow(StageNodeConfig excel)
        {
            GetU32("chapter_id", out excel.ChapterId);
            GetU32("#node_id", out excel.NodeId);
            GetU32("ui_show_id", out excel.UiShowId);
            GetU32("node_type", out excel.NodeType);
            GetString("$name", out excel.Name);
            GetU32("cost", out excel.Cost);
            GetU32("frontnode", out excel.Frontnode);
            GetU32("levelRequest", out excel.LevelRequest);
            GetString("backdrop", out excel.Backdrop);
            GetString("scene_path", out excel.ScenePath);
            GetString("scene_prefab", out excel.ScenePrefab);
            GetU32("battle_waves", out excel.BattleWaves);
            GetString("node_prefab", out excel.NodePrefab);
            GetString("node_prefab_battle", out excel.NodePrefabBattle);
            GetU32("reward_list", out excel.RewardList);
            GetArr("show_reward_percent", StrHelper.ArrSplitLv1, out excel.ShowRewardPercent, ParseU32);
            GetU32("node_music", out excel.NodeMusic);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}