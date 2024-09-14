using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct CastingData
{
    public CastingData(GameObject myObject, Transform myTransform, BuffFloat totalDamageRatio, BuffFloat totalCooltimeRatio)
    {
        MyObject = myObject;
        MyTransform = myTransform;
    }

    public GameObject MyObject { get; }
    public Transform MyTransform { get; }
}

// 무기는 Visitor 제거해주기
// PlayerData를 FSM 내부에서 캐싱해서 사용
// 
abstract public class BaseSkill : IUpgradable
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

        CreateShootingBuff,
        CreateDashBuff,
        CreateTotalDamageBuff,
        CreateTotalCooltimeBuff,

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

    protected IUpgradeVisitor _upgradeVisitor;
    protected UseConstraintComponent _useConstraint;

    protected Type _skillType;
    public Type SkillType { get { return _skillType; } }

    protected CastingData _castingData;

    int _maxUpgradePoint;
    public int MaxUpgradePoint { get { return _maxUpgradePoint; } }

    int _upgradePoint;
    public int UpgradePoint { get { return _upgradePoint; } }

    public bool CanUpgrade() { return _upgradePoint < _maxUpgradePoint; }

    public virtual void Upgrade() 
    {
        _upgradePoint++;
    }

    public virtual void Initialize(CastingData data) { _castingData = data; }

    public void AddViewEvent(Action<float, int, bool> viewEvent) { _useConstraint.AddViewEvent(viewEvent); }
    public void RemoveViewEvent(Action<float, int, bool> viewEvent) { _useConstraint.RemoveViewEvent(viewEvent); }

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

    public virtual void OnAdd() { }
}
