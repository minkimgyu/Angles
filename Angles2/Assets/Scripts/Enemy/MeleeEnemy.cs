using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    protected override void Update()
    {
        base.Update();
        ResetDirection();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveToDirection();
    }
}
