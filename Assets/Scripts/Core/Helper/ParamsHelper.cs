using System;
using System.Collections;

namespace IGG.Core.Helper
{
    /// <summary>
    /// 基于Object的多参数读取工具
    /// @author gaofan
    /// </summary>
    public static class ParamsHelper
    {
        public static string LastGetParamError;

        /// <summary>
        /// 尝试获取参数对象的值
        /// </summary>
        /// <typeparam name="T">要获取的参数类型</typeparam>
        /// <param name="data">object or list</param>
        /// <param name="index">参数数组中的第几个</param>
        /// <param name="defaultValue">如果获取错误时的默认值</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static T TryGetParam<T>(object data, int index, T defaultValue, ref string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                error += "\n";
            }

            if (data == null)
            {
                error += "data == null";
                return defaultValue;
            }

            if (index < 0)
            {
                error += "[" + index + "]: index < 0";
                return defaultValue;
            }

            if (data is IList)
            {
                IList datas = (IList) data;
                if (index >= datas.Count)
                {
                    error += "[" + index + "]: index >= arrlen";
                    return defaultValue;
                }

                data = datas[index];
            }
            else
            {
                if (index != 0)
                {
                    error += "[" + index + "]: >= 0";
                    return defaultValue;
                }
            }

            try
            {
                return (T) data;
            }
            catch (Exception ex)
            {
                error += "[" + index + "]: " + ex.Message;
                return defaultValue;
            }
        }

        /// <summary>
        /// 尝试获取参数对象的值
        /// 如果发生错误，错误则在LastGetParamError里
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryGetParam<T>(object data, int index, T defaultValue)
        {
            LastGetParamError = "";
            return TryGetParam<T>(data, index, defaultValue, ref LastGetParamError);
        }

        /// <summary>
        /// 尝试获取参数对象的值的最简使用方式
        /// 如果发生错误，错误则在LastGetParamError里
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T TryGetParam<T>(object data, int index)
        {
            return TryGetParam<T>(data, index, default(T));
        }
    }
}