using IGG.Core.Module;
using IGG.Logging;
using IGG.Core.Data.DataCenter;
using IGG.Core.Data.DataCenter.Dialog;

namespace IGG.Core.Module
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.2.20
    /// Desc    Dialog模块
    /// </summary>
    public class DialogModule : IModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void OnInit()
        {
            // 用于模拟数据的一个接口，正式需关闭
            //DialogDC.SimulationData();
            RegisterDCHooks();
        }
        
        /// <summary>
        /// 注册msg消息到DataCenter,关联对应的DC处理msg消息
        /// </summary>
        public void RegisterDCHooks()
        {
            /// 示例
            ///DataCenter.RegisterDCHooks(eMsgTypes.MsgServerKeepLive, GameTimeDC.RecvMsgGS2CLServerTimeNotice);
        }
        
        /// <summary>
        /// 重登录清理数据中心数据
        /// </summary>
        public void ClearDC()
        {
            DialogDC.ClearDC();
        }
    }
}

namespace IGG.Core
{
    /// 定义本模块的事件类型，用于EventCenter
    public partial class EventCenterType
    {
        /// <summary>
        /// 事件格式例子
        /// </summary>
        public const string Dialog = "Dialog_example";
        
        /// <summary>
        /// 剧情结束通知
        /// </summary>
        public const string DialogEnd = "Dialog_End";   //参数为uint dialogid
    }
    /// 定义事件的参数对象数据结构
}