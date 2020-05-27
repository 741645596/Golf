using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    scene_perform配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class ScenePerformConfig : IConfig<uint>
    {
        /// <summary>
        /// 表演id
        /// </summary>
        public uint PerformId;

        /// <summary>
        /// 预制名
        /// </summary>
        public string PerformPrefab;


        public uint GetKey()
        {
            return PerformId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    scene_perform配置文件访问接口
    /// </summary>
    public partial class ScenePerformDao:BaseDao<ScenePerformDao,uint,ScenePerformConfig>
    {
        public override string GetName()
        {
            return "scene_perform";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    scene_perform配置文件解码器
    /// </summary>
    public partial class ScenePerformDecoder : BaseCfgDecoder<ScenePerformConfig, ScenePerformCfgData>
    {
        public override string GetName()
        {
            return "scene_perform";
        }

        protected override void ProcessRow(ScenePerformConfig excel)
        {
            GetU32("#perform_id", out excel.PerformId);
            GetString("perform_prefab", out excel.PerformPrefab);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}