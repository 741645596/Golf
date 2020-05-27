namespace IGG.Logging
{
    /// <summary>
    /// 日志接口
    /// @author gaofan
    /// </summary>
    public interface ILogger
    {
        void LogInfo(string msg, string path);
        void LogDebug(string msg, string path);
        void LogWarning(string msg, string path);
        void LogError(string msg, string path);

    }
}
