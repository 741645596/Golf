#if ServerClient
using System;
using System.Runtime.InteropServices;
using IGG.Game.Data.Config;

namespace IGG.Logging
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.11.27
    /// Desc    在服务器端使用的日志
    /// </summary>
    public class ServerLogger:ILogger
    {
        [DllImport("clientbattlelog")]
        private static extern void BattleLog(string content);

        [DllImport("clientbattlelog")]
        private static extern void BattleLogInit();

        [DllImport("clientbattlelog")]
        private static extern void BattleLogRelease();

        /// <summary>
        /// 用于特殊情况下外部handler
        /// </summary>
        public static Action<string, string, string> LogHandler;

        public bool Init(string cfgPath)
        {
            BattleLogInit();

            try
            {
                ConfigMgr.CfgPath = cfgPath;
                ConfigMgr.LoadAll();
            }
            catch (Exception ex)
            {
                LogError("Init Client Cfg Error " + ex.ToString(), "ClientLogger.Init");
                return false;
            }

            m_init = true;
            return true;
        }

        public void Release()
        {
            BattleLogRelease();
            try
            {
                ConfigMgr.Clear();
            }
            catch (Exception ex)
            {
                LogError("Clear Client Cfg Error: " + ex.ToString(), "ClientLogger.Release");
            }
            m_init = false;
        }

        public void LogInfo(string msg, string path)
        {
            Log("Info", msg, path);
        }

        public void LogDebug(string msg, string path)
        {
            Log("Debug", msg, path);
        }

        public void LogWarning(string msg, string path)
        {
            Log("Warning", msg, path);
        }

        public void LogError(string msg, string path)
        {
            Log("Error", msg, path);
        }

        private void Log(string type, string msg, string path)
        {
            if (LogHandler != null)
            {
                LogHandler(type, msg, path);
            }

            if (!m_init)
            {
                return;
            }

            string head = type;
            if (!string.IsNullOrEmpty(path))
            {
                head += " p=" + path;
            }

            string log = String.Format("[{0} t={1}]:{2}", head, DateTime.Now.TimeOfDay, msg);
            BattleLog(log);
        }

        private bool m_init = false;
    }
}
#endif