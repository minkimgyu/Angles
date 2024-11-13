using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagneticFieldData : SkillData
{
    public float _damage;
    public float _adRatio;
    public float _delay;
    public List<ITarget.Type> _targetTypes;

    public MagneticFieldData(int maxUpgradePoint, float damage, float adRatio, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _delay = delay;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new MagneticFieldData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _damage,
            _adRatio,
            _delay,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class MagneticFieldCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;

    public MagneticFieldCreater(SkillData data, IUpgradeVisitor upgrader) : base(data)
    {
        _upgrader = upgrader;
    }

    public override BaseSkill Create()
    {
        MagneticFieldData data = CopySkillData.Copy() as MagneticFieldData;
        return new MagneticField(data, _upgrader);
    }
}