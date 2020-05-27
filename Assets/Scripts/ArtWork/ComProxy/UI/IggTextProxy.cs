using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class IggTextProxy : IggText
{

#if UNITY_EDITOR
    void Reset()
    {
    }
#endif

    public override void OnValidate()
    {
        base.OnValidate();
    }
}

