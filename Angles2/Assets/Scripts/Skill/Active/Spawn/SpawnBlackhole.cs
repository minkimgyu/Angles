using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackhole : RandomSkill
{
    List<ITarget.Type> _targetTypes;
    BaseFactory _weaponFactory;
    BlackholeData _blackholeData;

    public SpawnBlackhole(SpawnBlackholeData data, BaseFactory weaponFactory) : base(data._maxUpgradePoint, data._probability)
    {
        _blackholeData = data._data;
        _targetTypes = data._targetTypes;

        _weaponFactory = weaponFactory;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blackhole);
        if (weapon == null) return;

        weapon.ResetData(_blackholeData);
        weapon.Upgrade(UpgradePoint); // ���� �� ���׷��̵� ����
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
