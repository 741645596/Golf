using System;
using System.Collections;
using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    /// <summary>
    /// 为了序列为二维数组而创建出来的因数
    /// author:gaofan
    /// </summary>
    [Serializable]
    public class SerializableArr<T>:IEnumerable<T>
    {
        public T[] Data;

        public T this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public int Length
        {
            get
            {
                if (Data == null)
                {
                    return 0;
                }

                return Data.Length;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SerializableArrEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static T[][] ConvertToArr<TArr>(TArr[] arr) where TArr : SerializableArr<T>
        {
            if (arr == null)
            {
                return default(T[][]);
            }

            if (arr.Length < 1)
            {
                return new T[0][];
            }

            T[][] result = new T[arr.Length][];

            for (int i = 0; i < arr.Length; i++)
            {
                result[i] = arr[i].Data;
            }
            return result;
        }

        public static T[][] ConvertToArr(SerializableArr<T>[] arr)
        {
            if (arr == null)
            {
                return default(T[][]);
            }

            if (arr.Length < 1)
            {
                return new T[0][];
            }

            T[][] result = new T[arr.Length][];

            for (int i = 0; i < arr.Length; i++)
            {
                result[i] = arr[i].Data;
            }
            return result;
        }
    }

    public class SerializableArrEnumerator<T>:IEnumerator<T>
    {
        private SerializableArr<T> m_arr;
        private int m_index;
        private T m_curValue;

        public SerializableArrEnumerator(SerializableArr<T> arr)
        {
            m_arr = arr;
            m_index = -1;
        }

        public bool MoveNext()
        {
            m_index++;
            if (m_index >= m_arr.Length)
            {
                return false;
            }
            else
            {
                m_curValue = m_arr[m_index];
            }
            return true;
        }

        public void Reset()
        {
            m_curValue = default(T);
            m_index = -1;
        }

        public T Current
        {
            get { return m_curValue; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
            Reset();
            m_arr = null;
        }
    }
}
