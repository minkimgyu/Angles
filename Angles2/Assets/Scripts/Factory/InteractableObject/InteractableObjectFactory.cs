using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseInteractableObjectData { }

[System.Serializable]
abstract public class InteractableObjectCreater
{
    protected IInteractable _interactablePrefab;
    protected BaseInteractableObjectData _interactableData;

    public InteractableObjectCreater(IInteractable interactablePrefab, BaseInteractableObjectData interactableData) 
    { _interactablePrefab = interactablePrefab; _interactableData = interactableData; }

    public abstract IInteractable Create();
}

public class InteractableObjectFactory : BaseFactory
{
    Dictionary<IInteractable.Name, InteractableObjectCreater> _itemCreaters;

    public InteractableObjectFactory(Dictionary<IInteractable.Name, IInteractable> interactablePrefabs, 
        Dictionary<IInteractable.Name, BaseInteractableObjectData> interactableDatas)
    {
        _itemCreaters = new Dictionary<IInteractable.Name, InteractableObjectCreater>();

        _itemCreaters.Add(IInteractable.Name.CardTable, new CardTableCreater(interactablePrefabs[IInteractable.Name.CardTable], interactableDatas[IInteractable.Name.CardTable]));
        _itemCreaters.Add(IInteractable.Name.Shop, new ShopCreater(interactablePrefabs[IInteractable.Name.Shop], interactableDatas[IInteractable.Name.Shop]));
        _itemCreaters.Add(IInteractable.Name.SkillBubble, new SkillBubbleCreater(interactablePrefabs[IInteractable.Name.SkillBubble], interactableDatas[IInteractable.Name.SkillBubble]));
        _itemCreaters.Add(IInteractable.Name.Coin, new CoinCreater(interactablePrefabs[IInteractable.Name.Coin], interactableDatas[IInteractable.Name.Coin]));
    }

    public override IInteractable Create(IInteractable.Name name)
    {
        return _itemCreaters[name].Create();
    }
}
