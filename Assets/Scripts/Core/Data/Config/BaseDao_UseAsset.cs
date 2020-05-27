#if !ServerClient && !UNITY_EDITOR && !USE_CSV_CONFIG
using UnityEngine;

namespace IGG.Core.Data.Config
{
    /// <summary>
    /// 用户客户端读配置
    /// 配置使用asets格式
    /// </summary>
    /// <typeparam name="TDao"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public partial class BaseDao<TDao, TKey, TConfig>
    {
        private TConfig[] LoadCfg()
        {
            //BaseCfgData<TConfig> cfgData = Resources.Load<BaseCfgData<TConfig>>("Config/" + GetName());
            BaseCfgData<TConfig> cfgData = ResourceManger.LoadConfig(GetName()) as BaseCfgData<TConfig>;
            if (cfgData == null) {
                return null;
            }
            return cfgData.Data;
        }
    }
}
#endif