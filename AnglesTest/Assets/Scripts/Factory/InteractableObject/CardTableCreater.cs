using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class CardTableData : BaseInteractableObjectData
{
    [JsonProperty] private int _cardCount;
    [JsonIgnore] public int CardCount { get => _cardCount; }

    public CardTableData(int cardCount)
    {
        _cardCount = cardCount;
    }
}

public class CardTableCreater : InteractableObjectCreater
{
    public CardTableCreater(IInteractable interactablePrefab, BaseInteractableObjectData interactableData) 
        : base(interactablePrefab, interactableData)
    {
    }

    public override IInteractable Create()
    {
        GameObject interactableObject = UnityEngine.Object.Instantiate(_interactablePrefab.ReturnGameObject());
        IInteractable interactable = interactableObject.GetComponent<IInteractable>();
        if (interactable == null) return null;

        CardTableData data = _interactableData as CardTableData;
        interactable.Initialize(data);
        return interactable;
    }
}
