using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class PlayerLevelDao
    {
        private List<PlayerLevelConfig> m_listConfig = new List<PlayerLevelConfig>();
        
        protected override void ProcessCfgsAfter(ref PlayerLevelConfig[] cfgs)
        {
            int count = cfgs.Length;
            
            for (int i = 0; i < count; i++) {
                var heroConfig = cfgs[i];
                m_listConfig.Add(heroConfig);
            }
        }
        
        /// <summary>
        /// 根据经验获取等级
        /// </summary>
        /// <param name="totalLev"></param>
        /// <returns></returns>
        public int GetLev(int totalLev)
        {
            if (m_listConfig[0].Experience > (uint)totalLev) {
                return (int)m_listConfig[0].Lv;
            }
            
            for (int i = 0; i < m_listConfig.Count - 1; i++) {
                if (m_listConfig[i].Experience <= (uint)totalLev && m_listConfig[i + 1].Experience > (uint)totalLev) {
                    return (int)m_listConfig[i+1].Lv;
                }
            }
            return (int)m_listConfig[m_listConfig.Count - 1].Lv;
        }

        /// <summary>
        /// 当前等级 升级到下一级等级所需全部的经验值
        /// </summary>
        /// <param name="Lv"></param>
        /// <returns></returns>
        public uint GetExperience(int nLv) {
            if (nLv > 0 && nLv < m_listConfig.Count ) {
                return m_listConfig[nLv - 1].Experience;
            }
            return 0;
        }

        /// <summary>
        /// 玩家体力值
        /// </summary>
        /// <param name="nLv"></param>
        /// <returns></returns>
        public uint GetPowerMax(int nLv) {
            if (nLv > 0 && nLv < m_listConfig.Count) {
                return m_listConfig[nLv - 1].MaxPower;
            }
            return 0;
        }

        /// <summary>
        /// 铁矿上限
        /// </summary>
        /// <param name="nLv"></param>
        /// <returns></returns>
        public uint GetIronMax(int nLv) {
            if (nLv > 0 && nLv < m_listConfig.Count) {
                return m_listConfig[nLv - 1].MaxIron;
            }
            return 0;
        }

        /// <summary>
        /// 食物上限
        /// </summary>
        /// <param name="nLv"></param>
        /// <returns></returns>
        public uint GetFoodMax(int nLv) {
            if (nLv > 0 && nLv < m_listConfig.Count) {
                return m_listConfig[nLv - 1].MaxFood;
            }
            return 0;
        }

    }
    #endregion
    
    #region 配置定义扩展
    public partial class PlayerLevelConfig
    {
    }
    #endregion
    
    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class PlayerLevelDecoder
    {
        private void ProcessRowExt(PlayerLevelConfig excel)
        {
            //在这里对配置的解析进行扩展
        }
        
        protected override void AfterProcess(List<PlayerLevelConfig> datas)
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