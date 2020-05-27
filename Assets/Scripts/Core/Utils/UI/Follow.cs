using DG.Tweening;
using IGG.Core.Geom;
using UnityEngine;
using UnityEngine.UI;

public class Follow : MonoBehaviour
{
    protected Transform m_FollowTF = null;
    //protected RectTransform m_CtrlRT = null;
    protected bool m_IsMove = false;

    protected Vector2 m_CanvasSizeDelta = Vector2.one;

    protected Int2 m_TargetPos;
    protected float m_OffsetX;
    protected float m_OffsetY;

    protected virtual Vector3 TargetWorldPos
    {
        get
        {
            Vector3 position = m_FollowTF.position;
            position.y += m_OffsetY;

            return position;
        }
    }

    public void SetFollowData(Transform followTF, RectTransform CanvasRT, float offsetY, Int2 targetPos, float offsetX)
    {
        gameObject.SetActive(true);
        m_FollowTF = followTF;
        //m_CtrlRT = ctrlRT;
        if(null != CanvasRT)
        {
            m_CanvasSizeDelta = CanvasRT.gameObject.GetComponent<CanvasScaler>().referenceResolution;
            //m_CanvasSizeDelta = CanvasRT.sizeDelta;
        }

        m_TargetPos = targetPos;
        m_OffsetX = offsetX;
        m_OffsetY = offsetY;

        if (m_FollowTF != null)
        {
            m_IsMove = false;
            UpdatePos();
        }

        Init();
    }

    public virtual void Init()
    { }

    public virtual void Update()
    {
        UpdatePos();
    }

    public void ChangeFollowgo(Transform followTF)
    {
        m_FollowTF = followTF;
        m_IsMove = m_FollowTF != null;
    }
    
    public void UpdatePos()
    {
        if(null == m_FollowTF)
        {
            return;
        }
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(TargetWorldPos);
        Vector3 viewPortPos = Camera.main.ScreenToViewportPoint(screenPos);
        viewPortPos = new Vector3(viewPortPos.x * m_CanvasSizeDelta.x, 
            viewPortPos.y * m_CanvasSizeDelta.y, viewPortPos.z);

        if (m_IsMove)
        {
            float fDis = Vector3.Distance(transform.localPosition, viewPortPos);
            if (5 >= fDis)
            {
                m_IsMove = false;
            }
            else
            {
                Tweener tweener2 = transform.DOLocalMove(viewPortPos, 0.2f, false);
            }
        }
        else
        {
            transform.localPosition = viewPortPos;
        }
    }
}