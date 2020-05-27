using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfRotate : MonoBehaviour
{

    [HideInInspector] public uint HeroId;
    [HideInInspector] public GameObject m_Carema;
    //是否被拖拽
    private bool isRotating = false;
    //旋转速度
    public float speed = 6f;
    //阻尼速度
    private float zSpeed;
    //鼠标沿水平方向拖拽的增量
    private float X;
    //鼠标沿竖直方向拖拽的增量
    private float Y;
    //鼠标移动的距离
    private float mXY;

    private GameObject heroPreObj;

    private bool IsStart = false;
    //接受鼠标按下的事件
    //void OnMouseDown()
    //{
    //    X = 0f;
    //    //Y = 0f;
    //}

    public void Init()
    {
        //int heroLayer = LayerMask.NameToLayer("HeroCamera");
        //string heropath = "" ;//HeroInfoDao.Inst.GetCfg(HeroId).ModelPath;
        //if (heropath != null)
        //{
        //GameObject heroObj = (GameObject)Resources.Load("Prefab/Hero/" + heropath, typeof(GameObject));
        
        if (m_Carema != null)
        {
         
            //Transform[] trans = m_Carema.GetComponentsInChildren<Transform>();
            heroPreObj = m_Carema.transform.GetChild(0).gameObject;
        }
            //if (trans.Length > 0)
            //{
            //    foreach (Transform child in trans)
            //    {
            //        child.gameObject.layer = heroLayer;
            //    }
            //}
            //heroPreObj = (GameObject)Instantiate((Object)heroObj, new Vector3(0, 0, 0), Quaternion.identity);


            //    Rigidbody heroRigiCom = heroPreObj.GetComponent<Rigidbody>();
            //    if (heroRigiCom != null)
            //        heroRigiCom.useGravity = false;
            //    if (m_Carema != null)
            //    {
            //        heroPreObj.transform.parent = m_Carema.transform;
            //        GameObject child = m_Carema.transform.GetChild(0).gameObject;
            //        if (child != null)
            //        {
            //            Vector3 position = child.transform.localPosition;
            //            Quaternion rotation = child.transform.localRotation;
            //            heroPreObj.transform.localPosition = position;
            //            heroPreObj.transform.localRotation = rotation;
            //            Destroy(child);
            //        }
            //    }
            
       // }
    }

    public void LongPress(bool bStart)
    {
        IsStart = bStart;
    }
    // private bool isRotating;
    private void Update()
    {
        if (IsStart)
        {
            X = -Input.GetAxis("Mouse X");

            //获得鼠标增量

            //mXY = Mathf.Sqrt (X * X + Y * Y);
            //计算鼠标移动的长度
            // if(mXY == 0f){ mXY=1f;         }     }

            //计算鼠标移动的长度//
            //mXY = Mathf.Sqrt(X * X);
            //if (mXY == 0f)
            //{
            //    mXY = 1f;
            //}
           
            if (heroPreObj != null)
            {
                heroPreObj.transform.Rotate(new Vector3(0, X, 0) * RiSpeed(), Space.Self);
               
            }
            if(X!=0)
            {
                return;
            }
            Y = Input.GetAxis("Mouse Y");
            if (heroPreObj != null)
            {
               
                heroPreObj.transform.Rotate(new Vector3(Y, 0, 0) * RiSpeed(), Space.Self);
            }
        }
    }

    //获取阻尼速度
    float RiSpeed()
    {
        if (IsStart)
        {
            zSpeed = speed;
        }
        else
        {
            //if (zSpeed> 0)
            //{
            //通过除以鼠标移动长度实现拖拽越长速度减缓越慢
            //  zSpeed -= speed*2 * Time.deltaTime / mXY;
            //}
            //else
            //{
            zSpeed = 0;
            //}
        }
        return zSpeed;
    }
    private void OnDestroy()
    {
        if (m_Carema != null)
            Destroy(m_Carema);
    }

    public void Destory()
    {
        Destroy(this);
    }
}
