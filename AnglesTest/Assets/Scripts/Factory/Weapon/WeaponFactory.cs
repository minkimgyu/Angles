using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class WeaponData
{
    [JsonIgnore] DamageableData _damageableData;
    [JsonIgnore] List<ITarget.Type> _targetTypes;

    [JsonIgnore] public DamageableData DamageableStat { get => _damageableData; set => _damageableData = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public abstract WeaponData Copy();
}

public class WeaponCreater
{
    protected BaseWeapon _weaponPrefab;
    private WeaponData _weaponData;

    protected WeaponData CopyWeaponData { get { return _weaponData.Copy(); } }

    public WeaponCreater(BaseWeapon weaponPrefab, WeaponData weaponData) 
    { 
        _weaponPrefab = weaponPrefab;
        _weaponData = weaponData; 
    }

    public virtual BaseWeapon Create() { return default; }
}

public class WeaponFactory : BaseFactory
{
    Dictionary<BaseWeapon.Name, WeaponCreater> _weaponCreaters;

    public WeaponFactory(Dictionary<BaseWeapon.Name, BaseWeapon> weaponPrefabs, Dictionary<BaseWeapon.Name, WeaponData> weaponData, BaseFactory effectFactory)
    {
        _weaponCreaters = new Dictionary<BaseWeapon.Name, WeaponCreater>();

        _weaponCreaters[BaseWeapon.Name.Blade] = new BladeCreater(weaponPrefabs[BaseWeapon.Name.Blade], weaponData[BaseWeapon.Name.Blade]);
        _weaponCreaters[BaseWeapon.Name.Blackhole] = new BlackholeCreater(weaponPrefabs[BaseWeapon.Name.Blackhole], weaponData[BaseWeapon.Name.Blackhole]);

        _weaponCreaters[BaseWeapon.Name.PentagonicBullet] = new BulletCreater(weaponPrefabs[BaseWeapon.Name.PentagonicBullet], weaponData[BaseWeapon.Name.PentagonicBullet], effectFactory);
        _weaponCreaters[BaseWeapon.Name.ShooterBullet] = new BulletCreater(weaponPrefabs[BaseWeapon.Name.ShooterBullet], weaponData[BaseWeapon.Name.ShooterBullet], effectFactory);
        _weaponCreaters[BaseWeapon.Name.PentagonBullet] = new BulletCreater(weaponPrefabs[BaseWeapon.Name.PentagonBullet], weaponData[BaseWeapon.Name.PentagonBullet], effectFactory);
        _weaponCreaters[BaseWeapon.Name.HexahornBullet] = new BulletCreater(weaponPrefabs[BaseWeapon.Name.HexahornBullet], weaponData[BaseWeapon.Name.HexahornBullet], effectFactory);


        _weaponCreaters[BaseWeapon.Name.ShooterRocket] = new RocketCreater(weaponPrefabs[BaseWeapon.Name.ShooterRocket], weaponData[BaseWeapon.Name.ShooterRocket], effectFactory);

        _weaponCreaters[BaseWeapon.Name.RifleShooter] = new ShooterCreater(weaponPrefabs[BaseWeapon.Name.RifleShooter], weaponData[BaseWeapon.Name.RifleShooter], this);
        _weaponCreaters[BaseWeapon.Name.RocketShooter] = new ShooterCreater(weaponPrefabs[BaseWeapon.Name.RocketShooter], weaponData[BaseWeapon.Name.RocketShooter], this);

        _weaponCreaters[BaseWeapon.Name.StickyBomb] = new StickyBombCreater(weaponPrefabs[BaseWeapon.Name.StickyBomb], weaponData[BaseWeapon.Name.StickyBomb], effectFactory);
        _weaponCreaters[BaseWeapon.Name.TrackableMissile] = new TrackableMissileCreater(weaponPrefabs[BaseWeapon.Name.TrackableMissile], weaponData[BaseWeapon.Name.TrackableMissile], effectFactory);
    }

    public override BaseWeapon Create(BaseWeapon.Name name)
    {
        return _weaponCreaters[name].Create();
    }
}
