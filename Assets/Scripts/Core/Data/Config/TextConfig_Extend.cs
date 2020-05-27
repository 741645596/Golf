using System.Collections.Generic;
using IGG.Core;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class TextDao
    {
        /// <summary>
        /// 得到字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public string GetText(string key, List<string> paramList)
        {
            if (paramList == null)
            {
                return GetText(key);
            }
            else
            {
                return GetText(key, paramList.ToArray());
            }
        }

        /// <summary>
        /// 得到字符串
        /// 可以使用GetText(kye, new []{xx,xx})的用法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public string GetText(string key, params string[] paramArr)
        {
            TextConfig cfg = GetCfg(key);
            if (cfg == null)
            {
                return null;
            }

            if (paramArr.Length == 0)
            {
                return cfg.Text;
            }
            else
            {
                return StrHelper.ReplaceString(cfg.Text, paramArr);
            }
        }

        public bool TryGetText(string key, string language, ref string value)
        {
            TextConfig strCfg = GetCfg(key);
            if (strCfg == null)
            {
                return false;
            }

            /*switch (language)
            {
                case "cn":
                    value = strCfg.Cn;
                    break;

                case "en":
                    value = strCfg.En;
                    break;

                case "tw":
                    value = strCfg.Tw;
                    break;
            }

            if (string.IsNullOrEmpty(value))
            {
                value = strCfg.Cn;
            }*/
            value = strCfg.Text;

            return true;
        }
    }
    #endregion

    #region 配置定义扩展
    public partial class TextConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class TextDecoder
    {
        private void ProcessRowExt(TextConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<TextConfig> datas)
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