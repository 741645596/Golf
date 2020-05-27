#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

using System;
using System.Collections.Generic;
using IGG.Core;
using IGG.Core.Helper;
using IGG.Core.Data.Config;
using IGG.Core.Geom;
using Logger = IGG.Logging.Logger;
using ILogger = IGG.Logging.ILogger;

namespace IGG.Core.Data.Config
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.13
    /// Desc    配置文件解码器
    /// </summary>
    /// <typeparam name="TConfig">配置文件映身的数据类型</typeparam>
    /// <typeparam name="TConfigData">配置文件序列化对象类型</typeparam>
    public abstract class BaseCfgDecoder<TConfig, TConfigData> : Disposer, ICfgDecoder
        where TConfig : new()
        where TConfigData : BaseCfgData<TConfig>, new()
    {
        protected readonly ILogger m_logger;
        protected IRowReader m_row;
        protected uint m_curRowIndex;
        protected string m_curColName;
        private TConfigData m_data;

        public BaseCfgDecoder()
        {
            m_logger = Logger.Instance;
        }

        public bool Decode(ICfgReader excel)
        {
            if (excel == null || excel.RowCount == 0 || excel.ColCount == 0)
            {
                m_curRowIndex = 0;
                Log("配置文件为空", LogLevel.Error, true);
                return false;
            }

            List<TConfig> datas = new List<TConfig>((int) excel.RowCount);
            for (uint i = 0; i < excel.RowCount; i++)
            {
                m_curRowIndex = i;
                IRowReader rowReader = excel.GetRow(i);
                if (rowReader == null)
                {
                    Log("行数据为空", LogLevel.Error, true);
                    continue;
                }
                
                ProcessSource(rowReader, datas);
            }

            m_curRowIndex = 0;
            AfterProcess(datas);
            m_row = null;

#if ServerClient
            m_data = new TConfigData();
#else
            m_data = UnityEngine.ScriptableObject.CreateInstance<TConfigData>();
#endif
            m_data.Data = datas.ToArray();
            return true;
        }

        public object Data
        {
            get { return m_data; }
        }

        /// <summary>
        /// 得到配置文件的文件名
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// 得到保存的文件名
        /// </summary>
        /// <returns></returns>
        public virtual string GetSaveName()
        {
            return GetName() + ".asset";
        }

        /// <summary>
        /// 这个decoder是否可用
        /// 不可用的在编码时会忽略
        /// </summary>
        public virtual bool Enable
        {
            get { return true; }
        }

        /// <summary>
        /// 处理源数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="list"></param>
        protected virtual void ProcessSource(IRowReader row, List<TConfig> list)
        {
            m_row = row;
            TConfig excel = new TConfig();
            try
            {
                ProcessRow(excel);
                list.Add(excel);
            }
            catch (Exception e)
            {
                Log("解析静态配置出错:" + e, LogLevel.Error, true);
            }
        }

        /// <summary>
        /// 处理一行
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        protected abstract void ProcessRow(TConfig excel);

        /// <summary>
        /// 处理完表之后
        /// 用于处理这个表之间的关联数据 
        /// </summary>
        protected abstract void AfterProcess(List<TConfig> list);

        /// <summary>
        /// 全部配置全部加载完后处理
        /// 用处处理全部配置之间的关联信息
        /// </summary>
        public virtual void AllDecodeAfterProcess()
        {

        }

        /// <summary>
        /// 得到字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool GetString(string key, out string value)
        {
            m_curColName = key;
            if (m_row == null)
            {
                Log("GetString: m_row == null");
                value = null;
                return false;
            }

            value = m_row.Get(key);
            if (value == null)
            {
                Log("GetString: value == null");
                return false;
            }
            return true;
        }

        protected bool GetByte(string key, out Byte value)
        {
            string strValue;
            if (!GetString(key, out strValue))
            {
                value = 0;
                return false;
            }
            return StrHelper.GetByte(strValue, out value, m_logger);
        }

        protected bool GetU16(string key, out UInt16 value)
        {
            string strValue;
            if (!GetString(key, out strValue))
            {
                value = 0;
                return false;
            }

            return StrHelper.GetU16(strValue, out value, m_logger);
        }

        protected bool GetDouble(string key, out double value)
        {
            string strValue;
            if (!GetString(key, out strValue))
            {
                Log("GetDouble is empty", LogLevel.Warning);
                value = 0;
                return false;
            }

            if (!Double.TryParse(strValue, out value))
            {
                Log("GetDouble: Double.TryParse("+ strValue+") is error", LogLevel.Warning);
                return false;
            }
            return true;
        }

        protected bool GetU32(string key, out UInt32 value)
        {
            double dValue;
            if (GetDouble(key, out dValue))
            {
                value = (UInt32) dValue;
                return true;
            }
            value = 0;
            return false;
        }

        protected bool GetI32(string key, out Int32 value)
        {
            double dValue;
            if (GetDouble(key, out dValue))
            {
                value = (Int32) dValue;
                return true;
            }
            value = 0;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="per">默认值 1</param>
        /// <returns></returns>
        protected bool GetFloat(string key, out float value, float per)
        {
            double dValue;
            if (GetDouble(key, out dValue))
            {
                value = (float) dValue*per;
                return true;
            }
            value = 0;
            return false;
        }

        protected bool GetFloat(string key, out float value)
        {
            return GetFloat(key, out value, 1);
        }

        /// <summary>
        /// 得到布尔值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="trueStr">如果为真的字符串, 默认值"true"</param>
        /// <returns></returns>
        protected bool GetBool(string key, out bool value, string trueStr)
        {
            string strValue;
            if (!GetString(key, out strValue))
            {
                value = false;
                return false;
            }
            value = strValue == trueStr;
            return true;
        }

        protected bool GetBool(string key, out bool value)
        {
            return GetBool(key, out value, "true");
        }

        protected bool GetEnum<TEnum>(string key, out TEnum value)
        {
            string strValue;
            if (!GetString(key, out strValue))
            {
                value = default(TEnum);
                return false;
            }

            if (string.IsNullOrEmpty(strValue))
            {
                Log("GetEnum<TEnum>: value is empty");
                value = default(TEnum);
                return false;
            }

            return StrHelper.GetEnum(strValue, out value);
        }

        protected bool GetArr<TItem>(string key, string sprartor, out TItem[] value, Func<string, TItem> parse)
        {
            string strValue;
            if (!GetString(key, out strValue))
            {
                value = default(TItem[]);
                return false;
            }

            return StrHelper.GetArr(strValue, sprartor, out value, parse);
        }

        /// <summary>
        /// 得到bit位表示的枚举
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sprartor"></param>
        /// <param name="value"></param>
        /// <param name="isIndex">配置的值是否是索引,默认值true</param>
        /// <param name="offset">索引值偏移量, 默认值0</param>
        /// <returns></returns>
        protected bool GetBitEnum(string key, string sprartor, out int value, bool isIndex, int offset)
        {
            value = 0;
            int[] byteList;
            bool success = GetArr(key, sprartor, out byteList, ParseI32);
            if (!success)
            {
                return false;
            }

            if (isIndex)
            {
                for (int i = 0; i < byteList.Length; i++)
                {
                    value |= 1 << (byteList[i] + offset);
                }
            }
            else
            {
                for (int i = 0; i < byteList.Length; i++)
                {
                    value |= byteList[i];
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sprartor"></param>
        /// <param name="value"></param>
        /// <param name="isIndex">配置的值是否是索引,默认值true</param>
        /// <param name="offset">索引值偏移量, 默认值0</param>
        /// <returns></returns>
        protected bool GetBitEnumU8(string key, string sprartor, out byte value, bool isIndex, int offset)
        {
            int bitValue;
            GetBitEnum(key, sprartor, out bitValue, isIndex, offset);
            if (bitValue > byte.MaxValue)
            {
                Log("GetBitEnumU8: bitValue(" + bitValue + ") > byte.MaxValue(" + byte.MaxValue + ")");
                value = byte.MaxValue;
                return false;
            }
            else
            {
                value = (byte) bitValue;
                return true;
            }
        }

        /// <summary>
        /// 解析多元组
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="valueNum"></param>
        /// <param name="parse"></param>
        /// <param name="sprartor">默认值" "</param>
        /// <returns></returns>
        protected Func<string, TItem> ParseValueGroup<TItem>(uint valueNum, Action<TItem, int[]> parse, string sprartor)
            where TItem : new()
        {
            Func<string, TItem> p = s =>
            {
                string[] strArr = s.Split(new[] {sprartor}, StringSplitOptions.None);
                int[] intArr = new int[valueNum];

                for (int i = 0; i < strArr.Length; i++)
                {
                    StrHelper.GetI32(strArr[i], out intArr[i]);
                }

                TItem item = new TItem();
                parse(item, intArr);
                return item;
            };
            return p;
        }

        /// <summary>
        /// 解析百分比
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected float ParsePer100(string value)
        {
            UInt32 v2 = ParseU32(value);
            return v2*0.01f;
        }

        /// <summary>
        /// 解析千分比
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected float ParsePer1000(string value)
        {
            UInt32 v2 = ParseU32(value);
            return v2*0.001f;
        }

        /// <summary>
        /// 解析万分比
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected float ParsePer10000(string value)
        {
            UInt32 v2 = ParseU32(value);
            return v2*0.0001f;
        }

        protected UInt32 ParseU32(string value)
        {
            UInt32 newValue;
            if (!UInt32.TryParse(value, out newValue))
            {
                Log("UInt32.TryParse(" + value + ") error");
            }
            return newValue;
        }

        protected Int32 ParseI32(string value)
        {
            Int32 newValue;
            if (!Int32.TryParse(value, out newValue))
            {
                Log("Int32.TryParse(" + value + ") error");
            }
            return newValue;
        }

        protected byte ParseU8(string value)
        {
            byte newValue;
            byte.TryParse(value, out newValue);
            return newValue;
        }

        protected string ParseStr(string value)
        {
            return value;
        }

        protected TEnum ParseEnum<TEnum>(string value)
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value);
        }

        /// <summary>
        /// 主要用在ParseArr
        /// 支持string, enum, number的解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected T ParseAny<T>(string value)
        {
            Type tType = typeof(T);
            if (tType.IsEnum)
            {
                return ParseEnum<T>(value);
            }

            if (tType == typeof(string))
            {
                return (T) (object)value;
            }

            double doubleValue;
            if (!double.TryParse(value, out doubleValue))
            {
                return default(T);
            }

            if (typeof(T) == typeof(double))
            {
                return (T) (object) doubleValue;
            }

            try
            {
                object o = Convert.ChangeType(doubleValue, typeof(T));
                return (T) o;
            }
            catch (Exception e)
            {
                Log(e.ToString());
                return default(T);
            }
        }

        protected TArr ParseArr<TArr, TItem>(string value) where TArr : SerializableArr<TItem>, new()
        {
            TArr reslut = new TArr();
            reslut.Data = ParseArr<TItem>(value);
            return reslut;
        }

        protected SerializableArr<T> ParseSerializableArr<T>(string value)
        {
            var result = new SerializableArr<T>();
            if (!StrHelper.GetArr(value, StrHelper.ArrSplitLv2, out result.Data, ParseAny<T>))
            {
                result.Data = new T[0];
            }
            return result;
        }

        protected T[] ParseArr<T>(string value)
        {
            T[] item;
            if (StrHelper.GetArr(value, StrHelper.ArrSplitLv2, out item, ParseAny<T>))
            {
                return item;
            }
            return new T[0];
        }

        protected int Parse(string str)
        {
            int value;
            bool success = int.TryParse(str, out value);
            if (!success)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    Log("Parse: 数值转换失败: " + str);
                }
            }
            return value;
        }

        protected float Parsef(string str)
        {
            float value;
            bool success = float.TryParse(str, out value);
            if (!success)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    Log("Parsef: 数值转换失败: " + str);
                }
            }
            return value;
        }

        public static Int2 ParseInt2(string value)
        {
            string[] posArr = value.Split(new[] { StrHelper.ArrSplitLv2 }, StringSplitOptions.None);
            Int2 ret = new Int2();
            if (posArr.Length == 2)
            {
                int.TryParse(posArr[0], out ret.x);
                int.TryParse(posArr[1], out ret.y);
            }
            return ret;
        }

        public static Int3 ParseInt3(string value)
        {
            string[] posArr = value.Split(new[] { StrHelper.ArrSplitLv2 }, StringSplitOptions.None);
            Int3 ret = new Int3();
            if (posArr.Length == 3)
            {
                int.TryParse(posArr[0], out ret.x);
                int.TryParse(posArr[1], out ret.y);
                int.TryParse(posArr[2], out ret.z);
            }

            return ret;
        }

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="msg"></param>
        ///  <param name="level"></param>
        /// <param name="isGlobal">是否是全局LOG</param>
        protected void Log(string msg, LogLevel level = LogLevel.Error, bool isGlobal = false)
        {
            if (!isGlobal)
            {
                msg = string.Format("[第{0}行，列名:{1}] {2}", m_curRowIndex, m_curColName, msg);
            }

            msg = string.Format("配置错误({0}.csv) => {1}", GetName(), msg);
            switch (level)
            {
                case LogLevel.Warning:
                    Logger.LogWarning(msg, "BaseCfgDecoder.LogCfgError");
                    break;
                default:
                    Logger.LogError(msg, "BaseCfgDecoder.LogCfgError");
                    break;
            }
        }

        protected override void OnDispose()
        {
            m_row = null;
            m_data = null;
        }

        protected enum LogLevel
        {
            Warning,
            Error
        }
    }
}

#endif