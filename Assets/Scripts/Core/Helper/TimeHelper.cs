using System.Collections.Generic;

namespace IGG.Core.Helper
{
    /// <summary>
    /// Author  gaofan
    /// Date    2017.12.29
    /// Desc    时间相关工具
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// 通过秒得到时间
        /// 9002,{text1}天{text2}时
        /// 9003,{text1}时{text2}分
        /// 9004,{text1}分{text2}秒
        /// 9005,{text1}秒,{Text1}s
        /// 9023,{text1}时
        /// 9024,{text1}分
        /// 9025,{text1}天
        /// 9026,{text1}天{text2}时{text1}分
        /// </summary>
        /// <param name="nSec"></param>
        /// <returns></returns>
        public static string GetTimeBySeconds(int nSec, bool bZeroShow = true, bool bDHM = false)
        {
            string str;
            str = "";
            int day = nSec / (3600 * 24);
            int hrs = (nSec / 3600) % 24;
            int min = (nSec / 60) % 60;
            int sec = nSec % 60;
            List<string> list = new List<string>();
            if (0 < day)
            {
                list.Add(day.ToString());


                if (bDHM) {
                    list.Add(hrs.ToString());
                    list.Add(min.ToString());
                 //   str = MessageDao.Inst.GetText(9026, list);
                } else if(0 == hrs && min == 0 && sec == 0 && !bZeroShow) {
                 //   str = MessageDao.Inst.GetText(9025, list);
                } else {
                    list.Add(hrs.ToString());
                 //   str = MessageDao.Inst.GetText(9002, list);
                }
            }
            else if (0 < hrs)
            {
                list.Add(hrs.ToString());
                if (min == 0 && sec == 0 && !bZeroShow) {
                //    str = MessageDao.Inst.GetText(9023, list);
                } else {
                    list.Add(min.ToString());
                //    str = MessageDao.Inst.GetText(9003, list);
                }

            }
            else if (0 < min)
            {
                list.Add(min.ToString());
                if (0 == sec && !bZeroShow) {
                //    str = MessageDao.Inst.GetText(9024, list);
                } else {
                    list.Add(sec.ToString());
                //   str = MessageDao.Inst.GetText(9004, list);
                }
               
            }
            else
            {
                list.Add(sec.ToString());
                //str = MessageDao.Inst.GetText(9005, list);
            }
            return str;
        }

        /// <summary>
        /// 按 天:时:分:秒 格式给出
        /// eg: 1:23:24:45
        /// </summary>
        /// <param name="nSec"></param>
        /// <param name="bZeroShow"></param>
        /// <param name="bDHM"></param>
        /// <returns></returns>
        public static string GetTimeNumBySeconds(int nSec)
        {
            string str = "";
            int day = nSec / (3600 * 24);
            int hrs = (nSec / 3600) % 24;
            int min = (nSec / 60) % 60;
            int sec = nSec % 60;
            List<string> list = new List<string>();
            if (0 < day)
            {
                list.Add(day.ToString());
                list.Add(hrs.ToString());
                list.Add(min.ToString());
                list.Add(sec.ToString());
            }
            else if (0 < hrs)
            {
                list.Add(hrs.ToString());
                list.Add(min.ToString());
                list.Add(sec.ToString());
            }
            else if (0 < min)
            {
                list.Add(min.ToString());
                list.Add(sec.ToString());
            }
            else
            {
                list.Add(sec.ToString());
            }
            for (int i = 0; i < list.Count; i++)
            {
                str += list[i].ToString();
                if (i != list.Count - 1)
                {
                    str += ":";
                }
            }
            return str;
        }

        public static string GetTimeBySeconds(uint nSec, bool bZeroShow = false, bool bDHM = false)
        {
            return GetTimeBySeconds((int)nSec, bZeroShow, bDHM);
        }


        /// <summary>
        /// 天days 小时hrs 分钟mins 秒sceconds
        /// uTime = 60 or 119 输出 strTime = "1" strUnit = "分钟"
        /// 钟楼功能显示需要
        /// </summary>
        /// <param name="uTime">秒</param>
        /// <param name="strTime">时间</param>
        /// <param name="strUnit">单位</param>
        public static void GetTimeUnit(uint uTime, out string strTime, out string strUnit) {
            uint day = uTime / (3600 * 24);
            uint hrs = (uTime / 3600) % 24;
            uint min = (uTime / 60) % 60;
            uint sec = uTime % 60;
            strUnit = "";

            if (0 != day) {
                //strUnit = MessageDao.Inst.GetText(9033, "");
                strTime = day.ToString();
            } else if (0 != hrs) {
                //strUnit = MessageDao.Inst.GetText(9032, "");
                strTime = hrs.ToString();
            } else if (0 != min) {
                //strUnit = MessageDao.Inst.GetText(9031, "");
                strTime = min.ToString();
            } else {
                //strUnit = MessageDao.Inst.GetText(9005, "");
                strTime = sec.ToString();
            }
        }

        /// <summary>
        /// 显示多少时间前
        /// 比如 uTime = 70 1分钟前    uTime = 3655 一小时前
        /// </summary>
        /// <param name="uTime"></param>
        /// <returns></returns>
        public static string GetBeforeTime(int nTime) {
            uint uTime = nTime > 0 ? (uint)nTime : 0;
            string str = "";
            uint day = uTime / (3600 * 24);
            uint hrs = (uTime / 3600) % 24;
            uint min = (uTime / 60) % 60;
            //uint sec = uTime % 60;

            if (0 != day) {
                //str = MessageDao.Inst.GetText(9029, day.ToString());

            } else if (0 != hrs) {
                //str = MessageDao.Inst.GetText(9028, hrs.ToString());

            } else if (0 != min) {
                //str = MessageDao.Inst.GetText(9027, min.ToString());

            } else {
                //str = MessageDao.Inst.GetText(9030);
            }

            return str;
        }
    }
}