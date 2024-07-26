using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class LifeCreater : ObjCreater<BaseLife> { }

public class LifeFactory : MonoBehaviour
{
    Dictionary<BaseLife.Name, LifeCreater> _lifeCreaters;

    private static LifeFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        _lifeCreaters = new Dictionary<BaseLife.Name, LifeCreater>();

        _lifeCreaters[BaseLife.Name.Player] = new PlayerCreater();
        _lifeCreaters[BaseLife.Name.Triangle] = new TriangleCreater();
        _lifeCreaters[BaseLife.Name.Rectangle] = new RectangleCreater();
        _lifeCreaters[BaseLife.Name.Pentagon] = new PentagonCreater();
        _lifeCreaters[BaseLife.Name.Hexagon] = new HexagonCreater();

        foreach (var creater in _lifeCreaters)
        {
            GameObject prefab = AddressableManager.Instance.PrefabAssetDictionary[creater.Key.ToString()];
            creater.Value.Initialize(prefab);
        }
    }

    public static BaseLife Create(BaseLife.Name name)
    {
        return _instance._lifeCreaters[name].Create();
    }
}
