using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct SkillUpgradeData
{
    public SkillUpgradeData(BaseSkill.Name name, int upgradeCount, int maxUpgradeCount)
    {
        _name = name;
        _upgradeCount = upgradeCount;
        _maxUpgradeCount = maxUpgradeCount;
    }

    BaseSkill.Name _name;
    public BaseSkill.Name Name { get { return _name; } }

    int _upgradeCount;
    public int UpgradeCount { get { return _upgradeCount; } }


    int _maxUpgradeCount;
    public int MaxUpgradeCount { get { return _maxUpgradeCount; } }

}

public class SkillController : MonoBehaviour
{
    Dictionary<BaseSkill.Name, BaseSkill> _skillDictionary;
    CastingData _castingData;

    Action<BaseSkill.Name, BaseSkill> AddViewer;
    Action<BaseSkill.Name, BaseSkill> RemoveViewer;

    public void Initialize()
    {
        _castingData = new CastingData(gameObject, transform);
        _skillDictionary = new Dictionary<BaseSkill.Name, BaseSkill>();
    }

    public void Initialize(Action<BaseSkill.Name, BaseSkill> AddViewer, Action<BaseSkill.Name, BaseSkill> RemoveViewer)
    {
        _castingData = new CastingData(gameObject, transform);
        _skillDictionary = new Dictionary<BaseSkill.Name, BaseSkill>();

        this.AddViewer = AddViewer;
        this.RemoveViewer = RemoveViewer;
    }

    // µñ¼Å³Ê¸®·Î º¯°æ
    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        List<SkillUpgradeData> upgradeDatas = new List<SkillUpgradeData>();

        List<BaseSkill.Name> upgradeableSkills = Database.Instance.UpgradeableSkills;

        for (int i = 0; i < upgradeableSkills.Count; i++)
        {
            BaseSkill.Name key = upgradeableSkills[i];
            
            if(_skillDictionary.ContainsKey(key))
            {
                bool canUpgrade = _skillDictionary[key].CanUpgrade();
                if (canUpgrade == false) continue;

                SkillUpgradeData skillUpgradeData = new SkillUpgradeData(key, _skillDictionary[key].UpgradePoint, _skillDictionary[key].MaxUpgradePoint);
                upgradeDatas.Add(skillUpgradeData);
            }
            else
            {
                BaseSkillData skillData = Database.Instance.SkillDatas[key];

                SkillUpgradeData skillUpgradeData = new SkillUpgradeData(key, 0, skillData._maxUpgradePoint);
                upgradeDatas.Add(skillUpgradeData);
            }
              
        }

        return upgradeDatas;
    }

    public void AddSkill(BaseSkill.Name skillName)
    {
        bool alreadyHave = _skillDictionary.ContainsKey(skillName);
        if(alreadyHave)
        {
            _skillDictionary[skillName].Upgrade();
            return;
        }

        BaseSkill skill = SkillFactory.Create(skillName);
        skill.Initialize(_castingData);
        skill.OnAdd();

        _skillDictionary.Add(skillName, skill);
        AddViewer?.Invoke(skillName, skill);
    }

    public void AddSkill(List<BaseSkill.Name> skillNames)
    {
        for (int i = 0; i < skillNames.Count; i++)
        {
            AddSkill(skillNames[i]);
        }
    }

    public void OnReflect(Collision2D collision)
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnReflect(collision);
        }
    }

    public void OnUpdate()
    {
        foreach (var skill in _skillDictionary)
        {
            skill.Value.OnUpdate();
        }
    }

    public void OnCaptureEnter(ITarget target)
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnCaptureEnter(target);
        }
    }

    public void OnCaptureExit(ITarget target)
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnCaptureExit(target);
        }
    }

    public void OnCaptureEnter(ITarget target, IDamageable damageable) 
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnCaptureEnter(target, damageable);
        }
    }

    public void OnCaptureExit(ITarget target, IDamageable damageable) 
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnCaptureExit(target, damageable);
        }
    }
}
