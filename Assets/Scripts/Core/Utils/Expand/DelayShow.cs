using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayShow : MonoBehaviour
{
    public float DelayTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        SetChildActive(false);
        StartCoroutine(DelayAction(DelayTime, () =>
        {
            SetChildActive(true);
        }));
    }

    private void SetChildActive(bool bActive)
    {
        int count = this.transform.childCount;
        for (int i=0; i< count; ++i)
        {
            Transform tm = this.transform.GetChild(i);
            if (null != tm)
            {
                if (null != tm.gameObject)
                {
                    tm.gameObject.SetActive(bActive);
                }
            }
        }
        
    }

    IEnumerator DelayAction(float dTime, System.Action callback)
    {
        yield return new WaitForSeconds(dTime);
        callback();
    }
}
