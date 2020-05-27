using IGG.Core;
using System.Collections.Generic;
using ILogger = IGG.Logging.ILogger;
using Logger = IGG.Logging.Logger;

namespace IGG.Core.Data.Config
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.13
    /// Desc    配置文件访问对象基类
    /// </summary>
    /// <typeparam name="TDao">就是自己</typeparam>
    /// <typeparam name="TKey">key的类型</typeparam>
    /// <typeparam name="TConfig">配置类型</typeparam>
    public abstract partial class BaseDao<TDao, TKey, TConfig> : Singleton<TDao>, ILanguageOp
      where TDao : BaseDao<TDao, TKey, TConfig>, new()
      where TConfig : IConfig<TKey>
    {
        private Dictionary<TKey, TConfig> m_cfgMap;
        private TConfig[] m_cfgs;
        protected ILogger m_logger;
        private string m_curLang = "cn";

        public BaseDao()
        { 
            Init();
        }

        private void Init()
        {
            m_logger = Logger.Instance;
            TConfig[] confgs = LoadCfg();
            if (confgs == null)
            {
                m_logger.LogError("配置文件加载错误: " + GetName(), "OnCreate");
                return;
            }
            m_cfgs = ProcessData(confgs, m_cfgMap);
            ProcessCfgsAfter(ref m_cfgs);
            ConfigLanguage.UseCfgs.Add(this);
        }

        /// <summary>
        /// 眼据configs填充cfgDic
        /// 默认的数据处理方式
        /// </summary>
        /// <param name="confgs"></param>
        /// <param name="cfgDic"></param>
        protected virtual TConfig[] ProcessData(TConfig[] confgs, Dictionary<TKey, TConfig> cfgDic)
        {
            m_cfgMap = new Dictionary<TKey, TConfig>(confgs.Length);
            int len = confgs.Length;
            for (int i = 0; i < len; i++)
            {
                TConfig cfg = confgs[i];
                m_cfgMap[cfg.GetKey()] = cfg;
            }
            return confgs;
        }

        /// <summary>
        /// 二次处理cfgs
        /// 比如对cfgs排个序之类的
        /// </summary>
        /// <param name="cfgs"></param>
        protected virtual void ProcessCfgsAfter(ref TConfig[] cfgs)
        {

        }

        /// <summary>
        /// 处理语言切换
        /// </summary>
        /// <param name="cfgs"></param>
        /// <param name="language"></param>
        protected virtual void OnChangeLang(ref TConfig[] cfgs, string language)
        {
            
        }

        /// <summary>
        /// 得到配置的个数
        /// </summary>
        public int Count
        {
            get { return m_cfgs.Length; }
        }

        /// <summary>
        /// 得到配置文件的文件名
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// 得到全部的配置文件
        /// </summary>
        /// <returns></returns>
        public TConfig[] GetCfgs()
        {
            return m_cfgs;
        }

        /// <summary>
        /// 跟据key得到配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TConfig GetCfg(TKey key)
        {
            if (m_cfgMap == null)
            {
                return default(TConfig);
            }

            TConfig cfg;
            m_cfgMap.TryGetValue(key, out cfg);
            return cfg;
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="language"></param>
        public void SwitchLang(string language)
        {
            if (m_curLang == language)
            {
                return;
            }

            if (m_cfgs != null)
            {
                OnChangeLang(ref m_cfgs, language);
            }
            else
            {
                m_logger.LogError("m_cfgs == null, cfg = " + GetName(), "SwitchLang");
            }

            m_curLang = language;
        }

        protected override void OnGetInst()
        {
            // 先屏蔽
            //if (ConfigLanguage.Value != m_curLang)
            {
                SwitchLang(ConfigLanguage.Value);
            }
        }
    }

    public static class ConfigLanguage
    {
        public static string Value = "en";

        public static List<ILanguageOp> UseCfgs = new List<ILanguageOp>();
    }
}