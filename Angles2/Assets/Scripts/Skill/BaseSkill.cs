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
        UpgradeDash,
        UpgradeDamage,
        UpgradeCooltime,

        SpreadBullets,
        MultipleShockwave,
        Shockwave,
        MagneticField,
        SelfDestruction
    }

    public enum Type
    {
        Passive,
        Active,
        Basic
    }

    public BaseSkill(Type skillType, int maxUpgradePoint)
    {
        _skillType = skillType;
        _maxUpgradePoint = maxUpgradePoint;
        _upgradePoint = 0;
    }

    protected IUpgradeVisitor _upgrader;
    protected UseConstraintComponent _useConstraint = new NoConstraintComponent();

    protected Type _skillType;
    public Type SkillType { get { return _skillType; } }

    protected SkillController.CastingData _castingData;
    protected IUpgradeableRatio _upgradeableRatio;

    int _maxUpgradePoint;
    public int MaxUpgradePoint { get { return _maxUpgradePoint; } }

    int _upgradePoint;
    public int UpgradePoint { get { return _upgradePoint; } }

    public bool CanUpgrade() { return _upgradePoint < _maxUpgradePoint; }

    public virtual void Upgrade(int step)
    {
        for (int i = 0; i < step; i++) Upgrade();
    }

    public virtual void Upgrade() 
    {
        _upgradePoint++;
    }

    public void Initialize(SkillController.CastingData castingData, IUpgradeableRatio upgradeableRatio ) 
    { _castingData = castingData; _upgradeableRatio = upgradeableRatio; }

    public void AddViewEvent(Action<float, int, bool> viewEvent) 
    { 
        _useConstraint.AddViewEvent(viewEvent); 
    }

    public void RemoveViewEvent(Action<float, int, bool> viewEvent) 
    { 
        _useConstraint.RemoveViewEvent(viewEvent); 
    }

    public virtual bool CanUse() { return true; }

    public virtual void OnReflect(Collision2D collision) { }

    public virtual void OnCaptureEnter(ITarget target) { }
    public virtual void OnCaptureExit(ITarget target) { }

    public virtual void OnCaptureEnter(ITarget target, IDamageable damageable) { }
    public virtual void OnCaptureExit(ITarget target, IDamageable damageable) { }

    public virtual void OnUpdate() 
    {
        _useConstraint.OnUpdate();
    }

    public abstract void OnAdd();
}
