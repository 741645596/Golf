using IGG.Core.Data.Config;
using System;
using System.Collections.Generic;

/// <summary>
/// Author  zhulin
/// Date    2019.2.14
/// Desc    SimServer模块动态数据集
/// </summary>
namespace IGG.Core.Data.DataCenter.SimServer
{
    /// <summary>
    /// 整个游戏中的事件信息，之所以没定成dic结构，是因为本地存储不支持dic结构了。
    /// </summary>
    [Serializable]
    public class PuzzleEventInfo
    {
        public List<ChapterEventInfo> ListChapterEvent = new List<ChapterEventInfo>();
        /// <summary>
        /// 获取某章节的胜利关卡列表
        /// </summary>
        /// <param name="chapter">章节id</param>
        /// <returns></returns>
        public List<uint> GetWinBattleList(uint chapter)
        {
            List<uint> l = new List<uint>();
            foreach (ChapterEventInfo v in ListChapterEvent) {
                if (v.CharterID == chapter) {
                    if (v.ListWinBattleID != null && v.ListWinBattleID.Count > 0) {
                        l.AddRange(v.ListWinBattleID);
                    }
                }
            }
            return l;
        }
        
        /// <summary>
        /// 获取某章节的进入过战斗的关卡列表
        /// </summary>
        /// <param name="chapter">章节id</param>
        /// <returns></returns>
        public List<uint> GetEnterBattleList(uint chapter)
        {
            List<uint> l = new List<uint>();
            foreach (ChapterEventInfo v in ListChapterEvent) {
                if (v.CharterID == chapter) {
                    if (v.ListEnterBattleID != null && v.ListEnterBattleID.Count > 0) {
                        l.AddRange(v.ListEnterBattleID);
                    }
                }
            }
            return l;
        }
        
        /// <summary>
        /// 获取某章节获得的宝箱ID列表
        /// </summary>
        /// <param name="chapter">章节id</param>
        /// <returns></returns>
        public List<uint> GetBoxList(uint chapter)
        {
            List<uint> l = new List<uint>();
            foreach (ChapterEventInfo v in ListChapterEvent) {
                if (v.CharterID == chapter) {
                    if (v.ListBoxID != null && v.ListBoxID.Count > 0) {
                        l.AddRange(v.ListBoxID);
                    }
                }
            }
            return l;
        }
        /// <summary>
        /// 新增一场关卡胜利
        /// </summary>
        /// <param name="chapter">章节id</param>
        /// <param name="battleID">战斗id</param>
        public void AddWinBattle(uint chapter, uint battleID)
        {
            if (ListChapterEvent == null) {
                ListChapterEvent = new List<ChapterEventInfo>();
            }
            // 查询有没改章节的数据
            ChapterEventInfo v = FindAndAdd(chapter);
            if (v.ListWinBattleID.Contains(battleID) == false) {
                v.ListWinBattleID.Add(battleID);
            }
        }
        
        /// <summary>
        /// 新增一场开启战斗
        /// </summary>
        /// <param name="chapter">章节id</param>
        /// <param name="battleID">战斗id</param>
        public void AddEnterBattle(uint chapter, uint battleID)
        {
            if (ListChapterEvent == null) {
                ListChapterEvent = new List<ChapterEventInfo>();
            }
            // 查询有没改章节的数据
            ChapterEventInfo v = FindAndAdd(chapter);
            if (v.ListEnterBattleID.Contains(battleID) == false) {
                v.ListEnterBattleID.Add(battleID);
            }
        }
        
        /// <summary>
        /// 新增几个宝箱
        /// </summary>
        /// <param name="chapter">章节id</param>
        /// <param name="BoxID">宝箱id</param>
        public void AddBox(uint chapter, uint BoxID)
        {
            if (ListChapterEvent == null) {
                ListChapterEvent = new List<ChapterEventInfo>();
            }
            // 查询有没改章节的数据
            ChapterEventInfo v = FindAndAdd(chapter);
            if (v.ListBoxID.Contains(BoxID) == false) {
                v.ListBoxID.Add(BoxID);
            }
        }
        /// <summary>
        /// 查询某章节的数据，没有就添加上。
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        private ChapterEventInfo FindAndAdd(uint chapter)
        {
            foreach (ChapterEventInfo v in ListChapterEvent) {
                if (v.CharterID == chapter) {
                    return v;
                }
            }
            // 则添加。
            ChapterEventInfo c = new ChapterEventInfo(chapter);
            ListChapterEvent.Add(c);
            return c;
        }
    }
    /// <summary>
    /// 章节中的事件
    /// </summary>
    [Serializable]
    public class ChapterEventInfo
    {
        public uint CharterID = 0;
        public List<uint> ListWinBattleID = new List<uint>();
        public List<uint> ListEnterBattleID = new List<uint>();
        public List<uint> ListBoxID = new List<uint>();
        public ChapterEventInfo() { }
        public ChapterEventInfo(uint chapter)
        {
            this.CharterID = chapter;
        }
    }
}
