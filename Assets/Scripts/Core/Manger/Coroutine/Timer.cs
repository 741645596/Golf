using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 定时器管理
/// </summary>
namespace IGG.Core.Manger.Coroutine
{
    public class Timer
    {
        // 所有的定时器列表
        private static List<Timer> timers = new List<Timer>();
        private static List<Timer> listTemp = new List<Timer>();
        
        // 超时触发时间
        private float timeout = 0f;
        
        // 定时器的处理回调
        private Task cb;
        
        // 定时器ID
        private long timerID = 0;
        private static long currSeq = 0;
        
        // 定时器的名字
        private string timerName;
        
        /// <summary>
        /// 创建一个定时器，超时时间单位为秒
        /// </summary>
        public Timer(string name, float timeout, Task task)
        {
            this.timeout = timeout + Time.time;
            this.cb = task;
            
            // 确保ID > 1
            timerID = ++currSeq;
            if (timerID < 1) {
                timerID = 1;
                currSeq = 1;
            }
            
            // 定时器的名字
            if (name != null && name != string.Empty) {
                this.timerName = name;
            } else {
                this.timerName = "unname timer";
            }
            
            // 记录此定时器
            timers.Add(this);
        }
        
        /// <summary>
        /// 根据名字删除定时器(会将所有同名的删除器干掉)
        /// </summary>
        public static void Delete(string name)
        {
            List<Timer> toDelete = new List<Timer>();
            foreach (Timer timer in timers) {
                if (timer.name.Equals(name)) {
                    toDelete.Add(timer);
                }
            }
            foreach (Timer timer in toDelete) {
                timers.Remove(timer);
            }
        }
        
        /// <summary>
        /// 根据ID删除定时器
        /// </summary>
        public static void Delete(long id)
        {
            Timer toDelete = null;
            foreach (Timer timer in timers) {
                if (timer.id == id) {
                    toDelete = timer;
                    break;
                }
            }
            timers.Remove(toDelete);
        }
        
        /// <summary>
        /// 更新定时器，每帧都需要进行调度
        /// </summary>
        public static void Update()
        {
            listTemp.Clear();
            foreach (Timer timer in timers) {
                if (timer.timeout <= Time.time)
                    // 时间到了，加入列表中
                {
                    listTemp.Add(timer);
                }
            }
            
            // 处理已经超时的定时器
            foreach (Timer timer in listTemp) {
                if (timer.cb != null) {
                    timer.cb.Go();
                }
                timers.Remove(timer);
            }
        }
        
        /// <summary>
        /// 查找定时器
        /// </summary>
        public static Timer Find(int id)
        {
            foreach (Timer timer in timers) {
                if (timer.id == id) {
                    return timer;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 定时器名字
        /// </summary>
        public string name { get { return timerName; } }
        
        /// <summary>
        /// 定时器ID
        /// </summary>
        public long id { get { return timerID; } }
    }
}


