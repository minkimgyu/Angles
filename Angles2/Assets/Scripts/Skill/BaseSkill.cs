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

abstract public class BaseSkill : IUpgradable
{
    public enum Name
    {
        Statikk,
        Knockback,
        Impact,
        ContactAttack,

        SpawnOrb,

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
        _upgradePoint = 1;
    }

    protected Type _skillType;
    public Type SkillType { get { return _skillType; } }

    protected CastingData _castingData;

    int _maxUpgradePoint;
    public int MaxUpgradePoint { get { return _maxUpgradePoint; } }


    int _upgradePoint;
    public int UpgradePoint { get { return _upgradePoint; } }

    public bool CanUpgrade() { return _upgradePoint < _maxUpgradePoint; }

    public virtual void Upgrade(int step)
    {
        _upgradePoint = step;
        OnUpgradeRequested();
    }

    public virtual void Upgrade() 
    {
        _upgradePoint++;
        OnUpgradeRequested();
    }

    public virtual void Initialize(CastingData data) { _castingData = data; }

    public Action<float, int, bool> ResetViewerValue;

    public virtual bool CanUse() { return true; }

    public virtual void OnReflect(Collision2D collision) { }

    public virtual void OnCaptureEnter(ITarget target) { }
    public virtual void OnCaptureExit(ITarget target) { }

    public virtual void OnCaptureEnter(ITarget target, IDamageable damageable) { }
    public virtual void OnCaptureExit(ITarget target, IDamageable damageable) { }

    public virtual void OnUpdate() { }
    public virtual void OnAdd() { }

    protected virtual void OnUpgradeRequested() { }
}
