using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackhole : BaseSkill
{
    List<ITarget.Type> _targetTypes;
    BaseFactory _weaponFactory;

    BlackholeData _data;

    public SpawnBlackhole(SpawnBlackholeData data, SpawnBlackholeUpgrader upgrader, BaseFactory weaponFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _data = data._data;
        _targetTypes = data._targetTypes;

        _upgradeVisitor = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgradeVisitor.Visit(this, _data);
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blackhole);
        if (weapon == null) return;

        weapon.ResetData(_data);
        weapon.Upgrade(UpgradePoint); // 생성 후 업그레이드 적용
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
