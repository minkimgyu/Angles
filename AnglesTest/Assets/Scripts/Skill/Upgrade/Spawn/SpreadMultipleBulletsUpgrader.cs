using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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

    [JsonProperty] List<UpgradableData> _datas;

    public SpreadMultipleBulletsUpgrader(List<UpgradableData> datas)
    {
        _datas = datas;
    }

    public SpreadMultipleBulletsUpgrader()
    {
    }

    public void Visit(ISkillUpgradable upgradable, SpreadMultipleBulletsData data)
    {
        data.Delay += _datas[upgradable.UpgradePoint - 1]._delay;
        data.Damage += _datas[upgradable.UpgradePoint - 1]._damage;
        data.Force += _datas[upgradable.UpgradePoint - 1]._force;
    }
}
