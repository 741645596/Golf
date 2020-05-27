using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.1.28
    /// Desc    text配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class TextConfig : IConfig<string>
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Tid;

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text;


        public string GetKey()
        {
            return Tid;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.1.28
    /// Desc    text配置文件访问接口
    /// </summary>
    public partial class TextDao:BaseDao<TextDao,string,TextConfig>
    {
        public override string GetName()
        {
            return "text";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.1.28
    /// Desc    text配置文件解码器
    /// </summary>
    public partial class TextDecoder : BaseCfgDecoder<TextConfig, TextCfgData>
    {
        public override string GetName()
        {
            return "text";
        }

        protected override void ProcessRow(TextConfig excel)
        {
            GetString("#tid", out excel.Tid);
            GetString("text", out excel.Text);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}