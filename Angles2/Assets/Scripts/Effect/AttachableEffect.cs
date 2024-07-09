using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableEffect : ParticleEffect
{
    IAttachable _attachable;

    public override void ResetPosition(IAttachable attachable)
    {
        _attachable = attachable;
    }

    private void Update()
    {
        bool canAttach = _attachable.CanAttach();
        if (canAttach == false)
        {
            DestoryMe();
            return;
        }

        transform.position = _attachable.ReturnPosition();
    }
}
