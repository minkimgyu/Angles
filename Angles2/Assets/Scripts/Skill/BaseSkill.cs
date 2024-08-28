using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct CastingData
{
    public CastingData(GameObject myObject, Transform myTransform)
    {
        _myObject = myObject;
        _myTransform = myTransform;
    }

    GameObject _myObject;
    public GameObject MyObject { get { return _myObject; } }

    Transform _myTransform;
    public Transform MyTransform { get { return _myTransform; } }
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

    protected int _maxUpgradePoint;
    public int MaxUpgradePoint { get { return _maxUpgradePoint; } }


    protected int _upgradePoint;
    public int UpgradePoint { get { return _upgradePoint; } }

    public bool CanUpgrade() { return _upgradePoint < _maxUpgradePoint; }

    public virtual void Upgrade(int step)
    {
        _upgradePoint = step;
    }

    public virtual void Upgrade() 
    {
        _upgradePoint++;
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
}
