using System;
using System.Text;
using IGG.Logging;
using IGG.Core.Geom;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ILogger = IGG.Logging.ILogger;

namespace IGG.Core.Helper
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.13
    /// Desc    处理字符串的帮助类
    /// </summary>
    public static class StrHelper
    {
        /// <summary>
        /// 从字符串转值到u16
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <param name="logger">默认值为null</param>
        /// <returns></returns>
        public static bool GetU16(string valueIn, out UInt16 valueOut, ILogger logger)
        {
            if (string.IsNullOrEmpty(valueIn)) {
                valueOut = 0;
                return false;
            }
            
            bool success = UInt16.TryParse(valueIn, out valueOut);
            if (!success) {
                if (logger != null) {
                    logger.LogError("UInt16.TryParse error", "StrHelper.GetU16");
                }
                valueOut = 0;
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// 从字符串转值到u16
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <returns></returns>
        public static bool GetU16(string valueIn, out UInt16 valueOut)
        {
            return GetU16(valueIn, out valueOut, null);
        }
        
        /// <summary>
        /// 从字符串转值到u32
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <param name="logger">默认值为null</param>
        /// <returns></returns>
        public static bool GetU32(string valueIn, out UInt32 valueOut, ILogger logger)
        {
            if (string.IsNullOrEmpty(valueIn)) {
                valueOut = 0;
                return false;
            }
            
            bool success = UInt32.TryParse(valueIn, out valueOut);
            if (!success) {
                if (logger != null) {
                    logger.LogError("UInt32.TryParse error", "StrHelper.GetU32");
                }
                valueOut = 0;
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// 从字符串转值到u32
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <returns></returns>
        public static bool GetU32(string valueIn, out UInt32 valueOut)
        {
            return GetU32(valueIn, out valueOut, null);
        }
        
        /// <summary>
        /// 从字符串转值到i32
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <param name="logger">默认值为null</param>
        /// <returns></returns>
        public static bool GetI32(string valueIn, out Int32 valueOut, ILogger logger)
        {
            if (string.IsNullOrEmpty(valueIn)) {
                valueOut = 0;
                return false;
            }
            
            bool success = Int32.TryParse(valueIn, out valueOut);
            if (!success) {
                if (logger != null) {
                    logger.LogError("UInt32.TryParse error", "StrHelper.GetI32");
                }
                valueOut = 0;
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// 从字符串转值到i32
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <returns></returns>
        public static bool GetI32(string valueIn, out Int32 valueOut)
        {
            return GetI32(valueIn, out valueOut, null);
        }
        
        /// <summary>
        /// 从字符串转值到float
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <param name="per">默认值为1</param>
        /// <param name="logger">默认值为null</param>
        /// <returns></returns>
        public static bool GetFloat(string valueIn, out float valueOut, float per, ILogger logger)
        {
            if (string.IsNullOrEmpty(valueIn)) {
                valueOut = 0;
                return false;
            }
            
            bool success = float.TryParse(valueIn, out valueOut);
            if (!success) {
                if (logger != null) {
                    logger.LogError("float.TryParse error", "StrHelper.GetFloat");
                }
                valueOut = 0;
                return false;
            }
            valueOut = valueOut * per;
            return true;
        }
        
        /// <summary>
        /// 从字符串转值到float
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <returns></returns>
        public static bool GetFloat(string valueIn, out float valueOut)
        {
            return GetFloat(valueIn, out valueOut, 1, null);
        }
        
        /// <summary>
        /// 从字符串转值到Byte
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static bool GetByte(string valueIn, out Byte valueOut, ILogger logger)
        {
            if (string.IsNullOrEmpty(valueIn)) {
                valueOut = 0;
                return false;
            }
            
            bool success = Byte.TryParse(valueIn, out valueOut);
            if (!success) {
                if (logger != null) {
                    logger.LogError("Byte.TryParse error", "StrHelper.GetByte");
                }
                valueOut = 0;
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// 从字符串转值到Byte
        /// </summary>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <returns></returns>
        public static bool GetByte(string valueIn, out Byte valueOut)
        {
            return GetByte(valueIn, out valueOut, null);
        }
        
        /// <summary>
        /// 从字符串转值到enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="valueIn"></param>
        /// <param name="valueOut"></param>
        /// <returns></returns>
        public static bool GetEnum<TEnum>(string valueIn, out TEnum valueOut)
        {
            valueOut = (TEnum)Enum.Parse(typeof(TEnum), valueIn);
            return true;
        }
        
        #region 数组分隔符
        
        public const string ArrSplitLv1 = "|";
        public const string ArrSplitLv2 = "*";
        public const string ArrSplitLv3 = "$";
        public const string ArrSplitLv4 = "#";
        
        #endregion
        
        
        /// <summary>
        /// 通过一定规则，把字符串转为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueIn"></param>
        /// <param name="sprartor"></param>
        /// <param name="valueOut"></param>
        /// <param name="parse">解析器</param>
        /// <returns></returns>
        public static bool GetArr<T>(string valueIn, string sprartor, out T[] valueOut, Func<string, T> parse)
        {
            if (string.IsNullOrEmpty(valueIn)) {
                valueOut = new T[0];
                return true;
            }
            
            string[] strValueArr = valueIn.Split(new[] { sprartor }, StringSplitOptions.RemoveEmptyEntries);
            
            Type tType = typeof(T);
            if (typeof(string) == tType) {
                valueOut = strValueArr as T[];
                return true;
            }
            
            if (parse == null) {
                throw new NullReferenceException("如果项非string类型，则必需设置解析器");
            }
            
            valueOut = new T[strValueArr.Length];
            for (int i = 0; i < strValueArr.Length; i++) {
                valueOut[i] = parse(strValueArr[i]);
            }
            return true;
        }
        
        /// <summary>
        /// 通过一定规则，把字答串数组转为对应类型数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="sprartor"></param>
        /// <param name="parse"></param>
        /// <returns></returns>
        public static T[] GetArr<T>(string[] values, string sprartor, Func<string, T> parse)
        {
            T[] valueOut = new T[values.Length];
            for (int i = 0; i < values.Length; i++) {
                valueOut[i] = parse(values[i]);
            }
            return valueOut;
        }
        
        
        public static UInt32 ParseU32(string value)
        {
            UInt32 newValue;
            UInt32.TryParse(value, out newValue);
            return newValue;
        }
        
        public static Int32 ParseInt32(string value)
        {
            Int32 newValue;
            Int32.TryParse(value, out newValue);
            return newValue;
        }
        
        public static Int2 ParseInt2(string value)
        {
            string[] posArr = value.Split(new[] {ArrSplitLv2}, StringSplitOptions.None);
            Int2 ret = new Int2();
            if (posArr.Length == 2) {
                int.TryParse(posArr[0], out ret.x);
                int.TryParse(posArr[1], out ret.y);
            }
            return ret;
        }
        
        public static Int3 ParseInt3(string value)
        {
            string[] posArr = value.Split(new[] { StrHelper.ArrSplitLv2 }, StringSplitOptions.None);
            Int3 ret = new Int3();
            if (posArr.Length == 3) {
                int.TryParse(posArr[0], out ret.x);
                int.TryParse(posArr[1], out ret.y);
                int.TryParse(posArr[2], out ret.z);
            }
            
            return ret;
        }
        
        /// <summary>
        /// 解析千分比
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ParsePer1000(string value)
        {
            UInt32 v2 = ParseU32(value);
            return v2 * 0.001f;
        }
        
        public static TEnum ParseEnum<TEnum>(string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
        
        public static T[] ParseArr<T>(string value)
        {
            T[] item;
            if (GetArr(value, ArrSplitLv2, out item, ParseAny<T>)) {
                return item;
            }
            return new T[0];
        }
        
        /// <summary>
        /// 主要用在ParseArr
        /// 支持string, enum, number的解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseAny<T>(string value)
        {
            Type tType = typeof(T);
            if (tType.IsEnum) {
                return ParseEnum<T>(value);
            }
            
            if (tType == typeof(string)) {
                return (T)(object)value;
            }
            
            double doubleValue;
            if (!double.TryParse(value, out doubleValue)) {
                return default(T);
            }
            
            if (typeof(T) == typeof(double)) {
                return (T)(object)doubleValue;
            }
            
            try {
                object o = Convert.ChangeType(doubleValue, typeof(T));
                return (T)o;
            } catch (Exception e) {
                Logger.LogError(e.ToString(), "StrHelper.ParseAny");
                return default(T);
            }
        }
        
        /// <summary>
        /// 得到字符串的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">默认值为null</param>
        /// <param name="forceMultibyteToTwo">强制多字节按两字节算</param>
        /// <returns></returns>
        public static int GetStrByteLen(string str, Encoding encoding = null, bool forceMultibyteToTwo = false)
        {
            if (str == null) {
                return 0;
            }
            
            if (encoding == null) {
                encoding = Encoding.UTF8;
            }
            
            int totalLen = 0;
            for (int i = 0; i < str.Length; i++) {
                string c = str[i].ToString();
                int cLen = encoding.GetByteCount(c);
                if (forceMultibyteToTwo && cLen > 2) {
                    cLen = 2;
                }
                totalLen += cLen;
            }
            
            return totalLen;
        }
        
        /// <summary>
        /// 按字节长度来截取字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="limitNum"></param>
        /// <param name="encoding">默认值为null</param>
        /// <param name="forceMultibyteToTwo">强制多字节按两字节算</param>
        /// <returns></returns>
        public static string GetByteLenStr(string str, int limitNum, Encoding encoding = null, bool forceMultibyteToTwo = false)
        {
            if (encoding == null) {
                encoding = Encoding.UTF8;
            }
            
            int len = encoding.GetByteCount(str);
            if (len > limitNum) {
                int totalLen = 0;
                int i;
                for (i = 0; i < str.Length; i++) {
                    string c = str[i].ToString();
                    int cLen = encoding.GetByteCount(c);
                    if (forceMultibyteToTwo && cLen > 2) {
                        cLen = 2;
                    }
                    totalLen += cLen;
                    if (totalLen > limitNum) {
                        break;
                    }
                }
                return str.Substring(0, i);
            }
            
            return str;
        }
        
        /// <summary>
        /// 从串中取第index个值字符串。index 序号从 1开始。
        /// </summary>
        public static string GetStrValue(string strTarget, int index)
        {
            string v = "*";
            char[] cutChar = v.ToCharArray();
            string[] sArray = strTarget.Split(cutChar);
            if (sArray.Length <= index || index < 0) {
                return string.Empty;
            }
            return sArray[index];
        }
        
        /// <summary>
        /// 从串中取第index个值整形值，使用 英文 逗号 分隔
        /// </summary>
        public static int GetIntValue(string strTarget, int index)
        {
            string v = "*";
            char[] cutChar = v.ToCharArray();
            string[] sArray = strTarget.Split(cutChar);
            if (sArray.Length <= index || index < 0) {
                return -1;
            }
            return int.Parse(sArray[index]);
        }
        
        /// <summary>
        /// 根据切个符号切割字符串
        /// </summary>
        public static string[] SplitString(string strTarget, string spltStr)
        {
            return strTarget.Split(spltStr.ToCharArray());
        }
        
        public static List<int> SplitNumberString(string strTarget, string spltStr)
        {
            List<int> list = new List<int>();
            string[] strArr = SplitString(strTarget, spltStr);
            for (int i = 0; i < strArr.Length; i++) {
                Regex isNumeric = new Regex(@"^\d+$");
                if (isNumeric.IsMatch(strArr[i])) {
                    list.Add(int.Parse(strArr[i]));
                }
            }
            
            return list;
        }
        
        /// <summary>
        /// 根据字符串，替换 配置表读取专用
        /// </summary>
        public static string ReplaceString(string strTarget, List<string> strList)
        {
            for (int i = 0; i < strList.Count; i++) {
                string strSplit = "{Text" + (i + 1).ToString() + "}";
                if (strTarget.Contains(strSplit)) {
                    strTarget = strTarget.Replace(strSplit, strList[i]);
                }
            }
            
            return strTarget;
        }
        
        /// <summary>
        /// 根据字符串，替换 配置表读取专用
        /// </summary>
        public static string ReplaceString(string strTarget, string[] strList)
        {
            for (int i = 0; i < strList.Length; i++) {
                string strSplit = "{Text" + (i + 1).ToString() + "}";
                if (strTarget.Contains(strSplit)) {
                    strTarget = strTarget.Replace(strSplit, strList[i]);
                }
            }
            // 处理//n为/n 换行
            strTarget = strTarget.Replace("\\n", "\n");
            
            return strTarget;
        }
        
        
        /// <summary>
        /// 获取字符串的单元长度
        /// </summary>
        public static int GetLength(string strTarget)
        {
            string v = "*";
            char[] cutChar = v.ToCharArray();
            string[] sArray = strTarget.Split(cutChar);
            return sArray.Length;
        }
        
        /// <summary>
        /// 从串中判断是否为纯数字
        /// </summary>
        ///
        public static bool IsStringNumberic(string message)
        {
            System.Text.RegularExpressions.Regex rex =
                new System.Text.RegularExpressions.Regex(@"^\d+$");
            long result = -1;
            if (rex.IsMatch(message)) {
                result = long.Parse(message);
                return true;
            } else {
                return false;
            }
        }
    }
}