using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.13
    /// Desc    scene_box配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SceneBoxConfig : IConfig<uint>
    {
        /// <summary>
        /// 宝箱id
        /// </summary>
        public uint BoxId;
        
        /// <summary>
        /// 是否统计关卡计数
        /// 0：不计入关卡计数
        /// 1：计入关卡计数
        /// </summary>
        public uint BoxCount;
        
        /// <summary>
        /// 奖励id
        /// </summary>
        public uint RewardId;
        
        /// <summary>
        /// 标题文本
        /// </summary>
        public string TitleTid;
        
        /// <summary>
        /// 内容文本
        /// </summary>
        public string ContentTid;
        
        
        public uint GetKey()
        {
            return BoxId;
        }
    }
    #endregion
    
    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.13
    /// Desc    scene_box配置文件访问接口
    /// </summary>
    public partial class SceneBoxDao: BaseDao<SceneBoxDao, uint, SceneBoxConfig>
    {
        public override string GetName()
        {
            return "scene_box";
        }
        
        protected override void OnChangeLang(ref SceneBoxConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++) {
                SceneBoxConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.TitleTid, language, ref cfg.TitleTid);
                tDao.TryGetText(cfg.ContentTid, language, ref cfg.ContentTid);
            }
        }
    }
    #endregion
    
    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.13
    /// Desc    scene_box配置文件解码器
    /// </summary>
    public partial class SceneBoxDecoder : BaseCfgDecoder<SceneBoxConfig, SceneBoxCfgData>
    {
        public override string GetName()
        {
            return "scene_box";
        }
        
        protected override void ProcessRow(SceneBoxConfig excel)
        {
            GetU32("#box_id", out excel.BoxId);
            GetU32("box_count", out excel.BoxCount);
            GetU32("reward_id", out excel.RewardId);
            GetString("$title_tid", out excel.TitleTid);
            GetString("$content_tid", out excel.ContentTid);
            ProcessRowExt(excel);
        }
    }
    
#endif
    #endregion
}