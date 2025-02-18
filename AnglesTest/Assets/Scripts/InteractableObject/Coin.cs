using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour, IInteractable
{
    int _upCount;
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
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.GetCoin);
        GameStateManager.Instance.ChangeCoin(_upCount);
        Destroy(gameObject);
    }

    public void OnInteractExit(IInteracter interacter) { }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Initialize(CoinData data) 
    {
        _upCount = data.UpCount;
        _moveSpeed = data.MoveSpeed;

        _moveStrategy = GetComponent<FollowComponent>();
        _moveStrategy.Initialize(_moveSpeed);
        _moveStrategy.FreezePosition(true);
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}
