using System.Collections.Generic;
using System;
using IGG.Core.Geom;

namespace IGG.Core {
    public class Util {


		/// <summary>
		/// 检测2个碰撞体是否发生碰撞
		/// </summary>
		public static bool CheckCollider(Int2 Start1 ,int w1, int h1, Int2 Start2, int w2, int h2){

			double disx = Start1.x + w1 * 0.5f - (Start2.x + w2 * 0.5f);
			disx = disx < 0 ? -disx : disx;


			double disy = Start1.y + h1 * 0.5f - (Start2.y + h2 * 0.5f);
			disy = disy < 0 ? -disy : disy;


			double LimtX = w1 * 0.5f + w2 * 0.5f;
			double LimtY = h1 * 0.5f + h2 * 0.5f;


			if (disx < LimtX && disy < LimtY)
				return true;
			else
				return false;
		}



    }
    /// <summary>
    /// List 扩展方法
    /// </summary>
    public static class ListExtensions {
        /// <summary>
        /// 获取下一个元素
        /// </summary>
        public static T NextOf<T>(this IList<T> list, T item) {
            return list[(list.IndexOf(item) + 1) == list.Count ? 0 : (list.IndexOf(item) + 1)];
        }
        public static T PrevOf<T>(this IList<T> list, T item) {
            return list[(list.IndexOf(item) - 1) < 0 ? list.Count - 1 : (list.IndexOf(item) - 1)];
        }

        /// <summary>
        /// 添加元素到第一个
        /// </summary>
        public static void AddToFront<T>(this List<T> list, T item) {
            if (list.Contains(item) == false) {
                list.Insert(0, item);
            }
        }
        /// <summary>
        /// 随机排序
        /// </summary>
        public static List<T> Shuffle<T>(this List<T> list) {
            System.Random random = new System.Random();
            List<T> newList = new List<T>();

            foreach (T item in list) {
                newList.Insert(random.Next(newList.Count), item);
            }

            return newList;
        }
    }
}
