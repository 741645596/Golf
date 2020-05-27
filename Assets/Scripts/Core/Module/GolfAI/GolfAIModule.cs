using IGG.Core.Module;
using IGG.Logging;
using IGG.Core.Data.DataCenter;
using IGG.Core.Data.DataCenter.GolfAI;

namespace IGG.Core.Module
{
    /// <summary>
    /// Author  zhulin
    /// Date    2019.5.6
    /// Desc    GolfAI模块
    /// </summary>
    public class GolfAIModule : IModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void OnInit()
        {
		    // 用于模拟数据的一个接口，正式需关闭
		    //GolfAIDC.SimulationData();
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
            GolfAIDC.ClearDC();
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
        public const string GolfAI = "GolfAI_example";
    }
	/// 定义事件的参数对象数据结构
}