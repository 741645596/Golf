using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.5
    /// Desc    resource_preload配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class ResourcePreloadConfig : IConfig<uint>
    {
        /// <summary>
        /// 序号id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 文件夹路径
        /// </summary>
        public string FilePath;

        /// <summary>
        /// prefab名
        /// </summary>
        public string PrefabName;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks;

        /// <summary>
        /// 预加载数量
        /// </summary>
        public uint PreloadCounts;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.5
    /// Desc    resource_preload配置文件访问接口
    /// </summary>
    public partial class ResourcePreloadDao:BaseDao<ResourcePreloadDao,uint,ResourcePreloadConfig>
    {
        public override string GetName()
        {
            return "resource_preload";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.5
    /// Desc    resource_preload配置文件解码器
    /// </summary>
    public partial class ResourcePreloadDecoder : BaseCfgDecoder<ResourcePreloadConfig, ResourcePreloadCfgData>
    {
        public override string GetName()
        {
            return "resource_preload";
        }

        protected override void ProcessRow(ResourcePreloadConfig excel)
        {
            GetU32("id", out excel.Id);
            GetString("file_path", out excel.FilePath);
            GetString("prefab_name", out excel.PrefabName);
            GetString("remarks", out excel.Remarks);
            GetU32("preload_counts", out excel.PreloadCounts);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}