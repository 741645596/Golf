using IGG.Core;
using UnityEngine;
using IGG.Core.Data.DataCenter.Battle;

public class GolfRing : MonoBehaviour
{
    public RingDrag Drag;


    void Start()
    {
        RegisterHooks();
    }


    void OnDestroy()
    {
        AntiRegisterHooks();
    }


    private void RegisterHooks()
    {
        EventCenter.RegisterHooks(EventCenterType.Battle_UpdateRingPos, UpdatePos);
        EventCenter.RegisterHooks(EventCenterType.Battle_ShowRing, ShowRing);
        
    }

    private void AntiRegisterHooks()
    {
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_UpdateRingPos, UpdatePos);
        EventCenter.RegisterHooks(EventCenterType.Battle_ShowRing, ShowRing);
    }
    /// <summary>
    /// 设置环的位置
    /// </summary>
    /// <param name="Pos"></param>
    public void SetRingPos(Vector3 Pos)
    {
        transform.position = Pos;
        BattleM.RingPos = Pos;
    }

    private void UpdatePos(int Event_Send, object Param)
    {
        Vector3 pos = (Vector3)Param ;
        SetRingPos(pos);
    }


    private void ShowRing(int Event_Send, object Param)
    {
        bool isShow = (bool)Param;
        Drag.gameObject.SetActive(isShow);
    }
}


