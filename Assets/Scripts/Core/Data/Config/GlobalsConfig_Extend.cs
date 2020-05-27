using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class GlobalsDao
    {
        //默认队伍1数据 ~临时用
        /// <summary>
        ///  int => 1个队伍中 英雄编号 ，int => heroid
        /// </summary>
        private string m_monsterDropEffect = "";
        private string m_bossDropEffect = "";

        private Dictionary<int, uint> m_defaultTeam = new Dictionary<int, uint>();
        private List<float> m_chessFlyList = new List<float>();
        public Dictionary<int, uint> GetDefaultTeam() {
            return m_defaultTeam;
        }

        public float GetChessFlyScale(int index) 
        {
            if (index >= m_chessFlyList.Count || index < 0) 
            {
                return 1.0f;
            }
            return m_chessFlyList[(int)index];
        }

        public List<float> GetChessFlyList() 
        {
            return m_chessFlyList;
        }

        public string GetMonsterDropEffect()
        {
            return m_monsterDropEffect;
        }

        public string GetBossDropEffect()
        {
            return m_bossDropEffect;
        }

        protected override void ProcessCfgsAfter(ref GlobalsConfig[] cfgs) {
            int count = cfgs.Length;
            GlobalsConfig globalInfo = null;

            for (int i = 0; i < count; i++) {
                globalInfo = cfgs[i];
                if ("default_team" == globalInfo.Name) {

                   string[] sArray = globalInfo.Value.Split('*');
                   for (int nTeamHero = 0; nTeamHero < sArray.Length; ++nTeamHero) {
                        m_defaultTeam[nTeamHero] = uint.Parse(sArray[nTeamHero]);
                    }
                }
                else if ("monster_drop_effect" == globalInfo.Name)
                {
                    m_monsterDropEffect = globalInfo.Value;
                }
                else if ("boss_drop_effect" == globalInfo.Name)
                {
                    m_bossDropEffect = globalInfo.Value;
                }
                else if ("chessfly" == globalInfo.Name) {
                    string[] sArray = globalInfo.Value.Split('*');                    
                    foreach (string str in sArray) 
                    {
                        float value = float.Parse(str);
                        m_chessFlyList.Add(value);
                    }   
                }

            }
        }
    }
    #endregion

    #region 配置定义扩展
    public partial class GlobalsConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class GlobalsDecoder
    {
        private void ProcessRowExt(GlobalsConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<GlobalsConfig> datas)
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