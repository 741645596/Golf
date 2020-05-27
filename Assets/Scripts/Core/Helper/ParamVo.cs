using IGG.Core.Utils;

namespace IGG.Core.Helper
{
    public class ParamVo
    {
        private string m_error;

        /// <summary>
        /// 原始数据
        /// </summary>
        public object Data;

        /// <summary>
        /// 取参数时的错误
        /// </summary>
        public string Error
        {
            get { return m_error; }
        }

        /// <summary>
        /// 取参数时，是否有错误
        /// </summary>
        public bool HasError
        {
            get { return !string.IsNullOrEmpty(m_error); }
        }

        /// <summary>
        /// 取出参数列表的第index个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(int index, T defaultValue)
        {
            return ParamsHelper.TryGetParam<T>(Data, index, defaultValue, ref m_error);
        }

        /// <summary>
        /// 取出参数列表的index个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Get<T>(int index)
        {
            return ParamsHelper.TryGetParam<T>(Data, index, default(T), ref m_error);
        }

        /// <summary>
        /// 默认取出第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            return ParamsHelper.TryGetParam<T>(Data, 0, default(T), ref m_error);
        }

        public void Reset()
        {
            Data = null;
            m_error = null;
        }
    }
}