using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinData : BaseInteractableObjectData
{
    [JsonProperty] private int _upCount;
    [JsonProperty] private float _moveSpeed;

    [JsonIgnore] public int UpCount { get => _upCount; }
    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; }

    public CoinData(int upCount, float moveSpeed)
    {
        _upCount = upCount;
        _moveSpeed = moveSpeed;
    }
}

public class CoinCreater : InteractableObjectCreater
{
    public CoinCreater(IInteractable interactablePrefab, BaseInteractableObjectData interactableData) : base(interactablePrefab, interactableData)
    {
    }

    public override IInteractable Create()
    {
        GameObject interactableObject = Object.Instantiate(_interactablePrefab.ReturnGameObject());
        IInteractable interactable = interactableObject.GetComponent<IInteractable>();
        if (interactable == null) return null;

        CoinData data = _interactableData as CoinData;
        interactable.Initialize(data);
        return interactable;
    }
}