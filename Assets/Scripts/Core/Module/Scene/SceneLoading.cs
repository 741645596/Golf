using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour, ILoading
{

    public GameObject Loadinggo = null;
    public Image ProgressImage;
    public Text ProgressText;
    public Canvas loadingCanvas;
    // Use this for initialization
    public virtual void Start()
    {
    }
    
    
    /// <summary>
    /// 播放准备加载的动画
    /// </summary>
    public  void Play(int Progress)
    {
        if (ProgressImage != null) {
            ProgressImage.fillAmount = Progress * 0.01f;
        }
        if (ProgressText != null) {
            IGGString.strBuilder.Append(Progress);
            IGGString.strBuilder.Append("%");
            ProgressText.text = IGGString.strBuilder.ToString();
            IGGString.strBuilder.Length = 0;
        }
    }
    
    public void PlayDownLoad(int Progress, string ResourceName)
    {
        if (ProgressImage != null) {
            ProgressImage.fillAmount = Progress * 0.01f;
        }
        if (ProgressText != null) {
            IGGString.strBuilder.Append(ResourceName);
            IGGString.strBuilder.Append(" donwnload ");
            IGGString.strBuilder.Append(Progress);
            IGGString.strBuilder.Append("%");
            ProgressText.text = IGGString.strBuilder.ToString();
            IGGString.strBuilder.Length = 0;
        }
    }
    
    
    public void Playzip(int Progress, string UnizipResourceName)
    {
        if (ProgressImage != null) {
            ProgressImage.fillAmount = Progress * 0.01f;
        }
        if (ProgressText != null) {
            IGGString.strBuilder.Append(UnizipResourceName);
            IGGString.strBuilder.Append(" unzip... ");
            ProgressText.text = IGGString.strBuilder.ToString();
            IGGString.strBuilder.Length = 0;
        }
    }
    
    /// <summary>
    /// 播放加载中的动画
    /// </summary>
    public  void Load()
    {
        loadingCanvas.enabled = true;
    }
    
    /// <summary>
    /// 播放结束后尝试回收loading资源
    /// </summary>
    public  void TryDestroy()
    {
        if (ProgressImage != null) {
            ProgressImage.fillAmount = 0;
        }
        loadingCanvas.enabled = false;
    }
    
}
