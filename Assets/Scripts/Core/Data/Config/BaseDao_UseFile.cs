#if ServerClient || UNITY_EDITOR
namespace IGG.Core.Data.Config
{
    /// <summary>
    /// 用的于服务器lib的读配置
    /// 读取配置文件
    /// </summary>
    /// <typeparam name="TDao"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public partial class BaseDao<TDao, TKey, TConfig>
    {
        private TConfig[] LoadCfg()
        {
            string path = ConfigMgr.CfgPath + "/" + GetName() + ".csv";
            Excel excel = new Excel(2);
            if (!excel.Load(path)) {
                return null;
            }
            
            ICfgDecoder decoder = ConfigMgr.GetDecoder(GetName());
            if (decoder == null) {
                m_logger.LogError(GetName() + "的Decoder没有找到!", "BaseDao_UseXML.LoadCfg");
                return null;
            }
            
            if (!decoder.Decode(excel)) {
                return null;
            }
            
            BaseCfgData<TConfig> data = decoder.Data as BaseCfgData<TConfig>;
            if (data == null) {
                m_logger.LogError("data == null, dao = " + this, "BaseDao_UseXML.LoadCfg");
                return null;
            }
            
            return data.Data;
        }
    }
}
#endif