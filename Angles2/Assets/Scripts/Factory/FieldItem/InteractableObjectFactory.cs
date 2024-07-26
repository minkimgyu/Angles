using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractableObjectData
{
    public enum Name
    {
        CardTable,
        Coin,
        Portal
    }
}

[System.Serializable]
public class InteractableObjectCreater : ObjCreater<IInteractable> { }

public class InteractableObjectFactory : MonoBehaviour
{
    Dictionary<IInteractable.Name, InteractableObjectCreater> _itemCreaters;
    private static InteractableObjectFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        _itemCreaters = new Dictionary<IInteractable.Name, InteractableObjectCreater>();
    }

    public static IInteractable Create(IInteractable.Name name)
    {
        return _instance._itemCreaters[name].Create();
    }
}
