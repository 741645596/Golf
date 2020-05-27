using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CanvasEnum {
    ESceneItemCanvas,
    ENormalWndCanvas,
    EMenuWndCanvas,
    EModelDialogCanvas,
    EAll,
}
public class UINode : Node
{

    public Canvas SceneItemCanvas;
    public Canvas NormalWndCanvas;
    public Canvas MenuWndCanvas;
    public Canvas ModelDialogCanvas;
    public Canvas WorldUIItemCanvas;
    public Camera UICamera;
    private Transform tParentWnd;
    private Transform tParentMenu;
    private Transform tParentDialog;
    
    
    
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    
    void Start()
    {
        tParentWnd = NormalWndCanvas.GetComponent<Transform>();
        tParentMenu = MenuWndCanvas.GetComponent<Transform>();
        tParentDialog = ModelDialogCanvas.GetComponent<Transform>();
        WndManager.SetUINode(this);
    }
    // <summary>
    // 根据类型获取窗口挂靠节点
    // </summary>
    // <returns></returns>
    public Transform GetWndParent(WndType type)
    {
        if (type == WndType.MenuWnd) {
            return tParentMenu;
        } else if (type == WndType.DialogWnd) {
            return tParentDialog;
        } else {
            return tParentWnd;
        }
    }
    
    
    // <summary>
    // 根据类型获取窗口挂靠节点
    // </summary>
    // <returns></returns>
    public Transform GetWndParent(WndBase wnd)
    {
        if (wnd == null) {
            return null;
        }
        return GetWndParent(wnd.GetWndType());
    }
    
    
    // <summary>
    // 清理所有子节点。
    // </summary>
    public void ClearChild()
    {
        List<GameObject> l = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++) {
            l.Add(transform.GetChild(i).gameObject);
        }
        
        foreach (GameObject g in l) {
            GameObject.DestroyImmediate(g);
        }
    }
    
    /// <summary>
    /// 移动子对象为最后一个
    /// </summary>
    /// <returns></returns>
    public void MoveChild2Last(WndBase Wnd)
    {
        if (Wnd == null || Wnd.tWnd == null) {
            return;
        }
        
        Transform tParent = GetWndParent(Wnd);
        if (tParent == null) {
            return;
        }
        
        bool bChild = false;
        for (int i = 0; i < tParent.childCount; i++) {
            if (tParent.GetChild(i) == Wnd.tWnd) {
                bChild = true;
            }
        }
        if (bChild == true) {
            Wnd.tWnd.SetAsLastSibling();
        }
    }
    
    /// <summary>
    /// 移动子对象为最前一个
    /// </summary>
    /// <returns></returns>
    public void MoveChild2First(WndBase Wnd)
    {
        if (Wnd == null || Wnd.tWnd == null) {
            return;
        }
        
        Transform tParent = GetWndParent(Wnd);
        if (tParent == null) {
            return;
        }
        
        bool bChild = false;
        for (int i = 0; i < tParent.childCount; i++) {
            if (tParent.GetChild(i) == Wnd.tWnd) {
                bChild = true;
            }
        }
        if (bChild == true) {
            Wnd.tWnd.SetAsFirstSibling();
        }
    }
    
    public bool CheckTopWnd(WndBase Wnd)
    {
        if (Wnd == null) {
            return false;
        }
        
        WndType wndtype = Wnd.GetWndType();
        
        
        if (wndtype == WndType.DialogWnd || wndtype == WndType.MenuWnd) {
            return true;
        }
        
        if (tParentDialog.childCount > 0) {
            return false;
        }
        
        if (tParentWnd.childCount == 0) {
            return false;
        } else if (tParentWnd.GetChild(tParentWnd.childCount - 1) == Wnd.transform) {
            return true;
        } else {
            return false;
        }
    }
    
    public void SetWndView(CanvasEnum type, bool b)
    {
        switch (type) {
            case CanvasEnum.ESceneItemCanvas: {
                if (null != SceneItemCanvas) {
                    SceneItemCanvas.gameObject.SetActive(b);
                }
            }
            break;
            case CanvasEnum.ENormalWndCanvas: {
                if (null != tParentWnd) {
                    tParentWnd.gameObject.SetActive(b);
                }
            }
            break;
            case CanvasEnum.EMenuWndCanvas: {
                if (null != tParentMenu) {
                    tParentMenu.gameObject.SetActive(b);
                }
            }
            break;
            case CanvasEnum.EModelDialogCanvas: {
                if (null != tParentDialog) {
                    tParentDialog.gameObject.SetActive(b);
                }
            }
            break;
            case CanvasEnum.EAll: {
                SetWndView(CanvasEnum.ESceneItemCanvas, b);
                SetWndView(CanvasEnum.ENormalWndCanvas, b);
                SetWndView(CanvasEnum.EMenuWndCanvas, b);
                SetWndView(CanvasEnum.EModelDialogCanvas, b);
            }
            break;
            default:
                break;
        }
    }
    
    void OnDestroy()
    {
        WndManager.SetUINode(null);
    }
}

