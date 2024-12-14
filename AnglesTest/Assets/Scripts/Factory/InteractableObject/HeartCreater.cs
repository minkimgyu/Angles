using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartData : BaseInteractableObjectData
{
    public int _healPoint;
    public float _moveSpeed;

    public HeartData(int healPoint, float moveSpeed)
    {
        _healPoint = healPoint;
        _moveSpeed = moveSpeed;
    }
}

public class HeartCreater : InteractableObjectCreater
{
    public HeartCreater(IInteractable interactablePrefab, BaseInteractableObjectData interactableData) : base(interactablePrefab, interactableData)
    {
    }

    public override IInteractable Create()
    {
        GameObject interactableObject = Object.Instantiate(_interactablePrefab.ReturnGameObject());
        IInteractable interactable = interactableObject.GetComponent<IInteractable>();
        if (interactable == null) return null;

        HeartData data = _interactableData as HeartData;
        interactable.Initialize(data);
        return interactable;
    }
}
