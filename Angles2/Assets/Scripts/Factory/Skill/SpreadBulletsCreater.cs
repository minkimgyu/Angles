using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpreadBulletsData : BaseSkillData
{
    public float _damage;
    public float _force;
    public float _delay;

    public float _distanceFromCaster;
    public int _bulletCount;

    public List<ITarget.Type> _targetTypes;

    public SpreadBulletsData(int maxUpgradePoint, float damage, float force, float delay, float distanceFromCaster, int bulletCount, List<ITarget.Type> targetTypes) : base(maxUpgradePoint) 
    {
        _damage = damage;
        _force = force;
        _delay = delay;
        _distanceFromCaster = distanceFromCaster;

        _bulletCount = bulletCount;

        _targetTypes = targetTypes;
    }
}

public class SpreadBulletsCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        SpreadBulletsData data = Database.Instance.SkillDatas[BaseSkill.Name.SpreadBullets] as SpreadBulletsData;
        return new SpreadBullets(data);
    }
}
