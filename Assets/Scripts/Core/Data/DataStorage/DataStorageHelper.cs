using System;
using UnityEngine;
using Logger = IGG.Logging.Logger;

/// <summary>
/// 本地数据存储 特别注意：无法序列化dic字典
/// </summary>
namespace IGG.Data.DataStorage
{
    public class DataStorageHelper
    {
        // 所有的本地存储数据KEY都提供在这里
        public const string kSimServer = "SimServer";

        public static T LoadData<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key)) {
                return default(T);
            }
            
            try {
                T ret;
                if (typeof(T) == typeof(int) || typeof(T) == typeof(uint)) {
                    ret = (T)Convert.ChangeType(PlayerPrefs.GetInt(key), typeof(T));
                } else if (typeof(T) == typeof(float) || typeof(T) == typeof(double)) {
                    ret = (T)Convert.ChangeType(PlayerPrefs.GetFloat(key), typeof(T));
                } else if (typeof(T) == typeof(string)) {
                    ret = (T)Convert.ChangeType(PlayerPrefs.GetString(key), typeof(T));
                } else {
                    string txt = PlayerPrefs.GetString(key);
                    ret = JsonUtility.FromJson<T>(txt);
                    
                }
                
                return ret;
                
            } catch (Exception e) {
                Logger.LogWarning("读取配置错误, key = " + key + ", " + e);
            }
            
            return default(T);
        }
        
        public static void SaveData<T>(string key, T source)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(uint)) {
                PlayerPrefs.SetInt(key, (int)Convert.ChangeType(source, typeof(int)));
            } else if (typeof(T) == typeof(float) || typeof(T) == typeof(double)) {
                PlayerPrefs.SetFloat(key, (float)Convert.ChangeType(source, typeof(float)));
            } else if (typeof(T) == typeof(string)) {
                PlayerPrefs.SetString(key, (string)Convert.ChangeType(source, typeof(string)));
            } else {
                string txt = JsonUtility.ToJson(source);
                PlayerPrefs.SetString(key, txt);
            }
        }
        
        public static void ClearData(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
        
        public static void ClearAllData()
        {
            ClearData(kSimServer);
        }
        
    }
    
}