using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagneticFieldData : BaseSkillData
{
    public float _delay;
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public MagneticFieldData(int maxUpgradePoint, float damage, float range, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _delay = delay;
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class MagneticFieldCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        MagneticFieldData data = Database.Instance.SkillDatas[BaseSkill.Name.MagneticField] as MagneticFieldData;
        return new MagneticField(data);
    }
}