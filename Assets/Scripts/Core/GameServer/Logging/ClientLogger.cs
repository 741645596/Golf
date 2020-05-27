#if !ServerClient
using System;
using UnityEngine;

namespace IGG.Logging
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.11.27
    /// Desc    在客户端使用的LOGGER
    /// </summary>
    public class ClientLogger:ILogger
    {
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
            string head = type;
            if (!string.IsNullOrEmpty(path))
            {
                head += " p=" + path;
            }

            var now = DateTime.Now;
            var time = string.Format("{0}/{1} {2:D2}:{3:D2}:{4:D2}.{5:D3}", now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
            string log = String.Format("[{0} t={1}]:{2}", head, time, msg);

            switch (type)
            {
                case "Warning":
                    Debug.LogWarning(log);
                    break;
                case "Error":
                    Debug.LogError(log);
                    break;
                default:
                    Debug.Log(log);
                    break;
            }
        }
    }
}
#endif