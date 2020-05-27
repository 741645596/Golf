using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class PassvityskillDao
    {
        private Dictionary<uint, Dictionary<uint, PassvityskillConfig>> m_dicPassvtySkillCfg =
            new Dictionary<uint, Dictionary<uint, PassvityskillConfig>>();
            
        public PassvityskillConfig GetPassvityskillCfg(uint skillId, uint skillLv)
        {
            if (m_dicPassvtySkillCfg.ContainsKey(skillId))
            {
                if (m_dicPassvtySkillCfg[skillId].ContainsKey(skillLv))
                {
                    return m_dicPassvtySkillCfg[skillId][skillLv];
                }
            }
            
            return null;
        }
        public int GetSkillLvMax(uint skillId)
        {
            if (m_dicPassvtySkillCfg.ContainsKey(skillId))
            {
                return m_dicPassvtySkillCfg[skillId].Count;
            }
            return 0;
        }
        protected override void ProcessCfgsAfter(ref PassvityskillConfig[] cfgs)
        {
            int count = cfgs.Length;
            PassvityskillConfig skillCfg = null;
            for (int i = 0; i < count; i++)
            {
                skillCfg = cfgs[i];
                Dictionary<uint, PassvityskillConfig> levelCfgs;
                if (!m_dicPassvtySkillCfg.TryGetValue(skillCfg.Id, out levelCfgs))
                {
                    levelCfgs = new Dictionary<uint, PassvityskillConfig>();
                    m_dicPassvtySkillCfg[skillCfg.Id] = levelCfgs;
                }
                
                levelCfgs[skillCfg.Id] = skillCfg;
            }
        }
    }
    #endregion
    
    #region 配置定义扩展
    public partial class PassvityskillConfig
    {
    }
    #endregion
    
    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class PassvityskillDecoder
    {
        private void ProcessRowExt(PassvityskillConfig excel)
        {
            //在这里对配置的解析进行扩展
        }
        
        protected override void AfterProcess(List<PassvityskillConfig> datas)
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