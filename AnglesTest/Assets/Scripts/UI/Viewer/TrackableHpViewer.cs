using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrackableHpViewer : RatioViewer
{
    IFollowable _followTarget;

    public void SetFollower(IFollowable followTarget)
    {
        _followTarget = followTarget;
    }

    private void Update()
    {
        bool canAttach = _followTarget.CanFollow();
        if (canAttach == true)
        {
            TurnOnViewer(true);
            Vector3 pos = _followTarget.GetPosition();
            transform.position = pos + (Vector3)_followTarget.BottomPoint;
        }
        else
        {
            TurnOnViewer(false);
            return;
        }
    }
}
