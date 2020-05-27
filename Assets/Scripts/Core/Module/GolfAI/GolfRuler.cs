using UnityEngine;
# if UNITY_EDITOR
using UnityEditor;
#endif

public class GolfRuler : MonoBehaviour
{
    public bool IsShowRuler = false;
    public float step = 5.0f;
    public float gizmosSize = 0.2f;
    public float gizmosHeight = 0.0f;
    private Vector3 pos;
    private Vector2 m_AA;
    private Vector2 m_BB;

    private void Start()
    {
        pos = transform.position;
    }

    public void SetAABB(Vector2 AA, Vector2 BB)
    {
        m_AA = AA;
        m_BB = BB;
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (IsShowRuler == false)
            return;
        Gizmos.color = Color.red;

        for (float x = m_AA.x; x <= m_BB.x; x += step)
        {
            for (float y = m_AA.y; y <= m_BB.y; y += step)
            {
                Vector3 p = pos + pos + new Vector3(x, gizmosHeight, y);
                Gizmos.DrawSphere(p, gizmosSize);
                string str = "[" + x + "," + y + "]";
                Handles.Label(p, str);
            }
        }
    }
#endif
}
