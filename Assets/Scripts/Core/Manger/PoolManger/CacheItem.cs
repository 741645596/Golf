using UnityEngine;
using System.Collections.Generic;

public class CacheItem : MonoBehaviour
{
    public Vector3 DefaultlocalPostion = Vector3.zero;
    public Quaternion DefaultlocalRotation = Quaternion.identity;
    public Vector3 DefaultlocalScale = Vector3.one;
    public string ResType;
    public string ResName;
    
    public void SetCacheKey(string resType, string resName)
    {
        this.ResName = resName;
        this.ResType = resType;
    }
    
    public virtual void OnRecycle()
    {
        transform.localScale = DefaultlocalScale;
        transform.position = DefaultlocalPostion;
        transform.localRotation = DefaultlocalRotation;
    }
}
