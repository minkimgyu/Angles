using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour, IInteractable
{
    int _upCount;
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

        _trackComponent = GetComponent<FollowComponent>();
        _trackComponent.Initialize(_moveSpeed);
        _trackComponent.FreezePosition(true);
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}
