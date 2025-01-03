using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour, IInteractable
{
    float _healPoint;
    float _moveSpeed;
    FollowComponent _trackComponent;

    public void OnInteractEnter(IInteracter interacter)
    {
        _trackComponent.FreezePosition(false);

        IFollowable followable = interacter.ReturnFollower();
        _trackComponent.ResetFollower(followable);
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

        _trackComponent = GetComponent<FollowComponent>();
        _trackComponent.Initialize(_moveSpeed);
        _trackComponent.FreezePosition(true);
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}
