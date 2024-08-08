using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : BaseInteractableObjectData
{
    public int _cardCount;
    public int _recreateCount;

    public ShopData(int cardCount, int recreateCount)
    {
        _cardCount = cardCount;
        _recreateCount = recreateCount;
    }
}

public class ShopCreater : InteractableObjectCreater
{
    public ShopCreater(IInteractable interactablePrefab, BaseInteractableObjectData interactableData)
        : base(interactablePrefab, interactableData)
    {
    }

    public override IInteractable Create()
    {
        GameObject interactableObject = UnityEngine.Object.Instantiate(_interactablePrefab.ReturnGameObject());
        IInteractable interactable = interactableObject.GetComponent<IInteractable>();
        if (interactable == null) return null;

        ShopData data = _interactableData as ShopData;
        interactable.Initialize(data);
        return interactable;
    }
}
