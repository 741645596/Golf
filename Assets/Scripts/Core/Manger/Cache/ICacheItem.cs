using System;

namespace IGG.Game.Core.Cache
{
    /// <summary>
    /// 缓存对象的接口
    /// @author gaofan
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheItem<T> where T: ICacheItem<T>
    {
        /// <summary>
        /// 生存时间
        /// </summary>
        float LiveTime { get; }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="returnFun"></param>
        /// <param name="param"></param>
        void Initialize(Action returnFun, Object param);
        
        /// <summary>
        /// 使用完后调用这个
        /// 将返回到内存池
        /// </summary>
        void Return();
    }
}
