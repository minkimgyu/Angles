using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FieldItemCreaterInput
{
    public FieldItem _itemPrefab;
    public TextAsset _jsonAsset;
}

public class BaseItemData
{
    
}

public class FieldItemCreater : BaseCreater<FieldItemCreaterInput, FieldItem>
{
    protected FieldItem _prefab;
    protected BaseItemData _data;
    JsonParser _jsonParser;

    public override void Initialize(FieldItemCreaterInput input)
    {
        _prefab = input._itemPrefab;
        TextAsset asset = input._jsonAsset;
        _data = _jsonParser.JsonToData<BaseItemData>(asset.text);
    }
}

public class FieldItemFactory : MonoBehaviour
{
    [SerializeField] ItemInputDictionary _itemInputs; // 무기 prefab을 모아서 넣어준다.
    Dictionary<FieldItem.Name, FieldItemCreater> _itemCreaters;

    private static FieldItemFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        _itemCreaters = new Dictionary<FieldItem.Name, FieldItemCreater>();
        Initialize();
    }

    private void Initialize()
    {
    }

    public static FieldItem Create(FieldItem.Name name)
    {
        return _instance._itemCreaters[name].Create();
    }
}
