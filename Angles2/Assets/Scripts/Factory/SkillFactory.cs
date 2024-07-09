using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillCreaterInput
{
    public TextAsset _jsonAsset;
}

public class BaseSkillData
{

}

public class SkillCreater : BaseCreater<SkillCreaterInput, BaseSkill>
{
    protected BaseSkillData _data;
    JsonParser _jsonParser;

    public override void Initialize(SkillCreaterInput input)
    {
        TextAsset asset = input._jsonAsset;
        _data = _jsonParser.JsonToData<BaseSkillData>(asset.text);
    }
}

public class SkillFactory : MonoBehaviour
{
    [SerializeField] SkillInputDictionary _skillInputs; // 무기 prefab을 모아서 넣어준다.
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
        _skillCreaters[BaseSkill.Name.Impact] = new SkillCreater();
    }

    public static BaseSkill Create(BaseSkill.Name name)
    {
        return _instance._skillCreaters[name].Create();
    }
}
