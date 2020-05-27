using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using IGG.Core.Manger.Sound;
using IGG.Core.Manger.Coroutine;
using IGG.Core.Data.Config;
using UnityEngine.EventSystems;
using IGG.Core;
using DG.Tweening;
using IGG.Core.Data.DataCenter.Battle;
using IGG.Core.Data.DataCenter.GolfAI;

public class GolfDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject m_golf;
    public GameObject m_GolfboxOut;
    public GameObject m_GolfboxIn;
    private GameObject m_cloneBall;
    public GameObject m_arrow;

    private GameObject m_move;
    public GameObject m_arrowGreen;
    public GameObject m_arrowGreenMove;
    public GameObject m_arrowGreenMoveBox;

    private Vector3 m_boxPos = Vector3.zero;
    private Vector3 m_golfPos = Vector3.zero;
    private Vector3 m_dragGolfPos = Vector3.zero;
    private Vector3 m_oldVec = Vector3.zero;

    private Camera m_uiCamera;

    private Color m_colorGolfOut;
    private Color m_colorGolfIn;

    private Image m_imgGolfOut;
    private Image m_imgGolfIn;

    public int m_limitAngle;
    private float m_ballAngle;
    private float m_greenAngle;
    private float m_disGolfBox;

    float m_moveGolfDis;
    float m_dragPower;
    public BattingParam m_swingInfo;

    public GameObject m_backGround;
    public GameObject m_ring;
    private bool m_isDrag;
    private bool m_isRotateLeft = true;
    private float speed = 0.5f;
    /// <summary>
    /// 初始化窗口
    /// </summary>
    public void Start()
    {
        m_swingInfo = new BattingParam();
        m_limitAngle = 45;
        if (m_backGround != null && m_ring != null && m_arrow != null)
        {
            m_backGround.gameObject.SetActive(false);
            m_ring.gameObject.SetActive(false);
            m_arrow.gameObject.SetActive(false);
            m_arrowGreen.gameObject.SetActive(false);
            m_arrowGreenMove.gameObject.SetActive(false);
        }
        if (m_GolfboxOut != null && m_GolfboxIn != null)
        {
            m_imgGolfOut = m_GolfboxOut.GetComponent<Image>();
            m_imgGolfIn = m_GolfboxIn.GetComponent<Image>();
            if (m_imgGolfOut != null && m_imgGolfIn != null)
            {
                m_colorGolfOut = new Color(m_imgGolfOut.color.r, m_imgGolfOut.color.g, m_imgGolfOut.color.b, 0);
                m_colorGolfIn = new Color(m_imgGolfIn.color.r, m_imgGolfIn.color.g, m_imgGolfIn.color.b, 0);
                m_imgGolfOut.color = new Color(m_colorGolfOut.r, m_colorGolfOut.g, m_colorGolfOut.b, 0);
                m_imgGolfIn.color = new Color(m_colorGolfIn.r, m_colorGolfIn.g, m_colorGolfIn.b, 0);
            }
        }
        GameObject uiCamera = GameObject.Find("UI/UICamera");
        if (uiCamera != null)
        {
            this.m_uiCamera = uiCamera.GetComponent<Camera>();

        }

        if (m_GolfboxOut != null && m_golfPos != null && m_GolfboxOut != null && m_golf != null&& m_imgGolfIn!=null)
        {
            m_boxPos = m_GolfboxOut.transform.position;
            m_golfPos = m_golf.transform.position;
            m_disGolfBox = Vector3.Distance(m_golf.transform.position, m_boxPos);
            if (BattleM.BallInAra == AreaType.PuttingGreen)
            {
                m_GolfboxOut.gameObject.SetActive(false);
                m_imgGolfIn.gameObject.SetActive(false);
            }
        }
    }

    private void BoxScale()
    {
        if (m_GolfboxOut != null && m_cloneBall != null)
        {
            float num = (m_disGolfBox - Vector3.Distance(m_dragGolfPos, m_GolfboxOut.transform.position)) / m_disGolfBox;
            float dis = Vector3.Distance(m_golf.transform.position, m_boxPos);

            // 高尔夫框旋转
            if (m_disGolfBox >= dis)
            {
                if (CheckDis(num))
                {
                    m_GolfboxOut.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    float sca = 2 - num;
                    m_GolfboxOut.transform.localScale = new Vector3(sca, sca, sca);
                }
            }
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        m_isDrag = true;
        m_dragGolfPos = m_golf.transform.position;

        if (m_golf != null && m_GolfboxOut != null && m_uiCamera != null)
        {
            if (m_cloneBall == null)
            {
                ResourceManger.LoadWndItem("Btn_ballItem", m_golf.gameObject.transform, false, (obj) =>
                {
                    if (null == obj)
                    {
                        return;
                    }
                    m_cloneBall = obj;
                    obj.transform.SetParent(m_golf.transform.parent);
                    obj.transform.position = m_golfPos;
                    obj.transform.localScale = new Vector3(1, 1, 1);
                });
            }
            else
            {
                m_cloneBall.SetActive(true);
            }
            Vector3 globalMousePos;
            Vector3 mousePos;
            float z;
            //
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_GolfboxOut.GetComponentInParent<RectTransform>(), new Vector2(Input.mousePosition.x, Input.mousePosition.y),
            m_uiCamera, out mousePos))
            {
                if (mousePos.x > m_GolfboxOut.GetComponent<RectTransform>().position.x)
                {

                    z = Vector3.Angle(Vector3.down, m_golfPos - mousePos) - 180;
                }
                else
                {
                    z = 180 - Vector3.Angle(Vector3.down, m_golfPos - mousePos);
                }

                //球跟随鼠标
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_golf.GetComponent<RectTransform>(), Input.mousePosition,
               m_uiCamera, out globalMousePos))
                {
                    Vector3 compare = Vector3.zero;
                    if (z < m_limitAngle && z > -m_limitAngle)
                    {
                        m_golf.GetComponent<RectTransform>().position = globalMousePos;
                    }
                    else
                    {
                        if (globalMousePos.y < m_golfPos.y)
                        {
                            if (m_limitAngle < 90)
                            {
                                m_moveGolfDis = Vector3.Distance(new Vector3(m_golfPos.x, globalMousePos.y, m_golfPos.z), m_golfPos) * Mathf.Tan(m_limitAngle * Mathf.Deg2Rad);
                                if (compare != globalMousePos)
                                {
                                    if (globalMousePos.x > m_golfPos.x)
                                    {
                                        m_golf.GetComponent<RectTransform>().position = new Vector3(m_golfPos.x + m_moveGolfDis, globalMousePos.y, globalMousePos.z);
                                    }
                                    else
                                    {
                                        m_golf.GetComponent<RectTransform>().position = new Vector3(m_golfPos.x - m_moveGolfDis, globalMousePos.y, globalMousePos.z);
                                    }
                                }
                            }
                        }
                    }
                    compare = globalMousePos;
                }
                float dragDis = Vector3.Distance(new Vector3(m_golfPos.x, m_dragGolfPos.y, m_golfPos.z), m_golfPos) / m_disGolfBox;
                m_dragPower = dragDis;
                float num = ((m_disGolfBox - Vector3.Distance(new Vector3(m_golfPos.x, m_dragGolfPos.y, m_golfPos.z), m_GolfboxOut.transform.position)) / m_disGolfBox);
                float dis = CheckDis(num) ? 1 : num;
                Vector3 golfStraight = new Vector3(m_golfPos.x, m_dragGolfPos.y, m_golfPos.z);
                if (Vector3.Distance(golfStraight, m_golfPos) <= m_disGolfBox)
                {
                    m_imgGolfOut.color = new Color(m_imgGolfOut.color.r, m_imgGolfOut.color.g, m_imgGolfOut.color.b, dis);
                    m_imgGolfIn.color = new Color(m_imgGolfIn.color.r, m_imgGolfIn.color.g, m_imgGolfIn.color.b, dis);
                }
                else
                {

                    m_imgGolfOut.color = new Color((133 + 122 * (1 - dis)) / 255, (250 - 60 * (1 - dis)) / 255, (239 - 239 * (1 - dis)) / 255, 1);
                    m_imgGolfIn.color = new Color((133 + 122 * (1 - dis)) / 255, (250 - 60 * (1 - dis)) / 255, (239 - 239 * (1 - dis)) / 255, 1);
                }
                m_greenAngle = Mathf.Clamp(z, -m_limitAngle, m_limitAngle);
                m_ballAngle = Mathf.Clamp(CheckAngle(z), -m_limitAngle, m_limitAngle);
                m_GolfboxOut.transform.localRotation = Quaternion.Euler(0, 0, m_ballAngle);
                m_GolfboxIn.transform.localRotation = Quaternion.Euler(0, 0, m_ballAngle);

                if (BattleM.BallInAra == AreaType.PuttingGreen)
                {
                    m_arrowGreenMoveBox.transform.eulerAngles = new Vector3(0, 0, -m_greenAngle);
                }

            }
            BoxScale();
        }

    }
    public bool CheckDis(float z)
    {


        if (z > 0.96)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    public float CheckAngle(float z)
    {
        if (z < 5 && z > -5)
        {
            return 0;
        }
        else
        {
            return z;
        }
    }
    float rotate;
    BattleOperateInfo self = new BattleOperateInfo();
    void Update()
    {
        if (isDrag)
        {
            if (m_GolfboxOut != null && m_GolfboxIn != null && m_golf != null)
            {
                m_golf.transform.position = movingValue;
                m_GolfboxOut.transform.localScale = new Vector3(outScale, outScale, outScale);
                m_GolfboxIn.transform.localScale = new Vector3(inScale, inScale, inScale);
            }

        }
        if (m_isDrag && m_dragPower > 0.3)
        {
            if (m_backGround.gameObject.activeSelf != true)
            {
                if (m_backGround != null && m_ring != null && m_arrow != null)
                {
                    if (BattleM.BallInAra == AreaType.PuttingGreen)
                    {
                        m_move = m_arrowGreenMove;
                        m_backGround.gameObject.SetActive(false);
                        m_ring.gameObject.SetActive(false);
                        m_arrow.gameObject.SetActive(false);
                        m_arrowGreen.gameObject.SetActive(true);
                        m_arrowGreenMove.gameObject.SetActive(true);
                    }
                    else
                    {
                        m_move = m_arrow;
                        m_backGround.gameObject.SetActive(true);
                        m_ring.gameObject.SetActive(true);
                        m_arrow.gameObject.SetActive(true);
                        m_arrowGreen.gameObject.SetActive(false);
                        m_arrowGreenMove.gameObject.SetActive(false);
                    }

                }
            }


            float moveSpeed = 1;
            if (m_dragPower <= 1.1)
            {
                moveSpeed = 1;
            }
            else
            {
                moveSpeed *= m_dragPower * 2;
            }
            rotate += (m_isRotateLeft ? 100 : -100) * Time.deltaTime * speed * moveSpeed;
            if (rotate > 40)
            {
                m_isRotateLeft = false;
            }
            else if (rotate < -40)
            {
                m_isRotateLeft = true;
            }
            m_move.transform.localEulerAngles = new Vector3(0, 0, 180 + rotate);
            if (m_move == m_arrowGreenMove)
            {
                if (m_swingInfo != null)
                {
                    float offset = (m_move.transform.eulerAngles.z - 180 + 40) / 80 * 100;
                    if (offset < 0)
                    {
                        offset = 0;
                    }
                    else if (offset > 99)
                    {
                        offset = 100;
                    }

                    m_swingInfo.CurAccuracyOffset = offset - 100;
                    
                   
                    self = BattleM.SelfSel;
                    m_swingInfo.Ball = self.Ball;
                    m_swingInfo.Club = self.Club;
                   
                    ClubparameterConfig  clubConfig= ClubparameterDao.Inst.GetCfg((uint)self.Club.Type);
                    if (clubConfig != null)
                    { m_swingInfo.BattingSpeed = BattleM.GetRollSpeedDir() * clubConfig.MinDistance * m_dragPower; }
                   
                    m_swingInfo.CurHookAngle = m_greenAngle;
                    //m_swingInfo.WindSpeed ;
                   
                        m_swingInfo.CurFBSpin = 0;
                        m_swingInfo.CurLRSpin = 0;

                    BattleM.SetRollOperation(m_swingInfo);
                }
            }
            else
            {

                m_move.GetComponent<RectTransform>().localScale = new Vector3(2, 0.7f + m_dragPower * 0.3f, 1);
            }
            // Debug.Log(m_arrow.transform.eulerAngles.z);
            //if(m_arrow.transform.eulerAngles.z > 179.9&& m_arrow.transform.eulerAngles.z < 180.1 )
            //{
            //    Debug.Log("1");

        }
        else
        {
            if (m_arrow != null)
            {

                m_arrow.transform.eulerAngles = new Vector3(0, 0, 180);
                m_arrow.GetComponent<RectTransform>().localScale = new Vector3(2, 0.7f, 1);
            }
            if (m_backGround != null && m_ring != null && m_arrow != null)
            {
                m_backGround.gameObject.SetActive(false);
                m_ring.gameObject.SetActive(false);
                m_arrow.gameObject.SetActive(false);
                m_arrowGreen.gameObject.SetActive(false);
                m_arrowGreenMove.gameObject.SetActive(false);
            }

        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        m_isDrag = false;
        if (m_dragPower > 0.3)
        {
            Swing();
        }
        else if (m_golf != null && m_cloneBall != null && m_GolfboxOut != null)
        {

            m_imgGolfOut.color = m_colorGolfIn;
            m_imgGolfIn.color = m_colorGolfOut;
            m_cloneBall.SetActive(false);

            m_golf.GetComponent<RectTransform>().position = m_golfPos;

            m_GolfboxOut.transform.localScale = new Vector3(1, 1, 1);
            Quaternion boxAngels = Quaternion.Euler(0, 0, 0);
            m_GolfboxOut.transform.localRotation = boxAngels;
            m_GolfboxIn.transform.localRotation = boxAngels;
        }

    }

    bool isDrag = false;
    Vector3 movingValue = Vector3.zero;
    float outScale;
    float inScale;
    float dragDis;
    private void Swing()
    {
        self = BattleM.SelfSel;
        var varWnd = WndManager.FindWnd<MineBallSpinWnd>();
        if (m_swingInfo != null)
        {
            float offset = (m_move.transform.localEulerAngles.z - 180 + 40) / 80 * 100;
            if (offset < 0)
            {
                offset = 0;
            }
            else if (offset > 99)
            {
                offset = 100;
            }
            ClubparameterConfig clubConfig = ClubparameterDao.Inst.GetCfg((uint)self.Club.Type);
            if (BattleM.BallInAra == AreaType.PuttingGreen)
            {
                m_swingInfo.CurAccuracyOffset = 0;
                if (clubConfig != null)
                { m_swingInfo.BattingSpeed = MathUtil.RotateRound(BattleM.GetRollSpeedDir() * clubConfig.MinDistance, m_dragPower, m_ballAngle); }
                m_swingInfo.CurHookAngle = m_greenAngle;
            }
            else
            {
                m_swingInfo.CurHookAngle = m_ballAngle;
                m_swingInfo.CurAccuracyOffset = 50 - offset;
                m_swingInfo.BattingSpeed = self.NeedSpeed * m_dragPower;
            }
                

            m_swingInfo.Ball = self.Ball;
            m_swingInfo.Club = self.Club;

           
           
           
            
           
            //m_swingInfo.WindSpeed ;
            if (varWnd != null)
            {
                m_swingInfo.CurFBSpin = varWnd.m_CurFBSpin;
                //self.Club.BackSpin = varWnd.m_underSpin;
                m_swingInfo.CurLRSpin = varWnd.m_LRspin;

                //Debug.Log(m_swingInfo.m_dragAngle+".."+ m_swingInfo.m_downSpin+".."+ m_swingInfo.m_upSpin+"..." + m_swingInfo.m_leftSpin+"..."+ m_swingInfo.m_rightSpin);
                // Destroy(varWnd);
            }
            else
            {
                m_swingInfo.CurFBSpin = 0;
                m_swingInfo.CurLRSpin = 0;
            }

            movingValue = m_dragGolfPos;
            dragDis = Vector3.Distance(m_golfPos, m_dragGolfPos);
            if (m_GolfboxOut != null && m_GolfboxIn != null)
            {
                outScale = m_GolfboxOut.transform.localScale.x;
                inScale = m_GolfboxIn.transform.localScale.x;
                DOTween.To(() => outScale, x => outScale = x, 0.8f, 0.1f).OnComplete(() =>
                {
                    m_GolfboxOut.SetActive(false);
                });
                DOTween.To(() => inScale, x => inScale = x, 0.6f, 0.1f).OnComplete(() =>
                {
                    m_GolfboxIn.SetActive(false);
                });
            }
            DOTween.To(() => movingValue, x => movingValue = x, m_golfPos, 0.1f);

            isDrag = true;
            WndManager.DestoryWnd<SwingModeWnd>(0.2f);
            WndManager.DestoryWnd<BattleMainWnd>(0.2f);
        }
        BattleM.SetBattleStatus(BattleStatus.Round_FireBall, m_swingInfo);
    }
}

