using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.19
    /// Desc    hero_info配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class HeroInfoConfig : IConfig<uint>
    {
        /// <summary>
        /// 英雄id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 策划备注字段
        /// </summary>
        public string DesignerRemarks;

        /// <summary>
        /// 英雄职业
        /// 1步兵
        /// 2骑兵
        /// 3弓兵
        /// </summary>
        public uint Profession;

        /// <summary>
        /// 英雄初始星级
        /// 最高不超过6
        /// </summary>
        public uint StarLv;

        /// <summary>
        /// 英雄品质
        /// 1 N
        /// 2 R
        /// 3 SR
        /// 4 SSR
        /// </summary>
        public uint HeroQuality;

        /// <summary>
        /// 英雄星座
        /// 1水瓶2双鱼3白羊4金牛5双子6巨蟹7狮子8处女9天枰10天蝎11射手12摩羯
        /// </summary>
        public uint Constellation;

        /// <summary>
        /// 英雄元素
        /// 1火
        /// 2水
        /// 3土
        /// 4光
        /// 5暗
        /// 配置方式：初始|可选|可选…
        /// </summary>
        public uint[] Element;

        /// <summary>
        /// 英雄碎片id
        /// </summary>
        public uint HeroDebrisId;

        /// <summary>
        /// 英雄普攻技能id
        /// </summary>
        public uint HeroCommonSkill;

        /// <summary>
        /// 英雄主动技能id
        /// </summary>
        public uint HeroActiveSkillId;

        /// <summary>
        /// 英雄2D立绘索引
        /// </summary>
        public string Live2dPath;

        /// <summary>
        /// 英雄模型索引
        /// </summary>
        public string ModelPath;

        /// <summary>
        /// 英雄名称TID
        /// （界面显示）
        /// </summary>
        public string HeroName;

        /// <summary>
        /// 英雄介绍TID
        /// （界面显示）
        /// </summary>
        public string HeroIntroduce;

        /// <summary>
        /// 英雄头像索引
        /// </summary>
        public string HeroHeadPortraitPath;

        /// <summary>
        /// 英雄星座图片索引
        /// </summary>
        public string HeroConstellationIconPath;

        /// <summary>
        /// 英雄星座TIPS中的TID
        /// </summary>
        public string HeroConstellationTid;

        /// <summary>
        /// 英雄品质背景底图路径索引
        /// </summary>
        public string HeroQualityPath;

        /// <summary>
        /// 英雄战斗界面图片索引
        /// </summary>
        public string HeroBattleIconPath;

        /// <summary>
        /// 英雄大招id
        /// </summary>
        public uint HeroGodSkillId;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.19
    /// Desc    hero_info配置文件访问接口
    /// </summary>
    public partial class HeroInfoDao:BaseDao<HeroInfoDao,uint,HeroInfoConfig>
    {
        public override string GetName()
        {
            return "hero_info";
        }

        protected override void OnChangeLang(ref HeroInfoConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                HeroInfoConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.HeroName, language, ref cfg.HeroName);
                tDao.TryGetText(cfg.HeroIntroduce, language, ref cfg.HeroIntroduce);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.19
    /// Desc    hero_info配置文件解码器
    /// </summary>
    public partial class HeroInfoDecoder : BaseCfgDecoder<HeroInfoConfig, HeroInfoCfgData>
    {
        public override string GetName()
        {
            return "hero_info";
        }

        protected override void ProcessRow(HeroInfoConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetString("designer_remarks", out excel.DesignerRemarks);
            GetU32("profession", out excel.Profession);
            GetU32("star_lv", out excel.StarLv);
            GetU32("hero_quality", out excel.HeroQuality);
            GetU32("constellation", out excel.Constellation);
            GetArr("element", StrHelper.ArrSplitLv1, out excel.Element, ParseU32);
            GetU32("hero_debris_id", out excel.HeroDebrisId);
            GetU32("hero_common_skill", out excel.HeroCommonSkill);
            GetU32("hero_active_skill_id", out excel.HeroActiveSkillId);
            GetString("live2d_path", out excel.Live2dPath);
            GetString("model_path", out excel.ModelPath);
            GetString("$hero_name", out excel.HeroName);
            GetString("$hero_introduce", out excel.HeroIntroduce);
            GetString("hero_head_portrait_path", out excel.HeroHeadPortraitPath);
            GetString("hero_constellation_icon_path", out excel.HeroConstellationIconPath);
            GetString("hero_constellation_tid", out excel.HeroConstellationTid);
            GetString("hero_quality_path", out excel.HeroQualityPath);
            GetString("hero_battle_icon_path", out excel.HeroBattleIconPath);
            GetU32("hero_god_skill_id", out excel.HeroGodSkillId);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}