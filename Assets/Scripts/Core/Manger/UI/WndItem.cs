using UnityEngine;
using UnityEngine.UI;
using IGG.Core.Helper;

public  class WndItem : MonoBehaviour
{

    public virtual void Awake()
    {
        Init();
    }
    
    public virtual void Start()
    {
        InitItem();
    }
    
    /// <summary>
    /// 用于加载完成item时的操作
    /// </summary>
    protected virtual void Init()
    {
    
    }
    
    /// <summary>
    /// 初始化item
    /// </summary>
    protected virtual void InitItem()
    {
    
    }
    
    
    /// <summary>
    /// 传数据给item
    /// </summary>
    /// <param name="data"></param>
    public virtual void SetData(object data)
    {
    
    }
    
    /// <summary>
    /// 传数据给item
    /// </summary>
    /// <param name="data">传递给item的数据</param>
    public virtual void SetData(object[] data)
    {
    
    }
    
    /// <summary>
    /// 设置绑定icon
    /// </summary>
    /// <param name="img">img对象</param>
    /// <param name="altas">icon所在的图集</param>
    /// <param name="spriteName">icon sprite 名称</param>
    protected void SetImage(Image img, string altas, string spriteName)
    {
        if (img != null) {
            ResourceManger.LoadSprite(altas, spriteName, false, (g) => {
                if (null != g) {
                    Sprite sprite = g as Sprite;
                    if (null != sprite) {
                        img.sprite = sprite;
                    }
                } else {
                    img.sprite = null;
                }
            });
        }
    }
}
