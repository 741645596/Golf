#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
using IGG.Core;

namespace IGG.Core.Data.Config
{
    /// Author  gaofan
    /// Date    2017.12.13
    /// Desc    配置文件解码器的接口
    public interface ICfgDecoder : IDisposer
    {
        bool Enable { get; }

        string GetName();

        string GetSaveName();

        bool Decode(ICfgReader table);

        void AllDecodeAfterProcess();

        object Data { get; }
    }
}
#endif