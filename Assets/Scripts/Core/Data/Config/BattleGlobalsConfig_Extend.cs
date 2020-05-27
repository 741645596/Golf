using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class BattleGlobalsDao
    {
        // 基础暴击倍率
        private int m_attackCriteBase = 0;
        // 浮动的最低伤害系数
        private int m_minDamageRatio = 0;
        // 浮动的最高伤害系数
        private int m_maxDamageRatio = 0;
        // 获得的怒气值配置在global表
        private int m_angrayValue = 0;
        private int m_missAngrayPecent = 0;
        private int m_defendHitAngray = 0;
        private int m_defendEndAngray = 0;
        // 幂指数
        private double m_powerRate = 0;        
        // 克制
        private double m_likeRate = 0;
        // 被克制
        private double m_hateRate = 0;
        // combo 系数
        private Dictionary<int, double> m_comboDic = new Dictionary<int, double>();
        private double m_maxCombo = 0;
        // 
        private int m_addGodValue = 0;
        private int m_maxGodValue = 0;

        public int GetAttackCriteBase()
        {
            return m_attackCriteBase;
        }

        public int GetAddGodValue()
        {
            return m_addGodValue;
        }

        public int GetMaxGodValue()
        {
            return m_maxGodValue;
        }

        public int GetMinDamageRatio()
        {
            return m_minDamageRatio;
        }

        public int GetMaxDamageRatio()
        {
            return m_maxDamageRatio;
        }

        public int GetAngryValue()
        {
            return m_angrayValue;
        }

        public int GetMissAngryPecent()
        {
            return m_missAngrayPecent;
        }

        public int GetDefenderHitAngry()
        {
            return m_defendHitAngray;
        }

        public int GetDefenderEndAngry()
        {
            return m_defendEndAngray;
        }

        public double GetPowerRate()
        {
            return m_powerRate;
        }

        public double GetLikeRate()
        {
            return m_likeRate;
        }

        public double GetHateRate()
        {
            return m_hateRate;
        }

        public double GetComboRate(int combo)
        {
            if (combo == 0) 
            {
                return 0;
            }

            if (m_comboDic.ContainsKey(combo)) 
            {
                return m_comboDic[combo];
            }

            return m_maxCombo;
        }


        protected override void ProcessCfgsAfter(ref BattleGlobalsConfig[] cfgs)
        {
            int count = cfgs.Length;
            BattleGlobalsConfig globalInfo = null;

            for (int i = 0; i < count; i++)
            {
                globalInfo = cfgs[i];
                if ("attack_critrate" == globalInfo.Name)
                {
                    m_attackCriteBase = int.Parse(globalInfo.Value);
                }
                else if ("randmin" == globalInfo.Name)
                {
                    m_minDamageRatio = int.Parse(globalInfo.Value);
                }
                else if ("randmax" == globalInfo.Name)
                {
                    m_maxDamageRatio = int.Parse(globalInfo.Value);
                }
                else if ("angry_value" == globalInfo.Name)
                {
                    m_angrayValue = int.Parse(globalInfo.Value);
                }                                               
                else if ("miss_angry_percent" == globalInfo.Name)
                {
                    m_missAngrayPecent = int.Parse(globalInfo.Value);
                }
                else if ("def_hit_angry" == globalInfo.Name)
                {
                    m_defendHitAngray = int.Parse(globalInfo.Value);
                }
                else if ("def_end_angry" == globalInfo.Name)
                {
                    m_defendEndAngray = int.Parse(globalInfo.Value);
                }
                else if ("power_rate" == globalInfo.Name)
                {
                    m_powerRate = double.Parse(globalInfo.Value);
                }                
                else if ("like_rate" == globalInfo.Name)
                {
                    m_likeRate = double.Parse(globalInfo.Value);
                }
                else if ("hate_rate" == globalInfo.Name)
                {
                    m_hateRate = double.Parse(globalInfo.Value);
                }
                else if ("god_value" == globalInfo.Name)
                {
                    m_addGodValue = int.Parse(globalInfo.Value);

                }
                else if ("god_max_value" == globalInfo.Name)
                {
                    m_maxGodValue = int.Parse(globalInfo.Value);
                }
                else if ("combo" == globalInfo.Name)
                {
                    int maxKey = 0;                    
                    string [] sArray = globalInfo.Value.Split('|');
                    foreach (string str in sArray) 
                    {
                        string[] sArray2 = str.Split('*');
                        if (sArray2.Length >= 2) 
                        {
                            int key = int.Parse(sArray2[0]);
                            double value = double.Parse(sArray2[1]);
                            m_comboDic.Add(key, value);
                            if (key > maxKey) 
                            {
                                m_maxCombo = value;
                                maxKey = key;
                            }
                        }
                    }
                }

            }
        }
    }
    #endregion

    #region 配置定义扩展
    public partial class BattleGlobalsConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class BattleGlobalsDecoder
    {
        private void ProcessRowExt(BattleGlobalsConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<BattleGlobalsConfig> datas)
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