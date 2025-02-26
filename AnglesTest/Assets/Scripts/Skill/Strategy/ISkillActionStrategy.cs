using DamageUtility;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISkillActionStrategy
{
    // 성공적으로 실행했을 때 true 반환
    bool OnReflect(GameObject targetObject, Vector3 contactPos) { return false; }

    void OnDelayRequested() { }

    void OnAdd() { }
    void OnRevive() { }
    void OnUpgrade() { }
}

public class NoActionStrategy : ISkillActionStrategy
{
}

public class UpgradeStatStrategy : ISkillActionStrategy
{
    ICaster _caster;
    ICooltimeStat _stat;
    IStatModifier _statModifier;

    Func<int> GetUpgradePoint;

    public void OnUpgrade() => UpgradeStat();
    public void OnAdd() => UpgradeStat();

    void UpgradeStat()
    {
        IStatUpgradable visitor = _caster.GetComponent<IStatUpgradable>();
        if (visitor == null) return;

        visitor.Upgrade(_statModifier, GetUpgradePoint() - 1);
    }
}

public class MagneticFieldStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    IDamageStat _damageStat;

    float _adRatio;
    float _range;
    float _groggyDuration;
    List<ITarget.Type> _targetTypes;
    Func<List<IDamageable>> GetTargets;

    public MagneticFieldStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,

        // 아래 둘 가변 요소임
        ISizeModifyStat sizeModifyStat,
        IDamageStat damageStat,
        IDelayStat delayStat,

        float adRatio,
        float range,
        float groggyDuration,
        List<ITarget.Type> targetTypes,
        Func<List<IDamageable>> GetTargets)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;

        _damageStat = damageStat;

        _adRatio = adRatio;
        _range = range;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;
        this.GetTargets = GetTargets;
    }

    public void OnDelayRequested()
    {
        DamageableData damageData = new DamageableData
        (
            _caster,
                new DamageStat(
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio
            ),
            _targetTypes
        );

        List<IDamageable> damageableTargets = GetTargets();

        for (int i = 0; i < damageableTargets.Count; i++)
        {
            Damage.Hit(damageData, damageableTargets[i]);
        }
    }
}

public class ContactAttackStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    IDamageStat _damageStat;

    float _adRatio;
    float _range;
    float _groggyDuration;
    List<ITarget.Type> _targetTypes;

    BaseFactory _effectFactory;

    public ContactAttackStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,
        IDamageStat damageStat,
        float adRatio,
        float range,
        float groggyDuration,
        List<ITarget.Type> targetTypes,
        BaseFactory effectFactory)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;
        _damageStat = damageStat;
        _adRatio = adRatio;
        _range = range;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;
        _effectFactory = effectFactory;
    }

    public bool OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return false;

        IDamageable damageable = targetObject.GetComponent<IDamageable>();
        if (damageable == null) return false;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return false;

        Debug.Log("ContactAttack");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(contactPos);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio),
            _targetTypes,
            _groggyDuration
        );

        Damage.Hit(damageData, damageable);
        return true;
    }
}

public class SelfDestructionStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    ISizeModifyStat _sizeModifyStat;
    IDamageStat _damageStat;

    float _adRatio;
    float _range;
    float _groggyDuration;
    List<ITarget.Type> _targetTypes;

    BaseEffect.Name _effectName;
    BaseFactory _effectFactory;

    public SelfDestructionStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,

        // 아래 둘 가변 요소임
        ISizeModifyStat sizeModifyStat,
        IDamageStat damageStat,

        float adRatio,
        float range,
        float groggyDuration,
        List<ITarget.Type> targetTypes,
        BaseEffect.Name shockwaveEffectName,
        BaseFactory effectFactory)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;

        _sizeModifyStat = sizeModifyStat;
        _damageStat = damageStat;

        _adRatio = adRatio;
        _range = range;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;

        _effectName = shockwaveEffectName;
        _effectFactory = effectFactory;
    }

    public void OnDelayRequested()
    {
        Debug.Log("SelfDestruction");
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
        if (effect == null) return;

        Transform casterTransform = _caster.GetComponent<Transform>();

        effect.ResetPosition(casterTransform.position);
        effect.Play();

        IDamageable damageable = _caster.GetComponent<IDamageable>();
        if (damageable == null) return;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, casterTransform.position, 0.3f);

        DamageableData selfDamageData = new DamageableData(_caster, new DamageStat(Damage.InstantDeathDamage));
        Damage.Hit(selfDamageData, damageable);

        DamageableData damageData = new DamageableData
        (
            _caster,
           new DamageStat
           (
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio
           ),
            _targetTypes
        );

        Damage.HitCircleRange(damageData, casterTransform.position, _range * _sizeModifyStat.SizeMultiplier, true, Color.red, 3);
    }
}

public class ShockwaveStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    ISizeModifyStat _sizeModifyStat;
    IDamageStat _damageStat;

    float _adRatio;
    float _range;
    float _groggyDuration;
    List<ITarget.Type> _targetTypes;

    BaseEffect.Name _effectName;
    BaseFactory _effectFactory;

    public ShockwaveStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,

        // 아래 둘 가변 요소임
        ISizeModifyStat sizeModifyStat,
        IDamageStat damageStat,

        float adRatio,
        float range,
        float groggyDuration,
        List<ITarget.Type> targetTypes,
        BaseEffect.Name shockwaveEffectName,
        BaseFactory effectFactory)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;

        _sizeModifyStat = sizeModifyStat;
        _damageStat = damageStat;

        _adRatio = adRatio;
        _range = range;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;

        _effectName = shockwaveEffectName;
        _effectFactory = effectFactory;
    }

    public void OnDelayRequested()
    {
        Transform casterTransform = _caster.GetComponent<Transform>();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Shockwave, casterTransform.position, 0.6f);
        
        BaseEffect effect = _effectFactory.Create(_effectName);
        effect.ResetPosition(casterTransform.position);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio
            ),
            _targetTypes
        );

        Damage.HitCircleRange(damageData, casterTransform.position, _range * _sizeModifyStat.SizeMultiplier, true, Color.red, 3);
    }
}

public class KnockbackStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    IDamageStat _damageStat;
    ISizeModifyStat _sizeStat;

    SerializableVector2 _size;
    SerializableVector2 _offset;
    float _force;
    float _adRatio;
    float _groggyDuration;
    BaseFactory _effectFactory;
    List<ITarget.Type> _targetTypes;

    public KnockbackStrategy(ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,
        IDamageStat damageStat,
        ISizeModifyStat sizeStat,
        SerializableVector2 size,
        SerializableVector2 offset,
        float force,
        float adRatio,
        float groggyDuration,
        BaseFactory effectFactory,
        List<ITarget.Type> targetTypes)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;
        _damageStat = damageStat;
        _sizeStat = sizeStat;
        _size = size;
        _offset = offset;
        _force = force;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _effectFactory = effectFactory;
        _targetTypes = targetTypes;
    }

    public bool OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return false;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return false;

        Transform casterTransform = _caster.GetComponent<Transform>();

        IForce forceTarget = targetObject.GetComponent<IForce>();
        if (forceTarget != null) forceTarget.ApplyForce(casterTransform.forward, _force, ForceMode2D.Impulse);

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Knockback);
        Debug.Log("Knockback");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.KnockbackEffect);
        effect.ResetSize(_sizeStat.SizeMultiplier);
        effect.ResetPosition(casterTransform.position, casterTransform.right);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio
            ),
            _targetTypes,
            _groggyDuration
        );


        Damage.HitBoxRange(damageData, casterTransform.position, _offset.V2, casterTransform.right,
            _size.V2 * _sizeStat.SizeMultiplier, true, Color.red);
        return true;
    }
}

public class StatikkStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    IDamageStat _damageStat;
    ISizeModifyStat _sizeStat;
    IMaxTargetStat _maxTargetStat;
    IMaxStackStat _maxStackStat;

    float _adRatio;
    float _range;
    float _groggyDuration;
    BaseFactory _effectFactory;
    List<ITarget.Type> _targetTypes;

    public StatikkStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,
        IDamageStat damageStat,
        ISizeModifyStat sizeStat,
        IMaxTargetStat maxTargetStat,
        IMaxStackStat maxStackStat,
        float adRatio,
        float range,
        float groggyDuration,
        BaseFactory effectFactory,
        List<ITarget.Type> targetTypes)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;
        _damageStat = damageStat;
        _sizeStat = sizeStat;
        _maxTargetStat = maxTargetStat;
        _maxStackStat = maxStackStat;
        _adRatio = adRatio;
        _range = range;
        _groggyDuration = groggyDuration;
        _effectFactory = effectFactory;
        _targetTypes = targetTypes;
    }

    public bool OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return false;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return false;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Statikk);

        List<Vector2> hitPoints;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat
            (
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio
            ),
            _targetTypes,
            _groggyDuration
        );
        Damage.HitRaycast(damageData, _maxTargetStat.MaxTargetCount, targetObject.transform.position, _range * _sizeStat.SizeMultiplier, out hitPoints, true, Color.red, 3);

        for (int i = 0; i < hitPoints.Count; i++)
        {
            BaseEffect effect = _effectFactory.Create(BaseEffect.Name.LaserEffect);
            effect.ResetColor(new Color(93f / 255f, 177f / 255f, 255f / 255f), new Color(255f / 255f, 255f / 255f, 255f / 255f));
            effect.ResetPosition(Vector3.zero);
            effect.ResetLine(targetObject.transform.position, hitPoints[i]);

            effect.Play();
        }

        return true;
    }
}

public class SpreadBulletStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    IForceStat _forceStat;
    IDamageStat _damageStat;

    float _bulletCount;
    float _adRatio;
    float _groggyDuration;
    float _distanceFromCaster;
    BaseWeapon.Name _bulletName;
    BaseFactory _weaponFactory;
    List<ITarget.Type> _targetTypes;


    public SpreadBulletStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,
        SkillData skillData,
        IForceStat forceStat,
        IDelayStat delayStat,
        IDamageStat damageStat,
        float bulletCount,
        float adRatio,
        float groggyDuration,
        float distanceFromCaster,
        BaseWeapon.Name bulletName,
        BaseFactory weaponFactory,
        List<ITarget.Type> targetTypes)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;
        _forceStat = forceStat;
        _damageStat = damageStat;
        _bulletCount = bulletCount;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _distanceFromCaster = distanceFromCaster;
        _bulletName = bulletName;
        _weaponFactory = weaponFactory;
        _targetTypes = targetTypes;
    }

    public void OnDelayRequested() 
    {
        for (int i = 1; i <= _bulletCount; i++)
        {
            float angle = 360f / _bulletCount * i;
            ShootBullet(angle);
        }
    }

    void ShootBullet(float angle)
    {
        Transform casterTransform = _caster.GetComponent<Transform>();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.SpreadBullets, casterTransform.position, 0.3f);

        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 direction = new Vector3(x, y, 0);
        Vector3 spawnPosition = casterTransform.position + direction * _distanceFromCaster;

        BaseWeapon weapon = _weaponFactory.Create(_bulletName);
        if (weapon == null) return;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio
            ),
            _targetTypes,
            _groggyDuration
        );

        BulletDataModifier bulletDataModifier = new BulletDataModifier(damageData);

        weapon.ModifyData(bulletDataModifier);
        weapon.Activate();

        weapon.ResetPosition(spawnPosition, direction);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _forceStat.Force);
    }
}

public class SpreadMissileStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    IForceStat _forceStat;
    IDamageStat _damageStat;

    float _bulletCount;
    float _adRatio;
    float _groggyDuration;
    float _distanceFromCaster;
    BaseWeapon.Name _bulletName;
    BaseFactory _weaponFactory;
    List<ITarget.Type> _targetTypes;

    Func<List<ITarget>> GetTargets;

    public SpreadMissileStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,
        SkillData skillData,
        IForceStat forceStat,
        IDelayStat delayStat,
        IDamageStat damageStat,
        float bulletCount,
        float adRatio,
        float groggyDuration,
        float distanceFromCaster,
        BaseWeapon.Name bulletName,
        BaseFactory weaponFactory,
        List<ITarget.Type> targetTypes,
        Func<List<ITarget>> GetTargets)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;
        _forceStat = forceStat;
        _damageStat = damageStat;
        _bulletCount = bulletCount;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _distanceFromCaster = distanceFromCaster;
        _bulletName = bulletName;
        _weaponFactory = weaponFactory;
        _targetTypes = targetTypes;
        this.GetTargets = GetTargets;
    }

    public void OnDelayRequested()
    {
        for (int i = 1; i <= _bulletCount; i++)
        {
            float angle = 360f / _bulletCount * i;
            ShootBullet(angle);
        }
    }

    void ShootBullet(float angle)
    {
        Transform casterTransform = _caster.GetComponent<Transform>();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.SpreadBullets, casterTransform.position, 0.3f);

        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 direction = new Vector3(x, y, 0);
        Vector3 spawnPosition = casterTransform.position + direction * _distanceFromCaster;

        BaseWeapon weapon = _weaponFactory.Create(_bulletName);
        if (weapon == null) return;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio
            ),
            _targetTypes,
            _groggyDuration
        );

        BulletDataModifier bulletDataModifier = new BulletDataModifier(damageData);

        weapon.ModifyData(bulletDataModifier);
        weapon.Activate();

        weapon.ResetPosition(spawnPosition, direction);

        ITrackable projectile = weapon.GetComponent<ITrackable>();
        if (projectile == null) return;

        List<ITarget> targets = GetTargets();
        if (targets == null || targets.Count == 0) return; // 타겟 가져오기

        projectile.InjectTarget(targets[0]);
    }
}

public class ImpactStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    ISizeModifyStat _sizeModifyStat;
    IDamageStat _damageStat;

    float _adRatio;
    float _range;
    float _groggyDuration;
    List<ITarget.Type> _targetTypes;
    BaseFactory _effectFactory;

    public ImpactStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,

        // 아래 둘 가변 요소임
        ISizeModifyStat sizeModifyStat,
        IDamageStat damageStat,

        float adRatio,
        float range,
        float groggyDuration,
        List<ITarget.Type> targetTypes,
        BaseFactory effectFactory)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;

        _sizeModifyStat = sizeModifyStat;
        _damageStat = damageStat;

        _adRatio = adRatio;
        _range = range;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;
        _effectFactory = effectFactory;
    }

    public bool OnReflect(GameObject targetObject, Vector3 contactPos) 
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return false;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return false;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Impact, 0.7f);
        Debug.Log("Impact");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
        if (effect == null) return false;

        effect.ResetPosition(contactPos);
        effect.ResetSize(_sizeModifyStat.SizeMultiplier);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _damageStat.Damage,
                _upgradeableSkillData.AttackDamage,
                _adRatio,
                _upgradeableSkillData.TotalDamageRatio
            ),
            _targetTypes,
            _groggyDuration
        );

        Damage.HitCircleRange(damageData, contactPos, _range * _sizeModifyStat.SizeMultiplier, true, Color.red, 3);
        return true;
    }
}

public class ReviveImpactStrategy : ISkillActionStrategy
{
    ICaster _caster;
    IUpgradeableSkillData _upgradeableSkillData;

    float _range;
    List<ITarget.Type> _targetTypes;
    BaseFactory _effectFactory;

    public ReviveImpactStrategy(
        ICaster caster,
        IUpgradeableSkillData upgradeableSkillData,

        float range,
        List<ITarget.Type> targetTypes,
        BaseFactory effectFactory)
    {
        _caster = caster;
        _upgradeableSkillData = upgradeableSkillData;
        _targetTypes = targetTypes;
        _effectFactory = effectFactory;
    }

    public bool OnRevive(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return false;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return false;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Impact, 0.7f);
        Debug.Log("Impact");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
        if (effect == null) return false;

        effect.ResetPosition(contactPos);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                Damage.InstantDeathDamage
            ),
            _targetTypes
        );

        Damage.HitCircleRange(damageData, contactPos, _range, true, Color.red, 3);
        return true;
    }
}