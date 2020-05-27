using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.16
    /// Desc    achievement配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class AchievementConfig : IConfig<uint>
    {
        /// <summary>
        /// 表字段唯一id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 成就名称,用于界面显示所用
        /// </summary>
        public string Name;

        /// <summary>
        /// 事件枚举
        /// 1战斗关卡完成事件
        /// 2宝箱触发事件
        /// </summary>
        public uint EventEnum;

        /// <summary>
        /// 关联id
        /// 事件枚举为1则关联战斗id
        /// 事件枚举为2则关联宝箱id
        /// </summary>
        public uint[] RelevanceId;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.16
    /// Desc    achievement配置文件访问接口
    /// </summary>
    public partial class AchievementDao:BaseDao<AchievementDao,uint,AchievementConfig>
    {
        public override string GetName()
        {
            return "achievement";
        }

        protected override void OnChangeLang(ref AchievementConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                AchievementConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.Name, language, ref cfg.Name);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.16
    /// Desc    achievement配置文件解码器
    /// </summary>
    public partial class AchievementDecoder : BaseCfgDecoder<AchievementConfig, AchievementCfgData>
    {
        public override string GetName()
        {
            return "achievement";
        }

        protected override void ProcessRow(AchievementConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetString("$name", out excel.Name);
            GetU32("event_enum", out excel.EventEnum);
            GetArr("relevance_id", StrHelper.ArrSplitLv1, out excel.RelevanceId, ParseU32);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}