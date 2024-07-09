using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponCreaterInput
{
    public BaseWeapon _weaponPrafab;
    public TextAsset _jsonAsset;
}

public class BaseWeaponData
{

}

public class WeaponCreater : BaseCreater<WeaponCreaterInput, BaseWeapon>
{
    protected BaseWeapon _prefab;
    protected BaseWeaponData _data;
    JsonParser _jsonParser;

    public override void Initialize(WeaponCreaterInput input)
    {
        _prefab = input._weaponPrafab;
        TextAsset asset = input._jsonAsset;
        _data = _jsonParser.JsonToData<BaseWeaponData>(asset.text);
    }
}

public class WeaponFactory : MonoBehaviour
{
    [SerializeField] WeaponInputDictionary _weaponInputs; // 무기 prefab을 모아서 넣어준다.
    Dictionary<BaseWeapon.Name, WeaponCreater> _weaponCreaters;

    private static WeaponFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        _weaponCreaters = new Dictionary<BaseWeapon.Name, WeaponCreater>();
        Initialize();
    }

    private void Initialize()
    {
        _weaponCreaters[BaseWeapon.Name.Blade] = new WeaponCreater();
    }

    public static BaseWeapon Create(BaseWeapon.Name name)
    {
        return _instance._weaponCreaters[name].Create();
    }
}
