using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class WeaponData 
{
    private DamageableData _damageableData;
    [JsonIgnore] public DamageableData DamageableData { get => _damageableData; set => _damageableData = value; }

    public void ChangeDamage(DamageableData damageableData) { _damageableData = damageableData; }

    public virtual void ChangeRange(float range) { }
    public virtual void ChangeSizeMultiplier(float sizeMultiplier) { }
    public virtual void ChangeLifetime(float lifetime) { }

    public virtual void ChangeTargetCount(int targetCount) { }
    public virtual void ChangeForce(float force) { }
    public virtual void ChangeProjectile(BaseWeapon.Name name) { }
    public virtual void ChangeDelay(float delayModifier) { }

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
        _weaponCreaters[BaseWeapon.Name.Rocket] = new RocketCreater(weaponPrefabs[BaseWeapon.Name.Rocket], weaponData[BaseWeapon.Name.Rocket], effectFactory);

        _weaponCreaters[BaseWeapon.Name.RifleShooter] = new ShooterCreater(weaponPrefabs[BaseWeapon.Name.RifleShooter], weaponData[BaseWeapon.Name.RifleShooter], this);
        _weaponCreaters[BaseWeapon.Name.RocketShooter] = new ShooterCreater(weaponPrefabs[BaseWeapon.Name.RocketShooter], weaponData[BaseWeapon.Name.RocketShooter], this);

        _weaponCreaters[BaseWeapon.Name.StickyBomb] = new StickyBombCreater(weaponPrefabs[BaseWeapon.Name.StickyBomb], weaponData[BaseWeapon.Name.StickyBomb], effectFactory);
    }

    public override BaseWeapon Create(BaseWeapon.Name name)
    {
        return _weaponCreaters[name].Create();
    }
}
