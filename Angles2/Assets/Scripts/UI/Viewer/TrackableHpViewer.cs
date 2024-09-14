using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrackableHpViewer : HpViewer
{
    [SerializeField] float _topOffset;

    IFollowable _followTarget;

    public override void SetFollower(IFollowable followTarget)
    {
        _followTarget = followTarget;
    }

    private void Update()
    {
        if ((_followTarget as UnityEngine.Object) == null) return;

        Vector3 offset = Vector2.down * _topOffset;
        transform.position = _followTarget.ReturnPosition() + offset;
    }
}
