using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseSkillData 
{
    public int _maxUpgradePoint;

    public BaseSkillData(int maxUpgradePoint)
    {
        _maxUpgradePoint = maxUpgradePoint;
    }
}

[System.Serializable] 
public class RandomSkillData : BaseSkillData
{
    public float _probability;

    public RandomSkillData(int maxUpgradePoint, float probability) : base(maxUpgradePoint)
    {
        _probability = probability;
    }
}

[System.Serializable] 
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

public class SkillCreater : BaseCreater<BaseSkill> { }

public class SkillFactory : MonoBehaviour
{
    Dictionary<BaseSkill.Name, SkillCreater> _skillCreaters;

    private static SkillFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        _skillCreaters = new Dictionary<BaseSkill.Name, SkillCreater>();
        Initialize();
    }

    private void Initialize()
    {
        _skillCreaters[BaseSkill.Name.Statikk] = new StatikkCreater();
        _skillCreaters[BaseSkill.Name.Knockback] = new KnockbackCreater();
        _skillCreaters[BaseSkill.Name.Impact] = new ImpactCreater();

        _skillCreaters[BaseSkill.Name.SpawnBlackhole] = new SpawnBlackholeCreater();
        _skillCreaters[BaseSkill.Name.SpawnBlade] = new SpawnBladeCreater();
        _skillCreaters[BaseSkill.Name.SpawnShooter] = new SpawnShooterCreater();
        _skillCreaters[BaseSkill.Name.SpawnStickyBomb] = new SpawnStickyBombCreater();


        _skillCreaters[BaseSkill.Name.SpreadBullets] = new SpreadBulletsCreater();
        _skillCreaters[BaseSkill.Name.Shockwave] = new ShockwaveCreater();
        _skillCreaters[BaseSkill.Name.MagneticField] = new MagneticFieldCreater();
        _skillCreaters[BaseSkill.Name.SelfDestruction] = new SelfDestructionCreater();

        _skillCreaters[BaseSkill.Name.ContactAttack] = new ContactAttackCreater();
    }

    public static BaseSkill Create(BaseSkill.Name name)
    {
        return _instance._skillCreaters[name].Create();
    }
}
