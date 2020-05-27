using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class HeroStarDao
    {
        private Dictionary<uint, Dictionary<uint, HeroStarConfig>> m_dicStarCfg =
            new Dictionary<uint, Dictionary<uint, HeroStarConfig>>();
            
        public HeroStarConfig GeHeroStarCfg(uint heroID, uint starlv)
        {
            if (m_dicStarCfg.ContainsKey(heroID))
            {
                if (m_dicStarCfg[heroID].ContainsKey(starlv))
                {
                    return m_dicStarCfg[heroID][starlv];
                }
            }
            
            return null;
        }
        public int GetStarLvMax(uint heroID)
        {
            if (m_dicStarCfg.ContainsKey(heroID))
            {
                return m_dicStarCfg[heroID].Count;
            }
            return 0;
        }
        protected override void ProcessCfgsAfter(ref HeroStarConfig[] cfgs)
        {
            int count = cfgs.Length;
            HeroStarConfig starCfg = null;
            for (int i = 0; i < count; i++)
            {
                starCfg = cfgs[i];
                Dictionary<uint, HeroStarConfig> levelCfgs;
                if (!m_dicStarCfg.TryGetValue(starCfg.HeroId, out levelCfgs))
                {
                    levelCfgs = new Dictionary<uint, HeroStarConfig>();
                    levelCfgs[starCfg.StarLv] = starCfg;
                }

                m_dicStarCfg[starCfg.HeroId] = levelCfgs;
            }
        }
    }
    #endregion
    
    #region 配置定义扩展
    public partial class HeroStarConfig
    {
    }
    #endregion
    
    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class HeroStarDecoder
    {
        private void ProcessRowExt(HeroStarConfig excel)
        {
            //在这里对配置的解析进行扩展
        }
        
        protected override void AfterProcess(List<HeroStarConfig> datas)
        {
            //整张表处理完成后执行，用于对表的后期处理，如排序
        }
        
        public override void AllDecodeAfterProcess()
        {
            //全部配置处理完后执行，用于处理有全局关系的值，可以在生成配置期间进行预处理
        }
    }
#endif
    #endregion
}