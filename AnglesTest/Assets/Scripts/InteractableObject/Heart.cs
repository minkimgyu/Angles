using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour, IInteractable
{
    float _healPoint;
    float _moveSpeed;
    FollowComponent _followComponent;

    public void OnInteractEnter(IInteracter interacter)
    {
        _followComponent.FreezePosition(false);

        IFollowable followable = interacter.ReturnFollower();
        _followComponent.InjectFollower(followable);
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

    private void Update()
    {
        _followComponent.OnUpdate();
    }

    private void FixedUpdate()
    {
        _followComponent.OnFixedUpdate();
    }

    public void Initialize(HeartData data)
    {
        _moveSpeed = data.MoveSpeed;
        _healPoint = data.HealPoint;

        _followComponent = GetComponent<FollowComponent>();
        _followComponent.Initialize(_moveSpeed);
        _followComponent.FreezePosition(true);
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}
