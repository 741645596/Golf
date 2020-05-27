using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class HeroInfoDao
    {
        private List<uint> m_listhero = new List<uint>();
        private List<HeroInfoConfig> m_listHeroConfig = new List<HeroInfoConfig>();

        protected override void ProcessCfgsAfter(ref HeroInfoConfig[] cfgs)
        {
            int count = cfgs.Length;
            
            for (int i = 0; i < count; i++)
            {
                var heroConfig = cfgs[i];
                m_listhero.Add(heroConfig.Id);
                m_listHeroConfig.Add(heroConfig);
            }
        }
        public List<uint> GetHeroList()
        {
            return m_listhero;
        }

        public List<HeroInfoConfig> GetHeroConfigs()
        {
            return m_listHeroConfig;
        }
    }
    #endregion

    #region 配置定义扩展
    public partial class HeroInfoConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class HeroInfoDecoder
    {
        private void ProcessRowExt(HeroInfoConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<HeroInfoConfig> datas)
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