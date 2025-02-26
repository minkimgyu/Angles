using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 무기는 Visitor 제거해주기
// PlayerData를 FSM 내부에서 캐싱해서 사용
// 
abstract public class BaseSkill : ISkillUpgradable
{
    public enum Name
    {
        Statikk,
        Knockback,
        Impact,
        ContactAttack,

        SpawnRifleShooter, // weapon
        SpawnRocketShooter, // weapon

        SpawnBlackhole, // weapon
        SpawnBlade, // projectile
        SpawnStickyBomb, // projectile

        UpgradeShooting,
        UpgradeDamage,
        UpgradeCooltime,

        SpreadBullets,
        SpreadReflectableBullets,
        SpreadTrackableBullets,
        SpreadTrackableMissiles,

        RushAttack,

        ShootMultipleLaser,
        ShootFewLaser,

        MultipleShockwave,
        Shockwave,
        MagneticField,
        SelfDestruction,

        SpreadMultipleBullets,
        ReviveImpact,
    }

    public enum Type
    {
        Basic,
        Active,
        Passive
    }

    public BaseSkill(Type skillType, int maxUpgradePoint)
    {
        _skillType = skillType;
        _maxUpgradePoint = maxUpgradePoint;
        _upgradePoint = 1;
        // 0, 1, 2, 3, 4 까지 도달
        // 0, 1, 2
    }

    protected IUpgradeVisitor _upgrader;

    protected IUseConstraintStrategy _useConstraintStrategy; // = new NoConstraintStrategy();
    protected ITargetStrategy _targetStrategy;
    protected IDelayStrategy _delayStrategy;
    protected ISkillActionStrategy _actionStrategy;

    protected Type _skillType;
    public Type SkillType { get { return _skillType; } }

    protected ICaster _caster;
    protected IUpgradeableSkillData _upgradeableRatio;

    int _maxUpgradePoint;
    public int MaxUpgradePoint { get { return _maxUpgradePoint; } }

    int _upgradePoint;
    public int UpgradePoint { get { return _upgradePoint; } }

    public virtual void Upgrade(int step)
    {
        for (int i = 0; i < step; i++) Upgrade();
    }

    public virtual void Upgrade() 
    {
        _upgradePoint++;
    }

    public virtual void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster) 
    {
        _upgradeableRatio = upgradeableRatio;
        _caster = caster; 
    }

    public void AddViewEvent(Action<float, int, bool> viewEvent) 
    { 
        _useConstraintStrategy.AddViewEvent(viewEvent);
    }

    public void RemoveViewEvent(Action<float, int, bool> viewEvent) 
    {
        _useConstraintStrategy.RemoveViewEvent(viewEvent);
    }

    public bool CanUse() 
    {
        return _useConstraintStrategy.CanUse();
    }

    public void Use()
    {
        _useConstraintStrategy.Use();
    }

    public virtual void OnAdd() 
    {
        _actionStrategy.OnAdd();
    }

    public virtual bool OnReflect(GameObject targetObject, Vector3 contactPos) 
    { 
        return _actionStrategy.OnReflect(targetObject, contactPos); 
    }

    public virtual void OnDamaged(float ratio) 
    {
        _delayStrategy.OnDamaged(ratio);
    }

    public virtual void OnRevive() 
    {
        _actionStrategy.OnRevive();
    }

    public virtual void OnCaptureEnter(ITarget target) 
    {
        _targetStrategy.OnTargetEnter(target);
    }

    public virtual void OnCaptureExit(ITarget target) 
    {
        _targetStrategy.OnTargetExit(target);
    }

    public virtual void OnCaptureEnter(ITarget target, IDamageable damageable) 
    {
        _targetStrategy.OnTargetEnter(target, damageable);
    }

    public virtual void OnCaptureExit(ITarget target, IDamageable damageable) 
    {
        _targetStrategy.OnTargetExit(target, damageable);
    }

    public virtual void OnUpdate()
    {
        _useConstraintStrategy.OnUpdate();
    }
}
