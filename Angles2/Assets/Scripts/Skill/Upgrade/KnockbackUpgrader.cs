using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float damage, float sizeMultiplier)
        {
            _damage = damage;
            _sizeMultiplier = sizeMultiplier;
        }

        private float _damage;
        private float _sizeMultiplier;

        public float Damage { get => _damage; }
        public float SizeMultiplier { get => _sizeMultiplier; }
    }

    List<UpgradableData> _upgradeDatas;

    public KnockbackUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(IUpgradable upgradable, KnockbackData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data += upgradeData;
    }
}
