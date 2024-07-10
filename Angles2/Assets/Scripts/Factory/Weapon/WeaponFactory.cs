using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponCreaterInput
{
    public BaseWeapon _weaponPrafab;
    public TextAsset _jsonAsset;
}

[System.Serializable]
public class BaseWeaponData
{
    public BaseWeaponData(float damage)
    {
        _damage = damage;
    }

    public float _damage;
}

public class WeaponCreater<T> : BaseCreater<WeaponCreaterInput, BaseWeapon>
{
    protected BaseWeapon _prefab;
    protected T _data;
    protected JsonParser _jsonParser;

    public override void Initialize(WeaponCreaterInput input)
    {
        _prefab = input._weaponPrafab;
        TextAsset asset = input._jsonAsset;

        _jsonParser = new JsonParser();
        _data = _jsonParser.JsonToData<T>(asset.text);
    }
}

public class WeaponFactory : MonoBehaviour
{
    [SerializeField] WeaponInputDictionary _weaponInputs; // 무기 prefab을 모아서 넣어준다.
    Dictionary<BaseWeapon.Name, BaseCreater<WeaponCreaterInput, BaseWeapon>> _weaponCreaters;

    private static WeaponFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        _weaponCreaters = new Dictionary<BaseWeapon.Name, BaseCreater<WeaponCreaterInput, BaseWeapon>>();
        Initialize();
    }

    private void Initialize()
    {
        _weaponCreaters[BaseWeapon.Name.Blade] = new BladeCreater();
        _weaponCreaters[BaseWeapon.Name.Bullet] = new BulletCreater();
        _weaponCreaters[BaseWeapon.Name.Rocket] = new RocketCreater();

        _weaponCreaters[BaseWeapon.Name.Blackhole] = new BlackholeCreater();
        _weaponCreaters[BaseWeapon.Name.Shooter] = new ShooterCreater();
        _weaponCreaters[BaseWeapon.Name.StickyBomb] = new StickyBombCreater();

        foreach (var input in _weaponInputs)
        {
            _weaponCreaters[input.Key].Initialize(input.Value);
        }
    }

    public static BaseWeapon Create(BaseWeapon.Name name)
    {
        return _instance._weaponCreaters[name].Create();
    }
}
