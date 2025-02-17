using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackhole : BaseSkill
{
    BaseFactory _weaponFactory;
    SpawnBlackholeData _data;

    public SpawnBlackhole(SpawnBlackholeData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Active, data.MaxUpgradePoint)
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

    public override bool OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return false;

        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return false;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blackhole);
        if (weapon == null) return false;

        Transform casterTransform = _caster.GetComponent<Transform>();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage
            ),
            _data.TargetTypes,
            _data.GroggyDuration
        );

        BlackholeDataModifier blackholeDataModifier = new BlackholeDataModifier(damageData, _data.SizeMultiplier, _data.Lifetime);

        weapon.ModifyData(blackholeDataModifier);
        weapon.Activate();

        weapon.ResetPosition(casterTransform.position);
        return true;
    }
}
