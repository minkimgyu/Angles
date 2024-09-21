using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class WeaponData 
{
    public List<ITarget.Type> _targetTypes = new List<ITarget.Type>();
    public float _totalDamageRatio = 1;

    public void ChangeTarget(List<ITarget.Type> targetTypes) { _targetTypes = targetTypes; }
    public void ChangeTotalDamageRatio(float totalDamageRatio) { _totalDamageRatio = totalDamageRatio; }

    public virtual void ChangeDamage(float damage) { }
    public virtual void ChangeDelay(float delay) { }
    public virtual void ChangeRange(float range) { }
    public virtual void ChangeLifetime(float lifetime) { }

    public virtual void ChangeTargetCount(int targetCount) { }
    public virtual void ChangeForce(float force) { }
    public virtual void ChangeProjectile(BaseWeapon.Name name) { }

    public abstract WeaponData Copy();
}

public class WeaponCreater
{
    protected BaseWeapon _weaponPrefab;
    protected WeaponData _weaponData;

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


        _weaponCreaters[BaseWeapon.Name.Bullet] = new BulletCreater(weaponPrefabs[BaseWeapon.Name.Bullet], weaponData[BaseWeapon.Name.Bullet], effectFactory);
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
