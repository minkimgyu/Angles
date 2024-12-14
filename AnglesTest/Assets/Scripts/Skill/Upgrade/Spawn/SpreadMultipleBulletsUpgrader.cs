using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadMultipleBulletsUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float delay, float damage, float force)
        {
            _delay = delay;
            _damage = damage;
            _force = force;
        }

        public float _delay;
        public float _damage;
        public float _force;
    }

    List<UpgradableData> _datas;

    public SpreadMultipleBulletsUpgrader(List<UpgradableData> datas)
    {
        _datas = datas;
    }

    public void Visit(ISkillUpgradable upgradable, SpreadMultipleBulletsData data)
    {
        data._delay += _datas[upgradable.UpgradePoint - 1]._delay;
        data._damage += _datas[upgradable.UpgradePoint - 1]._damage;
        data._force += _datas[upgradable.UpgradePoint - 1]._force;
    }
}
