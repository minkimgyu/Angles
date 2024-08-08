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

public class WeaponCreater
{
    protected BaseWeapon _weaponPrefab;
    protected BaseWeaponData _weaponData;

    public WeaponCreater(BaseWeapon weaponPrefab, BaseWeaponData weaponData) { _weaponPrefab = weaponPrefab; _weaponData = weaponData; }

    public virtual BaseWeapon Create() { return default; }
}

public class WeaponFactory
{
    Dictionary<BaseWeapon.Name, WeaponCreater> _weaponCreaters;

    public WeaponFactory(Dictionary<BaseWeapon.Name, BaseWeapon> weaponPrefabs, Dictionary<BaseWeapon.Name, BaseWeaponData> weaponDatas,
        System.Func<BaseEffect.Name, BaseEffect> SpawnEffect)
    {
        _weaponCreaters = new Dictionary<BaseWeapon.Name, WeaponCreater>();

        _weaponCreaters[BaseWeapon.Name.Blade] = new BladeCreater(weaponPrefabs[BaseWeapon.Name.Blade], weaponDatas[BaseWeapon.Name.Blade]);
        _weaponCreaters[BaseWeapon.Name.Blackhole] = new BlackholeCreater(weaponPrefabs[BaseWeapon.Name.Blackhole], weaponDatas[BaseWeapon.Name.Blackhole]);


        _weaponCreaters[BaseWeapon.Name.Bullet] = new BulletCreater(weaponPrefabs[BaseWeapon.Name.Bullet], weaponDatas[BaseWeapon.Name.Bullet], SpawnEffect);
        _weaponCreaters[BaseWeapon.Name.Rocket] = new RocketCreater(weaponPrefabs[BaseWeapon.Name.Rocket], weaponDatas[BaseWeapon.Name.Rocket], SpawnEffect);

        _weaponCreaters[BaseWeapon.Name.RifleShooter] = new ShooterCreater(weaponPrefabs[BaseWeapon.Name.RifleShooter], weaponDatas[BaseWeapon.Name.RifleShooter], Create);
        _weaponCreaters[BaseWeapon.Name.RocketShooter] = new ShooterCreater(weaponPrefabs[BaseWeapon.Name.RocketShooter], weaponDatas[BaseWeapon.Name.RocketShooter], Create);

        _weaponCreaters[BaseWeapon.Name.StickyBomb] = new StickyBombCreater(weaponPrefabs[BaseWeapon.Name.StickyBomb], weaponDatas[BaseWeapon.Name.StickyBomb], SpawnEffect);
    }

    public BaseWeapon Create(BaseWeapon.Name name)
    {
        return _weaponCreaters[name].Create();
    }
}
