using System;

namespace IGG.Core
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.6
    /// Desc    单例类基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : Disposer where T : Singleton<T>, new()
    {
        private static T g_inst;

        public static T Inst
        {
            get
            {
                if (g_inst == null)
                {
                    new T();
                }
                if (g_inst != null)
                {
                    g_inst.OnGetInst();
                }
                return g_inst;
            }
        }

        public Singleton()
        {
            if (g_inst != null)
            {
#if UNITY_EDITOR
                throw new Exception(this + "是单例类，不能实例化两次");
#else
                return;
#endif
            }

            g_inst = (T) this;
        }

        protected virtual void OnGetInst()
        {

        }

        protected override void OnDispose()
        {
            g_inst = default(T);
        }
    }
}