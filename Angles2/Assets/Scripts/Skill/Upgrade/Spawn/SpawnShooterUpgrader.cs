using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShooterUpgrader : IUpgradeVisitor
{
    public struct UpgradableData
    {
        public UpgradableData(float shootForce, float fireDelay)
        {
            _shootForce = shootForce; // ������ �ӵ�
            _fireDelay = fireDelay; // ���� �ӵ�
        }

        private float _shootForce;
        private float _fireDelay;

        public float ShootForce { get => _shootForce; }
        public float FireDelay { get => _fireDelay; }
    }

    List<UpgradableData> _upgradeDatas;

    public SpawnShooterUpgrader(List<UpgradableData> upgradeDatas)
    {
        _upgradeDatas = upgradeDatas;
    }

    public void Visit(IUpgradable upgradable, ShooterData data)
    {
        UpgradableData upgradeData = _upgradeDatas[upgradable.UpgradePoint - 1];
        data += upgradeData;
    }
}
