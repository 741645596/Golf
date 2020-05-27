using System.Collections.Generic;
using UnityEngine;

public class CommonFunc {

    public static Transform FindChild(Transform trans, string goName)
    {
        Transform child = trans.Find(goName);
        if (child != null)
            return child;

        Transform go = null;
        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child, goName);
            if (go != null)
                return go;
        }

        return null;
    }

    public static T FindChild<T>(Transform trans, string goName) where T : Object
    {
        Transform child = trans.Find(goName);
        if (child != null)
        {
            return child.GetComponent<T>();
        }

        Transform go = null;
        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child, goName);
            if (go != null)
            {
                return go.GetComponent<T>();
            }
        }
        return null;
    }

    public static List<T> RandomSortList<T>(List<T> ListT) {
        System.Random random = new System.Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT) {
            newList.Insert(random.Next(newList.Count + 1), item);
        }

        return newList;
    }

    public static List<int> GetRandomList(int Value, int Num, int FrameCount = 30) {
        List<int> list = new List<int>();
        bool bLow = false;
        if (0 > Num) {
            bLow = true;
            Num = 0 - Num;
        }

        if (Num < FrameCount) {
            for (int i = 0; i <= Num; i++) {
                list.Add(i);
            }
        } else {

            int nTemp = Num / FrameCount;
            int nTemp1 = 0;
            for (int i = 1; i <= FrameCount; i++) {
                System.Random random = new System.Random((i * 1000) / 3);
                nTemp1 = (nTemp * i - random.Next(0, 10)) > 0 ? (nTemp * i - random.Next(0, 10)) : 0;
                if (nTemp1 > Num) {
                    nTemp1 = Num;
                }

                list.Add(nTemp1);
            }

            list.Add(Num);
        }

        list.Sort((int a, int b) => { return a >= b ? 1 : -1; });

        for (int i = 0; i < list.Count; i++) {
            list[i] = bLow ? Value - list[i] : Value + list[i];
            if (0 > list[i]) {
                list[i] = 0;
            }
        }

        return list;
    }


}

