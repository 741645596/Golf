using System;

namespace IGG.Core.Helper
{
    /// <summary>
    ///字符串数组类型的参数列表专用
    /// author: gaofan
    /// </summary>
    public class StrParamsVo
    {
        private string m_error;

        /// <summary>
        /// 原始数据
        /// </summary>
        public string[] Data;

        public StrParamsVo(string[] data = null)
        {
            Data = data;
        }

        /// <summary>
        /// 得到并清除错误
        /// </summary>
        /// <returns></returns>
        public string GetAndClearError()
        {
            string t = m_error;
            m_error = null;
            return t;
        }

        /// <summary>
        /// 取参数时，是否有错误
        /// </summary>
        public bool HasError
        {
            get { return !string.IsNullOrEmpty(m_error); }
        }

        /// <summary>
        /// 参数的个数
        /// </summary>
        public int Length
        {
            get
            {
                if (Data == null)
                {
                    return 0;
                }
                else
                {
                    return Data.Length;
                }
            }
        }

        /// <summary>
        /// 得到int
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Int32 GetI32(int index, Int32 defaultValue = 0)
        {
            string v = GetStr(index);
            if (v == null)
            {
                return defaultValue;
            }

            Int32 valueOut;
            if (Int32.TryParse(v, out valueOut))
            {
                return valueOut;
            }

            m_error = "[" + index + "]: Int32.TryParse error";
            return defaultValue;
        }

        /// <summary>
        /// 得到double
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double GetF64(int index, double defaultValue = 0)
        {
            string v = GetStr(index);
            if (v == null)
            {
                return defaultValue;
            }

            double valueOut;
            if (double.TryParse(v, out valueOut))
            {
                return valueOut;
            }

            m_error = "[" + index + "]: double.TryParse error";
            return defaultValue;
        }

        /// <summary>
        /// 得到枚举值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public TEnum GetEnum<TEnum>(int index, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            string v = GetStr(index);
            if (v == null)
            {
                return defaultValue;
            }

            /*TEnum valueOut;
            if (Enum.TryParse(v, out valueOut))
            {
                return valueOut;
            }*/

            m_error = "[" + index + "]: Enum.TryParse error";
            return defaultValue;
        }

        /// <summary>
        /// 取出原始字符串
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetStr(int index)
        {
            if (Data == null ||
                index >= Data.Length ||
                index < 0)
            {
                m_error = "[" + index + "]: not found";
                return null;
            }

            return Data[index];
        }

        public void Reset()
        {
            Data = null;
            m_error = null;
        }
    }
}