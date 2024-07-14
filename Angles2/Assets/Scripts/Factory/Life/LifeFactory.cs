using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LifeCreaterInput
{
    public BaseLife _lifePrafab;
    public TextAsset _jsonAsset;
}

[System.Serializable]
public class BaseLifeData
{
    public BaseLifeData(float maxHp, ITarget.Type targetType)
    {
        _maxHp = maxHp;
        _targetType = targetType;
    }

    public float _maxHp;
    public ITarget.Type _targetType;
}

public class LifeCreater<T> : BaseCreater<LifeCreaterInput, BaseLife>
{
    protected BaseLife _prefab;
    protected T _data;
    protected JsonParser _jsonParser;

    public override void Initialize(LifeCreaterInput input)
    {
        _prefab = input._lifePrafab;
        TextAsset asset = input._jsonAsset;

        _jsonParser = new JsonParser();
        _data = _jsonParser.JsonToData<T>(asset.text);
    }
}

public class LifeFactory : MonoBehaviour
{
    [SerializeField] LifeInputDictionary _lifeInputs; // 무기 prefab을 모아서 넣어준다.
    Dictionary<BaseLife.Name, BaseCreater<LifeCreaterInput, BaseLife>> _lifeCreaters;

    private static LifeFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        _lifeCreaters = new Dictionary<BaseLife.Name, BaseCreater<LifeCreaterInput, BaseLife>>();
        Initialize();
    }

    private void Initialize()
    {
        _lifeCreaters[BaseLife.Name.Player] = new PlayerCreater();

        _lifeCreaters[BaseLife.Name.Triangle] = new TriangleCreater();
        _lifeCreaters[BaseLife.Name.Rectangle] = new RectangleCreater();
        _lifeCreaters[BaseLife.Name.Pentagon] = new PentagonCreater();
        _lifeCreaters[BaseLife.Name.Hexagon] = new HexagonCreater();

        foreach (var input in _lifeInputs)
        {
            _lifeCreaters[input.Key].Initialize(input.Value);
        }
    }

    public static BaseLife Create(BaseLife.Name name)
    {
        return _instance._lifeCreaters[name].Create();
    }
}
