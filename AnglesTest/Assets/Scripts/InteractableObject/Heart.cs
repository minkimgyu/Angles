using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour, IInteractable
{
    float _healPoint;
    float _moveSpeed;
    FollowComponent _moveStrategy;

    public void OnInteractEnter(IInteracter interacter)
    {
        _moveStrategy.FreezePosition(false);

        IFollowable followable = interacter.ReturnFollower();
        _moveStrategy.ResetFollower(followable);
    }

    public void OnInteract(IInteracter interacter)
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.GetHeart);
        interacter.GetHeal(_healPoint);
        Destroy(gameObject);
    }

    public void OnInteractExit(IInteracter interacter) { }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Initialize(HeartData data)
    {
        _moveSpeed = data.MoveSpeed;
        _healPoint = data.HealPoint;

        _moveStrategy = GetComponent<FollowComponent>();
        _moveStrategy.Initialize(_moveSpeed);
        _moveStrategy.FreezePosition(true);
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}
