using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionViewer : BaseViewer
{
    [SerializeField] GameObject _directionSprite;

    IFollowable _followTarget;

    public override void Initialize() 
    {
        OnOffViewer(false);
    }

    public override void SetFollower(IFollowable followTarget)
    {
        _followTarget = followTarget;
    }

    private void Update()
    {
        if (_directionSprite.activeSelf == false) return;

        transform.position = _followTarget.ReturnPosition();
        transform.right = _followTarget.ReturnFowardDirection();
    }

    public override void OnOffViewer(bool show) 
    {
        _directionSprite.SetActive(show);
    }
}
