using IGG.Core;
using IGG.Core.Data.Config;
using IGG.Core.Data.DataCenter.GolfAI;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Data.DataCenter.Battle;
using IGG.Core.Manger.Coroutine;
using System.Collections;

public class GolfBall : MonoBehaviour
{
    /// <summary>
    /// 显示用的小球物体
    /// </summary>
    public GameObject ShowBall = null;

    public Vector3 GetBallPos()
    {
        return transform.position;
    }

    public void SetBallPos(Vector3 pos)
    {
        transform.position = pos;
    }

    private void Start()
    {
        RegisterHooks();
        ShowBall = transform.Find("Ball").gameObject;
        EventCenter.DispatchEvent(EventCenterType.Battle_LoadBallFinish, -1, transform);
    }

    private void RegisterHooks()
    {
        EventCenter.RegisterHooks(EventCenterType.Battle_BattingFinish, BattingFinish);
    }

    private void AntiRegisterHooks()
    {
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_BattingFinish, BattingFinish);
    }


    public void RunBall(GolfPathInfo pathinfo)
    {
        StartCoroutine(RunBallCoroutine(pathinfo));
    }

    IEnumerator RunBallCoroutine(GolfPathInfo pathinfo)
    {
        if (pathinfo == null || pathinfo.ListPt == null || pathinfo.ListPt.Count == 0)
            yield return null;
        for (int i = 0; i < pathinfo.ListPt.Count; i++)
        {
            GolfPathPoint golfPoint = pathinfo.ListPt[i];
            if (golfPoint.Status == BallRunStatus.Roll)
            {
                if (golfPoint.BallType == BallType.OutHole)
                {
                    yield return Yielders.GetWaitForSeconds(golfPoint.Interval / 2);
                }
                else
                {
                    yield return Yielders.GetWaitForSeconds(golfPoint.Interval / 0.5f);
                }
            }
            else
            {
                yield return Yielders.GetWaitForSeconds(golfPoint.Interval / 3);
            }
            if (!string.IsNullOrEmpty(golfPoint.EventType))
            {
                EventCenter.DispatchEvent(golfPoint.EventType, -1, golfPoint.EventParam);
            }
            SetBallPos(golfPoint.Position);
            BattleM.SetBallRunInfo(golfPoint.Position, golfPoint.AType, golfPoint.Status);
        }
        StopAllCoroutines();
    }


    private void OnDestroy()
    {
        AntiRegisterHooks();
    }

    private void BattingFinish(int Event_Send, object Param)
    {
        GolfPathInfo pathinfo = Param as GolfPathInfo;
        RunBall(pathinfo);
    }
}
