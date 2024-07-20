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

    public void Initialize()
    {
        _castingData = new CastingData(gameObject, transform);
        _skillDictionary = new Dictionary<BaseSkill.Name, BaseSkill>();
    }

    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        List<SkillUpgradeData> skillUpgradeDatas = new List<SkillUpgradeData>();

        foreach (var item in _skillDictionary)
        {
            SkillUpgradeData skillUpgradeData = new SkillUpgradeData(item.Key, item.Value.UpgradePoint, item.Value.MaxUpgradePoint);
            skillUpgradeDatas.Add(skillUpgradeData);
        }

        return skillUpgradeDatas;
    }

    public void AddSkill(BaseSkill.Name skillName)
    {
        BaseSkill skill = SkillFactory.Create(skillName);
        skill.Initialize(_castingData);
        skill.OnAdd();

        _skillDictionary.Add(skillName, skill);
    }

    public void AddSkill(List<BaseSkill.Name> skillNames)
    {
        for (int i = 0; i < skillNames.Count; i++)
        {
            BaseSkill skill = SkillFactory.Create(skillNames[i]);
            skill.Initialize(_castingData);
            skill.OnAdd();

            _skillDictionary.Add(skillNames[i], skill);
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
