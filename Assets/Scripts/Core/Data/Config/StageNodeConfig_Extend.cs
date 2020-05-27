using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class StageNodeDao
    {
        private Dictionary<uint,  Dictionary<uint, List<StageNodeConfig>> > dictData = 
            new Dictionary<uint, Dictionary<uint, List<StageNodeConfig>>>();

        private List<StageNodeConfig> list = new List<StageNodeConfig>();

        public StageNodeConfig GetBattleNodeInfo(int chapterID, int battleID)
        {
            StageNodeConfig[] cfgs = this.GetCfgs();
            foreach(StageNodeConfig cfg in cfgs)
            {
                if (cfg.ChapterId == chapterID && cfg.NodeId == battleID)
                {
                    return cfg;
                }
            }

            return null;
        }

        protected override void ProcessCfgsAfter(ref StageNodeConfig[] cfgs)
        {
            foreach (StageNodeConfig cfg in cfgs)
            {
                if (dictData.ContainsKey(cfg.ChapterId))
                {
                    if (dictData[cfg.ChapterId].ContainsKey(cfg.NodeType))
                    {
                        dictData[cfg.ChapterId][cfg.NodeType].Add(cfg);
                    }
                    else
                    {
                        var list = new List<StageNodeConfig>();
                        list.Add(cfg);
                        dictData[cfg.ChapterId][cfg.NodeType] = list;
                    }
                }
                else
                {
                    var dict = new Dictionary<uint, List<StageNodeConfig> > ();

                    var list = new List<StageNodeConfig>();
                    list.Add(cfg);
                    dict[cfg.NodeType] = list;
                    dictData[cfg.ChapterId] = dict;
                }
            }
        }

        // 根据章节id 战斗节点类型 获取章节对应所有数据
        public List<StageNodeConfig> getStageInfoList(uint nChapterId, uint nNodeType)
        {
            if (dictData.ContainsKey(nChapterId) && dictData[nChapterId].ContainsKey(nNodeType)) 
            {
                return dictData[nChapterId][nNodeType];
            }

            return null;
        }
    }
    #endregion

    #region 配置定义扩展
    public partial class StageNodeConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class StageNodeDecoder
    {
        private void ProcessRowExt(StageNodeConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<StageNodeConfig> datas)
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