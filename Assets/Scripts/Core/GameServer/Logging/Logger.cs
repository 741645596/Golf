namespace IGG.Logging
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.11.27
    /// Desc    默认的日志信息
    /// </summary>
    public class Logger
    {
        static Logger()
        {
#if ServerClient
            g_logger = new ServerLogger();
#else
            g_logger = new ClientLogger();
#endif
        }

        public static ILogger Instance
        {
            get
            {
                return g_logger;
            }
        }
        private static ILogger g_logger;

        /// <summary>
        /// 普通消息的LOG
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="path"></param>
        [System.Diagnostics.Conditional("LOG")]
        public static void Log(string msg, string path = "")
        {
            g_logger.LogInfo(msg,path);
        }

        [System.Diagnostics.Conditional("LOG")]
        public static void Log(object msg)
        {
            if (msg == null)
            {
                Log("null");
            }
            else
            {
                Log(msg.ToString());
            }
        }

        [System.Diagnostics.Conditional("LOG")]
        public static void Log(object msg, System.Object content)
        {
            if (msg == null)
            {
                Log("null");
            }
            else
            {
                Log(msg.ToString());
            }
          
        }

        /// <summary>
        /// 调试用
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="canShow"></param>
        [System.Diagnostics.Conditional("LOG")]
        public static void LogDebug(object msg, bool canShow = true)
        {
            
        }

        /// <summary>
        /// 调试信息的LOG
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="path"></param>
        [System.Diagnostics.Conditional("LOG")]
        public static void LogDebug(string msg, string path = "")
        {
            g_logger.LogDebug(msg, path);
        }

        /// <summary>
        /// 警告信息的LOG
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="path"></param>
        public static void LogWarning(string msg, string path = "")
        {
            g_logger.LogWarning(msg, path);
        }

        public static void LogWarning(object msg)
        {
            LogWarning(msg.ToString());
        }

        public static void LogWarning(object msg, System.Object content)
        {
            LogWarning(msg.ToString());
        }

        /// <summary>
        /// 错误信息的log
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="path"></param>
        public static void LogError(string msg, string path = "")
        {
            g_logger.LogError(msg, path);
        }

        public static void LogError(object msg)
        {
            LogError(msg.ToString());
        }

        public static void LogError(object msg, System.Object content)
        {
            LogWarning(msg.ToString());
        }

        public static void LogException(string msg, string path = "")
        {
            g_logger.LogError(msg, path);
        }

        public static void LogException(object msg)
        {
            LogException(msg.ToString());
        }

        public static void LogException(object msg, System.Object content)
        {
            LogWarning(msg.ToString());
        }
    }

}