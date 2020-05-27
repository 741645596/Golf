using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IggImageProxy : IggImage
{
#if UNITY_EDITOR
    protected override void Reset()
    {
    }
#endif
}
