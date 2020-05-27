using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.1.28
    /// Desc    random_tips配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class RandomTipsConfig : IConfig<uint>
    {
        /// <summary>
        /// 表字段唯一id
        /// </summary>
        public uint Id;

        /// <summary>
        /// Tips属于功能枚举
        /// 1 Loading界面
        /// 2“养成”功能界面
        /// </summary>
        public uint Type;

        /// <summary>
        /// TIPS标题,配置TID
        /// </summary>
        public string Title;

        /// <summary>
        /// 内容文本读取的TID
        /// </summary>
        public string Tid;

        /// <summary>
        /// TIPS图示资源路径
        /// </summary>
        public string TipsImage;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.1.28
    /// Desc    random_tips配置文件访问接口
    /// </summary>
    public partial class RandomTipsDao:BaseDao<RandomTipsDao,uint,RandomTipsConfig>
    {
        public override string GetName()
        {
            return "random_tips";
        }

        protected override void OnChangeLang(ref RandomTipsConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                RandomTipsConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.Title, language, ref cfg.Title);
                tDao.TryGetText(cfg.Tid, language, ref cfg.Tid);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.1.28
    /// Desc    random_tips配置文件解码器
    /// </summary>
    public partial class RandomTipsDecoder : BaseCfgDecoder<RandomTipsConfig, RandomTipsCfgData>
    {
        public override string GetName()
        {
            return "random_tips";
        }

        protected override void ProcessRow(RandomTipsConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetU32("type", out excel.Type);
            GetString("$title", out excel.Title);
            GetString("$tid", out excel.Tid);
            GetString("tips_image", out excel.TipsImage);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}