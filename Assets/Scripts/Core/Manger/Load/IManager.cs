using UnityEngine;
using IGG.Core;

namespace IGG.Core.Manger
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.7
    /// Desc    管理器接口
    /// </summary>
    public interface IManager: IDisposer
    {
        bool Enabled { get; set; }
        void Initialize(MonoBehaviour mb);
        void Update();
    }
}