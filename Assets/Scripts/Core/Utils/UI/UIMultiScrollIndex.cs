using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIMultiScrollIndex : MonoBehaviour {
    private UIMultiScroller _scroller;
    private int _index;

    void Start() {

    }

    public int Index {
        get { return _index; }
        set {
            _index = value;
            transform.localPosition = _scroller.GetPosition(_index);
            gameObject.name = "Scroll" + (_index < 10 ? "0" + _index : _index.ToString());
        }
    }

    public UIMultiScroller Scroller {
        set { _scroller = value; }
    }

    IEnumerator WaitDotween(float time, UIMultiScroller.Arrangement dir) {
        yield return new WaitForSeconds(time);
        FlyTrans.DOComplete();
        if (dir == UIMultiScroller.Arrangement.Vertical) {
            FlyTrans.DOLocalMoveY(0, 0.2f);
        } else {
            FlyTrans.DOLocalMoveX(0, 0.2f);
        }
    }

    public float FlyTime = 0.7f;
    public float SpringbackDistance = 50;
    public Transform FlyTrans;

    public void SetFlyIndex(int Num, int Len, UIMultiScroller.Arrangement dir) {
        if (null == FlyTrans) {
            return;
        }

        float Dist = 0.0f;
        float Time = FlyTime / (Num * Len) ;

        if (dir == UIMultiScroller.Arrangement.Vertical) {
            Dist = transform.GetComponent<RectTransform>().sizeDelta.y * Len;
            FlyTrans.localPosition = - new Vector3(0, Dist, 0);
            FlyTrans.DOLocalMoveY(SpringbackDistance, Time * (Index + 2));
        } else {
            Dist = transform.GetComponent<RectTransform>().sizeDelta.x * Len;
            FlyTrans.localPosition =  new Vector3(Dist, 0, 0);
            FlyTrans.DOLocalMoveX( - SpringbackDistance, Time * (Index + 2));
        }

        StartCoroutine(WaitDotween(Time * (Index + 2), dir));
        gameObject.name = "Scroll" + (_index < 10 ? "0" + _index : _index.ToString());
    }
}
