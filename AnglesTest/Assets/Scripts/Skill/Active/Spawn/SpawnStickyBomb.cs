using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnStickyBomb : BaseSkill
{
    BaseFactory _weaponFactory;
    SpawnStickyBombData _data;

    public SpawnStickyBomb(SpawnStickyBombData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _upgrader = upgrader;
        _data = data;
        _weaponFactory = weaponFactory;
    }

    public override void OnAdd()
    {
        _useConstraint = new CooltimeConstraint(_data, _upgradeableRatio);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        IFollowable followable = targetObject.GetComponent<IFollowable>();
        if (followable == null) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.StickyBomb);
        if (weapon == null) return;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data.TargetTypes,
            _data.GroggyDuration
        );

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(damageData));
        modifiers.Add(new WeaponDelayModifier(_data.Delay));

        weapon.ModifyData(modifiers);
        weapon.Activate();
        weapon.ResetFollower(followable);
    }
}
