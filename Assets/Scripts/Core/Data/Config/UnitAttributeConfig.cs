using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.19
    /// Desc    unit_attribute配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class UnitAttributeConfig : IConfig<uint>
    {
        /// <summary>
        /// 英雄id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 英雄等级
        /// </summary>
        public uint HeroLv;

        /// <summary>
        /// 英雄经验
        /// </summary>
        public uint HeroExp;

        /// <summary>
        /// 英雄基础生命值
        /// </summary>
        public uint HeroBasicsHp;

        /// <summary>
        /// 英雄基础攻击值
        /// </summary>
        public uint HeroBasicsAtk;

        /// <summary>
        /// 英雄基础防御值
        /// </summary>
        public uint HeroBasicsDef;

        /// <summary>
        /// 基础暴击率
        /// </summary>
        public uint HeroBasicsCri;

        /// <summary>
        /// 元素
        /// </summary>
        public uint Element;


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
    /// Desc    unit_attribute配置文件访问接口
    /// </summary>
    public partial class UnitAttributeDao:BaseDao<UnitAttributeDao,uint,UnitAttributeConfig>
    {
        public override string GetName()
        {
            return "unit_attribute";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.19
    /// Desc    unit_attribute配置文件解码器
    /// </summary>
    public partial class UnitAttributeDecoder : BaseCfgDecoder<UnitAttributeConfig, UnitAttributeCfgData>
    {
        public override string GetName()
        {
            return "unit_attribute";
        }

        protected override void ProcessRow(UnitAttributeConfig excel)
        {
            GetU32("id", out excel.Id);
            GetU32("hero_lv", out excel.HeroLv);
            GetU32("hero_exp", out excel.HeroExp);
            GetU32("hero_basics_hp", out excel.HeroBasicsHp);
            GetU32("hero_basics_atk", out excel.HeroBasicsAtk);
            GetU32("hero_basics_def", out excel.HeroBasicsDef);
            GetU32("hero_basics_cri", out excel.HeroBasicsCri);
            GetU32("element", out excel.Element);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}