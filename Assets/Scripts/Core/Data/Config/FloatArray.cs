using System;

namespace IGG.Core.Data.Config
{
    /// <summary>
    /// asset序列化不支持泛型，不得不这么写
    /// </summary>
    [Serializable]
    public class FloatArray : SerializableArr<float>
    {
    }
}
