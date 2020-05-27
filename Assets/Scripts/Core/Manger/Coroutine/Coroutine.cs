using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 协程的实现
/// </summary>
namespace IGG.Core.Manger.Coroutine
{
    public class IGGCoroutine
    {
        // 线程池
        private static List<YCContext> queue = new List<YCContext>();
        
        /// <summary>
        /// 线程调度，每一帧执行一次
        /// </summary>
        public static int Update()
        {
            for (int i = 0; i < queue.Count; i++) {
                YCContext c = queue[i];
                try {
                    if (!c.IsFinished()) {
                        c.Step();
                    }
                } catch (System.Exception e) {
                    // 有报错，需要进行收尾操作，并摘除掉
                    c.Error();
                    queue.RemoveAt(i);
                    i--;
                    UnityEngine.Debug.LogError(e);
                }
                
                if (c.IsFinished()) {
                    // 调度完毕，摘除掉
                    queue.RemoveAt(i);
                    i--;
                }
            }
            return 1;
        }
        /// <summary>
        /// 扔到协程中执行
        /// </summary>
        /// <param name="c">协程函数</param>
        /// <param name="owner">如果指明了owner，那么当owner=null时协程将自动被回收不予执行</param>
        /// <param name="f">如果指明了f，那么当有异常时会自动调用f来进行收尾工作，相对于try块的finally语句</param>
        /// <returns></returns>
        public static YCCoroutine DispatchService(IEnumerator c,
            GameObject owner = null, CoroutineErrorCallback f = null)
        {
            YCContext ycc = new YCContext(c, owner);
            
            // 立刻先执行一把
            try {
                ycc.Step();
            } catch (System.Exception e) {
                // 有报错，需要进行收尾操作，并摘除掉
                ycc.Error();
                UnityEngine.Debug.LogError(e);
                return null;
            }
            
            if (ycc.IsFinished())
                // 立刻就执行完毕了，就不要加到队列中
            {
                return null;
            }
            
            // 加到队列中继续执行
            queue.Add(ycc);
            return new YCCoroutine(ycc, owner, f);
        }
    }
    
    public interface IYYieldInstruction
    {
        bool IsFinished();
        void Error();
    }
    
    public class YCCoroutine : IYYieldInstruction
    {
        private YCContext context;
        private GameObject owner;
        private bool hasOwner;
        private CoroutineErrorCallback f;
        public YCCoroutine(YCContext c, GameObject owner, CoroutineErrorCallback f)
        {
            this.context = c;
            this.owner = owner;
            this.hasOwner = (owner != null);
            this.f = f;
        }
        public bool IsFinished()
        {
            return hasOwner && owner == null || context.IsFinished();
        }
        
        public void Error()
        {
            if (f != null) {
                try {
                    f();
                } catch (Exception e) {
                    UnityEngine.Debug.LogError(e);
                }
            }
        }
    }
    
    public class WaitForMSeconds : IYYieldInstruction
    {
        private float tick;
        public WaitForMSeconds(float msec)
        {
            this.tick = UnityEngine.Time.time + (float)msec / 1000f;
        }
        public bool IsFinished()
        {
            return UnityEngine.Time.time >= tick;
        }
        public void Error() { }
    }
    
    public class YCContext
    {
        IEnumerator coroutine;
        IYYieldInstruction currYield;
        bool isFinished;
        bool hasOwner;
        GameObject owner;
        public YCContext(IEnumerator c, GameObject owner)
        {
            coroutine = c;
            currYield = coroutine.Current as IYYieldInstruction;
            
            this.owner = owner;
            hasOwner = (owner != null);
        }
        public void Step()
        {
            if (isFinished || hasOwner && owner == null)
                // 已经完成了，嘛也不干
            {
                return;
            }
            
            if (currYield != null && currYield.IsFinished()) {
                currYield = null;
            }
            if (currYield == null) {
                if (!coroutine.MoveNext()) {
                    isFinished = true;
                } else {
                    currYield = coroutine.Current as IYYieldInstruction;
                }
            }
        }
        
        public void Reset()
        {
            coroutine.Reset();
        }
        public bool IsFinished()
        {
            return isFinished;
        }
        
        // 有报错时的调度
        public void Error()
        {
            if (currYield != null) {
                currYield.Error();
            }
        }
    }
    
    public delegate void CoroutineErrorCallback();
}


