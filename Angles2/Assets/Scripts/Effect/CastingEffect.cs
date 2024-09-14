using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CastingEffect : BaseEffect
{
    IFollowable _followable;
    Vector3 _endScale;
    float _duration;

    public override void AddFollower(IFollowable followable)
    {
        _followable = followable;
    }

    public override void ResetSize(Vector3 startScale, Vector3 endScale, float duration)
    {
        transform.localScale = startScale;
        _endScale = endScale;
        _duration = duration;
    }

    public override void Play()
    {
        transform.DOScale(_endScale, _duration);
    }

    protected override void OnDestroy()
    {
        transform.DOKill();
        base.OnDestroy();
    }

    private void Update()
    {
        if(_followable == null) return;

        transform.position = _followable.ReturnPosition();
        transform.forward = _followable.ReturnFowardDirection();
    }
}
