using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackhole : BaseSkill
{
    BaseFactory _weaponFactory;
    SpawnBlackholeData _data;

    public SpawnBlackhole(SpawnBlackholeData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override void OnAdd()
    {
        _useConstraint = new RandomConstraintComponent(_data, _upgradeableRatio);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blackhole);
        if (weapon == null) return;

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponTargetCountModifier(_data._targetCount));
        modifiers.Add(new WeaponForceModifier(_data._force));
        modifiers.Add(new WeaponSizeModifier(_data._sizeMultiplier));
        modifiers.Add(new WeaponLifetimeModifier(_data._lifetime));
        modifiers.Add(new WeaponTargetModifier(_data._targetTypes));

        weapon.ModifyData(modifiers);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
