using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayEffectDestroy : MonoBehaviour
{
    public string ResType = ResourcesType.Effect;
    public float DelayTime;
    void Start()
    {
    
    }
    
    private void OnEnable()
    {
        StartCoroutine(DelayAction(DelayTime, () => {
        
            ResourceManger.FreeGo(this.gameObject);
        }));
    }
    
    IEnumerator DelayAction(float dTime, System.Action callback)
    {
        yield return new WaitForSeconds(dTime);
        callback();
    }
}
