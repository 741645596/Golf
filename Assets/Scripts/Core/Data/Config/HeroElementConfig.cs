using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    hero_element配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class HeroElementConfig : IConfig<uint>
    {
        /// <summary>
        /// 表字段唯一id
        /// </summary>
        public uint Id;
        
        /// <summary>
        /// 英雄元素
        /// 1火
        /// 2水
        /// 3土
        /// 4光
        /// 5暗
        /// </summary>
        public uint ElementType;
        
        /// <summary>
        /// 英雄职业
        /// 1步兵
        /// 2骑兵
        /// 3弓兵
        /// </summary>
        public uint HeroProfession;
        
        /// <summary>
        /// 英雄职业和元素图片索引路径
        /// </summary>
        public uint HeroElementIconPath;
        
        /// <summary>
        /// 英雄职业和元素图片TIPS中的TID
        /// </summary>
        public string HeroElementTid;
        
        
        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion
    
    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    hero_element配置文件访问接口
    /// </summary>
    public partial class HeroElementDao: BaseDao<HeroElementDao, uint, HeroElementConfig>
    {
        public override string GetName()
        {
            return "hero_element";
        }
        
        protected override void OnChangeLang(ref HeroElementConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                HeroElementConfig cfg = cfgs[i];
                //tDao.TryGetText(cfg.HeroElementIconPath.ToString(), language, ref cfg.HeroElementIconPath);
                tDao.TryGetText(cfg.HeroElementTid, language, ref cfg.HeroElementTid);
            }
        }
    }
    #endregion
    
    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    hero_element配置文件解码器
    /// </summary>
    public partial class HeroElementDecoder : BaseCfgDecoder<HeroElementConfig, HeroElementCfgData>
    {
        public override string GetName()
        {
            return "hero_element";
        }
        
        protected override void ProcessRow(HeroElementConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetU32("element_type", out excel.ElementType);
            GetU32("hero_profession", out excel.HeroProfession);
            GetU32("$hero_element_icon_path", out excel.HeroElementIconPath);
            GetString("$hero_element_tid", out excel.HeroElementTid);
            ProcessRowExt(excel);
        }
    }
    
#endif
    #endregion
}