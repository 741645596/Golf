using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class UnitAttributeDao
    {
        private Dictionary<uint, Dictionary<uint, UnitAttributeConfig>> m_dicUnitAttribute =
           new Dictionary<uint, Dictionary<uint, UnitAttributeConfig>>();

        public UnitAttributeConfig GetCfg(uint unitID, uint unitLev)
        {
            if (m_dicUnitAttribute.ContainsKey(unitID))
            {
                if (m_dicUnitAttribute[unitID].ContainsKey(unitLev))
                {
                    return m_dicUnitAttribute[unitID][unitLev];
                }
            }

            return null;
        }

        public int GetUnitLvMax(uint unitID)
        {
            if (m_dicUnitAttribute.ContainsKey(unitID))
            {
                return m_dicUnitAttribute[unitID].Count;
            }

            return 0;
        }

        protected override void ProcessCfgsAfter(ref UnitAttributeConfig[] cfgs)
        {
            int count = cfgs.Length;
            UnitAttributeConfig unitAttrCfg = null;
            for (int i = 0; i < count; i++)
            {

                unitAttrCfg = cfgs[i];

                Dictionary<uint, UnitAttributeConfig> levelCfgs;
                if (!m_dicUnitAttribute.TryGetValue(unitAttrCfg.Id, out levelCfgs))
                {
                    levelCfgs = new Dictionary<uint, UnitAttributeConfig>();
                    m_dicUnitAttribute[unitAttrCfg.Id] = levelCfgs;
                }

                levelCfgs[unitAttrCfg.HeroLv] = unitAttrCfg;
            }
        }
    }
    #endregion

    #region 配置定义扩展
    public partial class UnitAttributeConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class UnitAttributeDecoder
    {
        private void ProcessRowExt(UnitAttributeConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<UnitAttributeConfig> datas)
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