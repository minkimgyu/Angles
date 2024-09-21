using DamageUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBulletsUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float delay, float damage, float force, float bulletCount)
        {
            _delay = delay;
            _damage = damage;
            _force = force;
            _bulletCount = bulletCount;
        }

        public float _delay;
        public float _damage;
        public float _force;
        public float _bulletCount;
    }

    List<UpgradableData> _datas;

    public SpreadBulletsUpgrader(List<UpgradableData> datas)
    {
        _datas = datas;
    }

    public void Visit(ISkillUpgradable upgradable, SpreadBulletsData data) 
    {
        data._delay += _datas[upgradable.UpgradePoint - 1]._delay;
        data._damage += _datas[upgradable.UpgradePoint - 1]._damage;
        data._force += _datas[upgradable.UpgradePoint - 1]._force;
        data._bulletCount += _datas[upgradable.UpgradePoint - 1]._bulletCount;
    }
}
