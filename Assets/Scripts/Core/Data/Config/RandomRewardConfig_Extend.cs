using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class RandomRewardDao
    {
        private List<RandomRewardConfig> listReward = new List<RandomRewardConfig>();
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<RandomRewardConfig> GetRandomReward(uint id)
        {
            List<RandomRewardConfig> l = new List<RandomRewardConfig>();
            int count = listReward.Count;
            for (int i = 0; i < count; i++) {
                if (listReward[i].Id == id) {
                    l.Add(listReward[i]);
                }
            }
            return l;
        }
        protected override void ProcessCfgsAfter(ref RandomRewardConfig[] cfgs)
        {
            int count = cfgs.Length;
            for (int i = 0; i < count; i++) {
                listReward.Add(cfgs[i]);
            }
        }
    }
    #endregion
    
    #region 配置定义扩展
    public partial class RandomRewardConfig
    {
    }
    #endregion
    
    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class RandomRewardDecoder
    {
        private void ProcessRowExt(RandomRewardConfig excel)
        {
            //在这里对配置的解析进行扩展
        }
        
        protected override void AfterProcess(List<RandomRewardConfig> datas)
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