using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class StageWavesDao
    {
        private Dictionary<uint, Dictionary<uint, List<StageWavesConfig>>> dicStageWave =
            new Dictionary<uint, Dictionary<uint, List<StageWavesConfig>>>();

        public Dictionary<uint, Dictionary<uint, List<StageWavesConfig>>> GetStageCfg()
        {
            return dicStageWave;
        }

        public Dictionary<uint, List<StageWavesConfig>> GetWaveCfg(uint nId)
        {
            if (dicStageWave.ContainsKey(nId))
            {
                return dicStageWave[nId];
            }
            return null;
        }

        protected override void ProcessCfgsAfter(ref StageWavesConfig[] cfgs)
        {
            int count = cfgs.Length;
            StageWavesConfig stageWaveCfg = null;
            for (int i = 0; i < count; i++)
            {
                stageWaveCfg = cfgs[i];
                if (dicStageWave.ContainsKey(stageWaveCfg.Id))
                {
                    if (dicStageWave[stageWaveCfg.Id].ContainsKey(stageWaveCfg.Order))
                    {
                        dicStageWave[stageWaveCfg.Id][stageWaveCfg.Order].Add(stageWaveCfg);
                    }
                    else
                    {
                        var listStage = new List<StageWavesConfig>();
                        listStage.Add(stageWaveCfg);
                        dicStageWave[stageWaveCfg.Id][stageWaveCfg.Order] = listStage;
                    }
                }
                else
                {
                    var dicOrder = new Dictionary<uint, List<StageWavesConfig>>();
                    var listStage = new List<StageWavesConfig>();
                    listStage.Add(stageWaveCfg);
                    dicOrder[stageWaveCfg.Order] = listStage;
                    dicStageWave[stageWaveCfg.Id] = dicOrder;
                }
            }
        }
    }
    #endregion

    #region 配置定义扩展
    public partial class StageWavesConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class StageWavesDecoder
    {
        private void ProcessRowExt(StageWavesConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<StageWavesConfig> datas)
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