using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    hero_star配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class HeroStarConfig : IConfig<uint>
    {
        /// <summary>
        /// 表字段唯一id
        /// </summary>
        public uint HeroId;

        /// <summary>
        /// 英雄星等级星级
        /// </summary>
        public uint StarLv;

        /// <summary>
        /// 英雄升星所需英雄碎片数量
        /// </summary>
        public uint StarUplvResourceNum;

        /// <summary>
        /// 升星增加的生命值
        /// </summary>
        public uint StarAddHp;

        /// <summary>
        /// 升星增加的攻击值
        /// </summary>
        public uint StarAddAtk;

        /// <summary>
        /// 升星增加的防御值
        /// </summary>
        public uint StarAddDef;

        /// <summary>
        /// 星级对应的英雄天赋技能id
        /// </summary>
        public uint TelentPassiveSkillId;


        public uint GetKey()
        {
            return HeroId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    hero_star配置文件访问接口
    /// </summary>
    public partial class HeroStarDao:BaseDao<HeroStarDao,uint,HeroStarConfig>
    {
        public override string GetName()
        {
            return "hero_star";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    hero_star配置文件解码器
    /// </summary>
    public partial class HeroStarDecoder : BaseCfgDecoder<HeroStarConfig, HeroStarCfgData>
    {
        public override string GetName()
        {
            return "hero_star";
        }

        protected override void ProcessRow(HeroStarConfig excel)
        {
            GetU32("hero_id", out excel.HeroId);
            GetU32("star_lv", out excel.StarLv);
            GetU32("star_uplv_resource_num", out excel.StarUplvResourceNum);
            GetU32("star_add_hp", out excel.StarAddHp);
            GetU32("star_add_atk", out excel.StarAddAtk);
            GetU32("star_add_def", out excel.StarAddDef);
            GetU32("telent_passive_skill_id", out excel.TelentPassiveSkillId);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}