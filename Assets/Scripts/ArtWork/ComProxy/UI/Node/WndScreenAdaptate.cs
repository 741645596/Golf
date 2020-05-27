using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WndScreenAdaptate : MonoBehaviour
{
    public float _StandardWidth = 0;
    public float _StandardHeight = 0;
    public CanvasScaler _CanvasScaler;
    // Use this for initialization
    
    void Start()
    {
        /*StandarRatio = new Vector3[RechangeForms.Length];
        float referenceRatio = 16f / 9f;
        float currentRatio = ((float)Screen.width / (float)Screen.height);
        for (int i = 0; i < RechangeForms.Length; i++)
        {
            StandarRatio[i] = RechangeForms[i].transform.localScale;
            float yFactor = StandarRatio[i].y * (referenceRatio / currentRatio);
            float posYFactor = RechangeForms[i].transform.position.y * (referenceRatio / currentRatio);
            RechangeForms[i].transform.DOScaleY(yFactor, 0.0f);
            RechangeForms[i].transform.DOMoveY(posYFactor, 0.0f);
        }*/
        ScreenAdaptate();
    }
    
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        ScreenAdaptate();
#endif
    }
    
    public void ScreenAdaptate()
    {
        if (_CanvasScaler == null) {
            return;
        }
        
        float standard_width = _StandardWidth;
        float standard_height = _StandardHeight;
        float device_width = 0f;
        float device_height = 0f;
        
        device_width = Screen.width;
        device_height = Screen.height;
        const float PRECISION = 0.000001f;
        if (standard_height <= PRECISION && standard_height >= -PRECISION ||
            device_height <= PRECISION && device_height >= -PRECISION) {
            return;
        }
        
        float standard_aspect = standard_width / standard_height;
        float device_aspect = device_width / device_height;
        
        if (device_aspect < standard_aspect) {
            _CanvasScaler.matchWidthOrHeight = 0;
        } else {
            _CanvasScaler.matchWidthOrHeight = 1;
        }
    }
}
