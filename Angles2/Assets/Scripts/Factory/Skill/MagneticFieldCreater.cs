using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MagneticFieldUpgradableData
{
    public MagneticFieldUpgradableData(float damage, float delay)
    {
        _damage = damage;
        _delay = delay;
    }

    private float _damage;
    private float _delay;

    public float Damage { get => _damage; }
    public float Delay { get => _delay; }
}

[System.Serializable]
public class MagneticFieldData : SkillData
{
    public List<MagneticFieldUpgradableData> _upgradableDatas;
    public List<ITarget.Type> _targetTypes;

    public MagneticFieldData(int maxUpgradePoint, List<MagneticFieldUpgradableData> upgradableDatas, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _upgradableDatas = upgradableDatas;
        _targetTypes = targetTypes;
    }
}

public class MagneticFieldCreater : SkillCreater
{
    public MagneticFieldCreater(SkillData data) : base(data)
    {
    }

    public override BaseSkill Create()
    {
        MagneticFieldData data = _skillData as MagneticFieldData;
        return new MagneticField(data);
    }
}