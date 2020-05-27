using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Helper;

public class DialogTest : MonoBehaviour
{
    public uint dialogID = 1010001;
    // Start is called before the first frame update
    void OnGUI()
    {
        if (GUI.Button(new Rect(300, 600, 300, 200), "剧情测试")) {
            TipsHelper.ShowDialog(dialogID, true);
        }
        if (GUI.Button(new Rect(300, 900, 300, 200), "预加载测试")) {
            PoolManager.PoolsPrepareData(100);
        }
    }
}
