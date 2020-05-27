namespace IGG.Core.Data.Config
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.12
    /// Desc    配置文件序列化对象的基类
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
#if ServerClient
    public class BaseCfgData<TConfig>
#else
    public class BaseCfgData<TConfig>: UnityEngine.ScriptableObject
#endif
    {
        public TConfig[] Data;
    }
}