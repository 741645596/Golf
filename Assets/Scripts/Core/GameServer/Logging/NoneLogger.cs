namespace IGG.Logging
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.11.27
    /// Desc    就是一个日志的占位符
    /// </summary>
    public class NoneLogger : ILogger
    {
        public static readonly NoneLogger Inst = new NoneLogger();

        public void LogInfo(string msg, string path)
        {
        }

        public void LogDebug(string msg, string path)
        {
        }

        public void LogWarning(string msg, string path)
        {
        }

        public void LogError(string msg, string path)
        {
        }
    }
}