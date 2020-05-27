using System;
using UnityEngine;

namespace IGG.Core.Helper
{
    public static class TipsHelper
    {
        /// <summary>
        /// 显示提示窗口
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="title">标题ID</param>
        /// <param name="type">确认对话框类型</param>
        /// <param name="callback">回调,回调参数中可以判断点击了那个按钮</param>
        /// <returns></returns>
        public static void ShowMsgBox(string msg, string title, MsgBoxType type = MsgBoxType.Ok, Action<IMsgBoxResult> callback = null)
        {
            WndManager.CreateWnd<ConfirmWnd>(WndType.DialogWnd, false, false, (wnd) => {
                (wnd as ConfirmWnd).SetData(msg, title, type, callback);
            });
        }
        
        
        /// <summary>
        /// 显示剧情对话skip提示窗口
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="callback">回调,回调参数中可以判断点击了那个按钮</param>
        /// <returns></returns>
        public static void ShowSkipMsgBox(string msg, Action<IMsgBoxResult> callback = null)
        {

        }
        
        /// <summary>
        /// 关闭消息框
        /// </summary>
        public static void CloseMsgBox()
        {
            WndManager.DestoryWnd<ConfirmWnd>();
        }
        
        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="errId"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public static void ShowMsgBoxByErrId(int errId, params string[] paramArr)
        {
            //string tips = MessageDao.Inst.GetErrInfo((ErrorCode)errId, paramArr);
            string tips = "";
            if (string.IsNullOrEmpty(tips)) {
                return ;
            }
            ShowMsgBox("提示", tips);
        }
        
        /// <summary>
        /// 显示tips
        /// </summary>
        /// <param name="msg">tip 内容</param>
        /// <param name="title">tip 标题</param>
        /// <param name="tipRect">弹出tips 的对象</param>
        /// <param name="align">对其方式</param>
        /// <param name="offset">修正偏移值</param>
        /// <returns></returns>
        public static bool ShowTips(string msg, string title, RectTransform tipRect, TipAlignment align, Vector2 offset)
        {
            ResourceManger.LoadWndItem("HintItem", (WndManager.GetUINode() as UINode).GetWndParent(WndType.DialogWnd), false,
            (obj) => {
                HintItem item = obj.GetComponent<HintItem>();
                if (item != null) {
                    item.SetData(new object[] { msg, title, tipRect, align, offset });
                }
            });
            return true;
        }
        
        
        /// <summary>
        /// 显示中央讯息
        /// </summary>
        /// <param name="msg">tip 内容</param>
        /// <param name="type">0:战斗外用，1：战斗内用的</param>
        /// <returns></returns>
        public static void ShowTipsMessage(string msg, int type)
        {
            WndManager.CreateWnd<MessageWnd>(WndType.DialogWnd, false, false, (wnd) => {
                (wnd as MessageWnd).SetType(type);
                (wnd as MessageWnd).SetData(msg);
            });
        }
        
        /// <summary>
        /// 显示敌我目标tips
        /// </summary>
        /// <param name="Info">目标数据</param>
        /// <param name="tipRect">弹出tips的控件对象</param>
        /// <param name="align">对其方式</param>
        /// <param name="isTouchDestroy">是否触碰销毁</param>
        /// <returns></returns>
        public static void ShowTargetTips(TargetHeroInfo Info, RectTransform tipRect, TipAlignment align, Vector2 offset, bool isTouchDestroy)
        {
            WndManager.DestoryWnd<TargetInfoWnd>();
            WndManager.CreateWnd<TargetInfoWnd>(WndType.DialogWnd, false, false, (wnd) => {
                (wnd as TargetInfoWnd).SetData(new object[] { 0, Info, tipRect, align, offset, isTouchDestroy });
            });
        }
        
        
        /// <summary>
        /// 显示敌我目标tips
        /// </summary>
        /// <param name="Info">目标数据</param>
        /// <param name="ScreenPos">屏幕位置</param>
        /// <param name="align">对其方式</param>
        /// <param name="TargetSize">触碰3D对象的在屏幕中的大小</param>
        /// <param name="isTouchDestroy">是否触碰销毁</param>
        /// <returns></returns>
        public static void ShowTargetTips(TargetHeroInfo Info, Vector2 ScreenPos, TipAlignment align, Vector2 TargetSize, bool isTouchDestroy)
        {
            WndManager.CreateWnd<TargetInfoWnd>(WndType.DialogWnd, false, false, (wnd) => {
                (wnd as TargetInfoWnd).SetData(new object[] { 1, Info, ScreenPos, align, TargetSize, isTouchDestroy });
            });
        }
        
        /// <summary>
        /// 关闭敌我目标tips
        /// </summary>
        /// <returns></returns>
        public static void CloseTargetTips()
        {
            WndManager.DestoryWnd<TargetInfoWnd>();
        }
        
        
        /// <summary>
        /// 显示剧情对话
        /// </summary>
        /// <param name="dialogID">剧情id</param>
        /// <param name="TestMode">调试模式，给测试用的</param>
        /// <returns></returns>
        public static void ShowDialog(uint dialogID, bool TestMode = false)
        {
            WndManager.CreateWnd<AdventureDialogueWnd>(WndType.DialogWnd, false, false, (wnd) => {
                (wnd as AdventureDialogueWnd).SetData(dialogID);
                (wnd as AdventureDialogueWnd).TestMode = TestMode;
            });
        }
        
        /// <summary>
        /// 过渡窗口处理
        /// </summary>
        /// <param name="waitTime">loading 过渡时间</param>
        /// <param name="callback">loading,回调参数中可以判断点击了那个按钮</param>
        /// <returns></returns>
        public static bool LoadingWnd(float waitTime, Action<ILoading> callback = null)
        {
            WndManager.CreateWnd<LoadingWnd>(WndType.NormalWnd, false, false, (wnd) => {
                (wnd as LoadingWnd).SetData(waitTime, callback);
            });
            return true;
        }
    }
    
    // 对其方式
    public enum TipAlignment {
        Left = 0x01,
        Right = 0x02,
        Up = 0x04,
        Down = 0x08,
        LeftUp = Left | Up,
        LeftDown = Left | Down,
        RightUp = Right | Up,
        RightDown = Right | Down,
        
    }
    
    /// <summary>
    /// 目标英雄信息
    /// </summary>
    public class TargetHeroInfo
    {
        public bool IsSelf = true;   // true 己方英雄， false，敌方英雄
        public uint ID;              // 己方就是英雄id hero_info.csv 的ID字段，敌方就是怪物id，Creature.csv中的id
        public uint Lev;             // 等级
        public uint Star;             // 英雄星级
        public uint Attack;           // 攻击力
        public uint Defence;          // 防御力
        public uint Hp;               // HP
    }
}