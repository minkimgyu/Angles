using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseWeaponData
{
    public BaseWeaponData(float damage)
    {
        _damage = damage;
    }

    public float _damage;
}

public class WeaponCreater : ObjCreater<BaseWeapon> { }

public class WeaponFactory : MonoBehaviour
{
    Dictionary<BaseWeapon.Name, WeaponCreater> _weaponCreaters;

    private static WeaponFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        _weaponCreaters = new Dictionary<BaseWeapon.Name, WeaponCreater>();

        _weaponCreaters[BaseWeapon.Name.Blade] = new BladeCreater();
        _weaponCreaters[BaseWeapon.Name.Bullet] = new BulletCreater();
        _weaponCreaters[BaseWeapon.Name.Rocket] = new RocketCreater();

        _weaponCreaters[BaseWeapon.Name.Blackhole] = new BlackholeCreater();
        _weaponCreaters[BaseWeapon.Name.Shooter] = new ShooterCreater();
        _weaponCreaters[BaseWeapon.Name.StickyBomb] = new StickyBombCreater();

        foreach (var creater in _weaponCreaters)
        {
            GameObject prefab = AddressableManager.Instance.PrefabAssetDictionary[creater.Key.ToString()];
            creater.Value.Initialize(prefab);
        }
    }

    public static BaseWeapon Create(BaseWeapon.Name name)
    {
        return _instance._weaponCreaters[name].Create();
    }
}
