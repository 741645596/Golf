using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchedulerProxy : Scheduler
{
    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
