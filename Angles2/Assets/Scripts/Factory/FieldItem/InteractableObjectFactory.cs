using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableObjectCreaterInput
{
    public GameObject _itemPrefab;
    public TextAsset _jsonAsset;
}

public class BaseInteractableObjectData
{
    public enum Name
    {
        CardTable,
        Coin
    }
}

[System.Serializable]
public class InteractableObjectCreater : BaseCreater<InteractableObjectCreaterInput, IInteractable>
{
    protected GameObject _prefab;
    protected BaseInteractableObjectData _data;
    JsonParser _jsonParser;

    public override void Initialize(InteractableObjectCreaterInput input)
    {
        _prefab = input._itemPrefab;
        TextAsset asset = input._jsonAsset;
        _data = _jsonParser.JsonToData<BaseInteractableObjectData>(asset.text);
    }
}

public class InteractableObjectFactory : MonoBehaviour
{
    [SerializeField] InteractableObjectInputDictionary _itemInputs; // 무기 prefab을 모아서 넣어준다.
    Dictionary<IInteractable.Name, InteractableObjectCreater> _itemCreaters;

    private static InteractableObjectFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        _itemCreaters = new Dictionary<IInteractable.Name, InteractableObjectCreater>();
        Initialize();
    }

    private void Initialize()
    {
    }

    public static IInteractable Create(IInteractable.Name name)
    {
        return _instance._itemCreaters[name].Create();
    }
}
