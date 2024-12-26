using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionViewer : BaseViewer
{
    [SerializeField] GameObject _directionSprite;

    IFollowable _followTarget;

    public void SetFollower(IFollowable followTarget)
    {
        _followTarget = followTarget;
    }

    public override void Initialize() 
    {
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.State.OnTurnOnOffDirection, new TurnOnOffCommand(TurnOnViewer));
        TurnOnViewer(false);
    }
    public override void TurnOnViewer(bool show) 
    {
        _directionSprite.SetActive(show);
    }

    private void Update()
    {
        if ((_followTarget as UnityEngine.Object) == null) return;

        transform.position = _followTarget.ReturnPosition();
        transform.right = _followTarget.ReturnFowardDirection();
    }
}
