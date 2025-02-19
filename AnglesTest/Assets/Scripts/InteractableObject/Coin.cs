using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour, IInteractable
{
    int _upCount;
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
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.GetCoin);
        GameStateManager.Instance.ChangeCoin(_upCount);
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

    public void Initialize(CoinData data) 
    {
        _upCount = data.UpCount;
        _moveSpeed = data.MoveSpeed;

        _followComponent = GetComponent<FollowComponent>();
        _followComponent.Initialize(_moveSpeed);
        _followComponent.FreezePosition(true);
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}
