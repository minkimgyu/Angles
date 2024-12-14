using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinData : BaseInteractableObjectData
{
    public int _upCount;
    public float _moveSpeed;

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