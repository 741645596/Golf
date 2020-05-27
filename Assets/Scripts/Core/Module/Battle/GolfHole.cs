using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core;

public class GolfHole : MonoBehaviour
{
    public MeshRenderer ren;
    public GameObject Qizhi;
    private bool m_isCross = false;
    // Start is called before the first frame update
    void Start()
    {
        ///mat.SetColor();
        RegisterHooks();
        WndManager.CreateWnd<FlagItemWnd>(WndType.DialogWnd, false, false, (wnd) =>
        {
            (wnd as FlagItemWnd).SetData(Qizhi.transform.position);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 注册消息函数
    private void RegisterHooks()
    {

        EventCenter.RegisterHooks(EventCenterType.Battle_PathCrossHole, SetHoleEffect);
    }

    // 反注册消息函数
    private void AntiRegisterHooks()
    {
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_PathCrossHole, SetHoleEffect);
    }

    private void OnDestroy()
    {
        AntiRegisterHooks();
    }


    private void SetHoleEffect(int sender, object Param)
    {
        bool isCross = (bool)Param;
        if (m_isCross == isCross)
            return;
        m_isCross = isCross;
        if (ren != null && ren.sharedMaterial != null)
        {
            ren.sharedMaterial.SetColor("_Color", m_isCross ? Color.blue : Color.red);
        }
    }
}
