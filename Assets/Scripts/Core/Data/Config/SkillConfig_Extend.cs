using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展

    public partial class SkillDao
    {
        private Dictionary<uint, Dictionary<uint, SkillConfig>> m_dicSkillCfg =
            new Dictionary<uint, Dictionary<uint, SkillConfig>>();

        public SkillConfig GetSkillCfg(uint skillID, uint level)
        {
            if (m_dicSkillCfg.ContainsKey(skillID))
            {
                if (m_dicSkillCfg[skillID].ContainsKey(level))
                {
                    return m_dicSkillCfg[skillID][level];
                }
            }

            return null;
        }
        public int GetSkillLvMax(uint heroID)
        {
            if (m_dicSkillCfg.ContainsKey(heroID))
            {
                return m_dicSkillCfg[heroID].Count;
            }
            return 0;
        }
        protected override void ProcessCfgsAfter(ref SkillConfig[] cfgs)
        {
            int count = cfgs.Length;
            SkillConfig skillCfg = null;
            for (int i = 0; i < count; i++)
            {
                skillCfg = cfgs[i];
                Dictionary<uint, SkillConfig> levelCfgs;
                if (!m_dicSkillCfg.TryGetValue(skillCfg.Id, out levelCfgs))
                {
                    levelCfgs = new Dictionary<uint, SkillConfig>();
                    m_dicSkillCfg[skillCfg.Id] = levelCfgs;
                }

                levelCfgs[skillCfg.Lv] = skillCfg;
            }
        }
    }

    #endregion

    #region 配置定义扩展
    public partial class SkillConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class SkillDecoder
    {
        private void ProcessRowExt(SkillConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<SkillConfig> datas)
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