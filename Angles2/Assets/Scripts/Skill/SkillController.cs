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

    public Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested;
    public Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested;

    public void Initialize()
    {
        _castingData = new CastingData(gameObject, transform);
        _skillDictionary = new Dictionary<BaseSkill.Name, BaseSkill>();
    }

    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        List<SkillUpgradeData> upgradeDatas = new List<SkillUpgradeData>();
        foreach (var item in _skillDictionary)
        {
            SkillUpgradeData upgradeData = new SkillUpgradeData(item.Key, item.Value.UpgradePoint, item.Value.MaxUpgradePoint);
            upgradeDatas.Add(upgradeData);
        }

        return upgradeDatas;
    }

    public void AddSkill(BaseSkill.Name name, BaseSkill skill)
    {
        bool alreadyHave = _skillDictionary.ContainsKey(name);
        if(alreadyHave)
        {
            _skillDictionary[name].Upgrade();
            return;
        }

        skill.Initialize(_castingData);
        skill.OnAdd();

        _skillDictionary.Add(name, skill);
        OnAddSkillRequested?.Invoke(name, skill);
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
