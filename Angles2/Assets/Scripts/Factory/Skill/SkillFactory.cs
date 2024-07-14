using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillCreaterInput
{
    public TextAsset _jsonAsset;
}

[System.Serializable]
public class BaseSkillData
{
    public BaseSkillData(float probability)
    {
        _probability = probability;
    }

    public float _probability;
}

public class SkillCreater<T> : BaseCreater<SkillCreaterInput, BaseSkill>
{
    protected T _data;
    JsonParser _jsonParser;

    public override void Initialize(SkillCreaterInput input)
    {
        TextAsset asset = input._jsonAsset;

        _jsonParser = new JsonParser();
        _data = _jsonParser.JsonToData<T>(asset.text);
    }
}

public class SkillFactory : MonoBehaviour
{
    [SerializeField] SkillInputDictionary _skillInputs; // 무기 prefab을 모아서 넣어준다.
    Dictionary<BaseSkill.Name, BaseCreater<SkillCreaterInput, BaseSkill>> _skillCreaters;

    private static SkillFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        _skillCreaters = new Dictionary<BaseSkill.Name, BaseCreater<SkillCreaterInput, BaseSkill>>();
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

        foreach (var input in _skillInputs)
        {
            _skillCreaters[input.Key].Initialize(input.Value);
        }
    }

    public static BaseSkill Create(BaseSkill.Name name)
    {
        return _instance._skillCreaters[name].Create();
    }
}
