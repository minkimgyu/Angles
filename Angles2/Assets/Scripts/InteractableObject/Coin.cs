using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour, IInteractable
{
    [SerializeField] int _upCount = 1;
    [SerializeField] float _moveSpeed = 8f;

    TrackComponent _trackComponent;

    private void Start()
    {
        _trackComponent = GetComponent<TrackComponent>();
        _trackComponent.Initialize(_moveSpeed);
    }

    public void OnInteractEnter(InteractEnterData data)
    {
        _trackComponent.ResetFollower(data.Followable);
    }

    public void OnInteract(InteractData data) 
    {
        StageManager.Instance.CoinCount += _upCount;
        Destroy(gameObject);
    }

    public void OnInteractExit(InteractExitData data) { }

    public UnityEngine.Object ReturnObject()
    {
        return this;
    }
}
