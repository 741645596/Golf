using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGG.Core.Module
{
    public interface IModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void OnInit();

        /// <summary>
        /// 注册msg消息到DataCenter,关联对应的DC处理msg消息
        /// </summary>
        void RegisterDCHooks();


        /// <summary>
        /// 重等清理数据中心数据
        /// </summary>
        void ClearDC();
    }

 }
