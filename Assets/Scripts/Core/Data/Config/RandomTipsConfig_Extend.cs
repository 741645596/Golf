using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展

    public partial class RandomTipsDao
    {
        private List<uint> m_idList = new List<uint>();
        private List<uint> m_typeTips = new List<uint>();
        Dictionary<uint, uint> map = new Dictionary<uint, uint>();

        protected override void ProcessCfgsAfter(ref RandomTipsConfig[] cfgs)
        {
            foreach (RandomTipsConfig cfg in cfgs)
            {
                m_idList.Add(cfg.Id);
                map[cfg.Id] = cfg.Type;
            }
        }

        public Dictionary<uint, uint> GetDic()
        {
            return map;
        }

        public List<uint> GetTipsByCasterLv(uint type)
        {
            Dictionary<uint, uint> dic = GetDic();

            if (null != dic)
            {
                m_typeTips.Clear();
                int lenth = m_idList.Count - 1;
                for (int i = 0; i <= lenth; i++)
                {
                    uint id = m_idList[i];
                    RandomTipsConfig cfgTip = RandomTipsDao.Inst.GetCfg(id);
                    if (null == cfgTip)
                    {
                        continue;
                    }
                    if (cfgTip.Type == type)
                    {
                        m_typeTips.Add(cfgTip.Id);
                    }
                }
            }

            return m_typeTips;
        }
    }

    #endregion

    #region 配置定义扩展

    public partial class RandomTipsConfig
    {
    }

    #endregion

    #region 配置解码器扩展

#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class RandomTipsDecoder
    {
        private void ProcessRowExt(RandomTipsConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<RandomTipsConfig> datas)
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