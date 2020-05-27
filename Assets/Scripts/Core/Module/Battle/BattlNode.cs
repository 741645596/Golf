using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGG.Core.Data.DataCenter.Battle;
using IGG.Core.Data.DataCenter.GolfAI;

public class BattlNode : Node
{
    void Awake()
    {
        BattleM.SetBattleNode(this);
    }

    /// <summary>
    /// 根据地图数据加载地图
    /// </summary>
    /// <param name="map"></param>
    public void LoadMap2Ball(GolfCourseMap map)
    {
        if (map == null)
            return;
        ResourceManger.LoadMap("GolfMap", transform, false, false, (g) =>
        {
            if (g != null)
            {
                GolfRuler ruler = g.AddComponent<GolfRuler>();
                ruler.SetAABB(map.AA2, map.BB2);
            }
        });
        ResourceManger.LoadBall("GolfBall", transform, false, false, (ballGo) =>
        {
            if (ballGo != null)
            {
                GolfBall ball = ballGo.GetComponent<GolfBall>();
                if (ball == null)
                {
                    ball = ballGo.AddComponent<GolfBall>();
                }
                if (ball != null)
                {
                    ball.SetBallPos(map.StartPos);
                }
            }
        });
    }

}
