using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ButtonClickEventStatus : MonoBehaviour {
    private Vector3 v3Down = new Vector3();
    private Vector3 v3Up = new Vector3();
    public delegate void VoidDelegate();
    public delegate void VoidDelegateParam(GameObject go);
    public VoidDelegateParam onPlayerClickParam;
    public VoidDelegate onPlayerClick;
    public VoidDelegate onPlayerDown;
    public VoidDelegate onPlayerUp;
    public VoidDelegate onScrollMove;
    public VoidDelegate onPointerExit;
    public VoidDelegate onPointerEnter;
    private float fScale = 0.97f;
    Rect vcBoxBound = new Rect();
    bool bClick = false;
    public string SoundName;
    public bool m_bDrag = false;
    public bool CanPressShow = true;
    // Use this for initialization
    void Start () {
        EventTriggerListener.Get(gameObject).onClick = onClick;
        EventTriggerListener.Get(gameObject).onDown = onDown;
        EventTriggerListener.Get(gameObject).onEnter = onEnter;
        EventTriggerListener.Get(gameObject).onExit = onExit;
        EventTriggerListener.Get(gameObject).onUp = onUp;

        if (m_bDrag) {
            EventTriggerListenerDrag.Get(gameObject).onOBDrag = OnDrag;
            EventTriggerListenerDrag.Get(gameObject).onOBEndDrag = OnEndDrag;
        }
    

        v3Up = gameObject.transform.localScale;
        v3Down = fScale * gameObject.transform.localScale;
        if (fScale < 1.0f) {
            Vector2 vecTemp = transform.GetComponent<RectTransform>().sizeDelta * transform.localScale.x * (1.0f - fScale);
            vcBoxBound.width = vecTemp.x * 2;
            vcBoxBound.height = vecTemp.y * 2;
        }
    }

    // Update is called once per frame
    void Update () {
      
    }


    public void OnDrag(GameObject obj) {
        m_bDrag = true;
        if (onScrollMove != null) onScrollMove();
    }

    public void OnEndDrag(GameObject obj) {
        m_bDrag = false;
    }

    public void onClick(GameObject obj) {
        if (!bClick) {
            if (onPlayerClick != null) onPlayerClick();
            if (onPlayerClickParam != null) onPlayerClickParam(obj);
        }

        bClick = false;
    }
    public void onDown(GameObject obj) {
        if (null != onPlayerDown) {
            onPlayerDown();
        }
        if (CanPressShow) {
            gameObject.transform.localScale = v3Down;
            vcBoxBound.x = Input.mousePosition.x - vcBoxBound.width / 2;
            vcBoxBound.y = Input.mousePosition.y + vcBoxBound.height / 2;
        }
    }
    public void onEnter(GameObject obj) {
        if (null != onPointerEnter) {
            onPointerEnter();
        }
    }

    public void onExit(GameObject obj) {
        if (null != onPointerExit) {
            onPointerExit();
        }
    }

    public void onUp(GameObject obj) {
        if (null != onPlayerUp) {
            onPlayerUp();
        }

        gameObject.transform.localScale = v3Up;
//     
//             if (vcBoxBound.x <= Input.mousePosition.x 
//                 && vcBoxBound.y >= Input.mousePosition.y 
//                 && vcBoxBound.x + vcBoxBound.width >= Input.mousePosition.x 
//                 && vcBoxBound.y - vcBoxBound.height <= Input.mousePosition.y) {
//                 if (onPlayerClick != null) onPlayerClick();
//                 bClick = true;
//             }
        }
}
