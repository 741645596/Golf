using IGG.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IGG.Core.Data.Config
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.18
    /// Desc    服务器端的配置处理类
    /// </summary>
    public class ConfigMgr
    {
    
        private static string[] m_languageMap;
        
        static ConfigMgr()
        {
            //构建语言映射
            m_languageMap = new string[50];
            //m_languageMap[(int) MsgLanguageType.kLanguageEnglish] = "en";
            //m_languageMap[(int) MsgLanguageType.kLanguageChineseCN] = "cn";
            //m_languageMap[(int) MsgLanguageType.kLanguageChineseTW] = "tw";
            
#if UNITY_EDITOR
            CfgPath = UnityEngine.Application.dataPath + "/Config";
            //#elif UNITY_STANDALONE
            //CfgPath = UnityEngine.Application.streamingAssetsPath + "/Config";
#endif
            
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
            g_cfgDecoderMap = new Dictionary<string, ICfgDecoder>();
            RegistAll();
            RegistXml();
#endif
        }
        
        public static void LoadAll()
        {
        }
        
        /*public static void SetLanguage(MsgLanguageType type)
        {
            int index = (int) type;
            if (index < 0 ||
                index >= m_languageMap.Length ||
                m_languageMap[index] == null)
            {
                ConfigLanguage.Value = "en";
            }
            else
            {
                ConfigLanguage.Value = m_languageMap[index];
            }
        
            //因为有的配置文件有缓存，并不一定是从Inst.GetCfg处取，所以只能全刷一次
            for (int i = 0; i < ConfigLanguage.UseCfgs.Count; i++)
            {
                var cfg = ConfigLanguage.UseCfgs[i];
                cfg.SwitchLang(ConfigLanguage.Value);
            }
        }*/
        
        public static void Clear()
        {
        
        }
        
        
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
        
        public static string CfgPath;
        private static readonly Dictionary<string, ICfgDecoder> g_cfgDecoderMap;
        
        /// <summary>
        /// 取得解码器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ICfgDecoder GetDecoder(string name)
        {
            ICfgDecoder decoder;
            g_cfgDecoderMap.TryGetValue(name, out decoder);
            return decoder;
        }
        
        private static void RegistXml()
        {
        
        }
        
        private static void RegistAll()
        {
            Type[] cfgDecoderTypes = GetAllCfgDecoderTypes();
            foreach (Type decoderType in cfgDecoderTypes) {
                ICfgDecoder decoder = Activator.CreateInstance(decoderType) as ICfgDecoder;
                if (decoder == null) {
                    Logger.LogError("decoder == null, type = " + decoderType, "ConfigData.RegistAll");
                    continue;
                }
                g_cfgDecoderMap[decoder.GetName()] = decoder;
            }
        }
        
        private static Type[] GetAllCfgDecoderTypes()
        {
            Type cfgInterfaceType = typeof(ICfgDecoder);
            List<Type> decoderTypes = new List<Type>();
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblys) {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types) {
                    if (type.GetInterfaces().Contains(cfgInterfaceType)) {
                        if (!type.IsAbstract) {
                            decoderTypes.Add(type);
                        }
                    }
                }
            }
            return decoderTypes.ToArray();
        }
#endif
    }
}