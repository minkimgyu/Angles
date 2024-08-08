using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BaseSkillData 
{
    public int _maxUpgradePoint;

    public BaseSkillData(int maxUpgradePoint)
    {
        _maxUpgradePoint = maxUpgradePoint;
    }
}

[Serializable] 
public class RandomSkillData : BaseSkillData
{
    public float _probability;

    public RandomSkillData(int maxUpgradePoint, float probability) : base(maxUpgradePoint)
    {
        _probability = probability;
    }
}

[Serializable] 
public class CooltimeSkillData : BaseSkillData
{
    public int _maxStackCount;
    public float _coolTime;

    public CooltimeSkillData(int maxUpgradePoint, float coolTime, int maxStackCount) : base(maxUpgradePoint)
    {
        _coolTime = coolTime;
        _maxStackCount = maxStackCount;
    }
}

public class SkillCreater
{
    protected BaseSkillData _skillData;
    public SkillCreater(BaseSkillData data) { _skillData = data; }
    // 생성자에 이팩트 생성을 받는 이벤트를 추가해준다.

    public virtual BaseSkill Create() { return default; }
}

public class SkillFactory
{
    Dictionary<BaseSkill.Name, SkillCreater> _skillCreaters;

    public SkillFactory(Dictionary<BaseSkill.Name, BaseSkillData> skillDatas, Func<BaseEffect.Name, BaseEffect> CreateEffect, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon)
    {
        _skillCreaters = new Dictionary<BaseSkill.Name, SkillCreater>();

        _skillCreaters[BaseSkill.Name.Statikk] = new StatikkCreater(skillDatas[BaseSkill.Name.Statikk], CreateEffect);
        _skillCreaters[BaseSkill.Name.Knockback] = new KnockbackCreater(skillDatas[BaseSkill.Name.Knockback], CreateEffect);
        _skillCreaters[BaseSkill.Name.Impact] = new ImpactCreater(skillDatas[BaseSkill.Name.Impact], CreateEffect);

        _skillCreaters[BaseSkill.Name.SpawnBlackhole] = new SpawnBlackholeCreater(skillDatas[BaseSkill.Name.SpawnBlackhole], CreateWeapon);
        _skillCreaters[BaseSkill.Name.SpawnBlade] = new SpawnBladeCreater(skillDatas[BaseSkill.Name.SpawnBlade], CreateWeapon);

        _skillCreaters[BaseSkill.Name.SpawnRifleShooter] = new SpawnRifleShooterCreater(skillDatas[BaseSkill.Name.SpawnRifleShooter], CreateWeapon);
        _skillCreaters[BaseSkill.Name.SpawnRocketShooter] = new SpawnRocketShooterCreater(skillDatas[BaseSkill.Name.SpawnRocketShooter], CreateWeapon);


        _skillCreaters[BaseSkill.Name.SpawnStickyBomb] = new SpawnStickyBombCreater(skillDatas[BaseSkill.Name.SpawnStickyBomb], CreateWeapon);


        _skillCreaters[BaseSkill.Name.SpreadBullets] = new SpreadBulletsCreater(skillDatas[BaseSkill.Name.SpreadBullets], CreateWeapon);
        _skillCreaters[BaseSkill.Name.Shockwave] = new ShockwaveCreater(skillDatas[BaseSkill.Name.Shockwave], CreateEffect);
        _skillCreaters[BaseSkill.Name.MagneticField] = new MagneticFieldCreater(skillDatas[BaseSkill.Name.MagneticField]);
        _skillCreaters[BaseSkill.Name.SelfDestruction] = new SelfDestructionCreater(skillDatas[BaseSkill.Name.SelfDestruction], CreateEffect);

        _skillCreaters[BaseSkill.Name.ContactAttack] = new ContactAttackCreater(skillDatas[BaseSkill.Name.ContactAttack], CreateEffect);
    }

    public BaseSkill Create(BaseSkill.Name name)
    {
        return _skillCreaters[name].Create();
    }
}
