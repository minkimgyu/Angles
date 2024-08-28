using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackhole : RandomSkill
{
    List<ITarget.Type> _targetTypes;
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;
    BlackholeData _blackholeData;

    public SpawnBlackhole(SpawnBlackholeData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data._maxUpgradePoint, data._probability)
    {
        _blackholeData = data._data;
        _targetTypes = data._targetTypes;

        this.CreateWeapon = CreateWeapon;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = CreateWeapon?.Invoke(BaseWeapon.Name.Blackhole);
        if (weapon == null) return;

        weapon.ResetData(_blackholeData);
        weapon.Upgrade(_upgradePoint); // 생성 후 업그레이드 적용
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
