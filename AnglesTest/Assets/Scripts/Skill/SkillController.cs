using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skill;

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

    // 1, 2, 3, 4, 5
    // 5

    public bool CanUpgrade() { return _upgradeCount < _maxUpgradeCount; } // 만약 같다면 더 이상 업그레이드 불가능
}

public class SkillController : MonoBehaviour
{
    Dictionary<BaseSkill.Name, BaseSkill> _skillDictionary;
    IUpgradeableSkillData _upgradeableRatio;
    ICaster _caster;

    public Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested;
    public Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested;

    public void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        _caster = caster;
        _upgradeableRatio = upgradeableRatio;
        _skillDictionary = new Dictionary<BaseSkill.Name, BaseSkill>();
    }
    

    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        List<SkillUpgradeData> upgradeDatas = new List<SkillUpgradeData>();
        foreach (var item in _skillDictionary)
        {
            SkillUpgradeData upgradeData = new SkillUpgradeData(item.Key, item.Value.UpgradePoint + 1, item.Value.MaxUpgradePoint); // 1을 더해서 준다.
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

        skill.Initialize(_upgradeableRatio, _caster);
        skill.OnAdd();
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.State.OnAddSkill, name, skill);

        _skillDictionary.Add(name, skill);
        OnAddSkillRequested?.Invoke(name, skill);
    }

    public void OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue; // 개수 맞는지 확인

            bool canUseReflectSkill = skill.Value.OnReflect(targetObject, contactPos, contactNormal);
            if (canUseReflectSkill == false) continue;

            skill.Value.Use(); // 사용 가능하다면 쓰기
        }
    }

    public void OnRevive()
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnRevive();
            skill.Value.Use();
        }
    }

    public void OnDamaged(float ratio)
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnDamaged(ratio);
            skill.Value.Use();
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
            skill.Value.Use();
        }
    }

    public void OnCaptureExit(ITarget target)
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnCaptureExit(target);
            skill.Value.Use();
        }
    }

    public void OnCaptureEnter(ITarget target, IDamageable damageable) 
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnCaptureEnter(target, damageable);
            skill.Value.Use();
        }
    }

    public void OnCaptureExit(ITarget target, IDamageable damageable) 
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnCaptureExit(target, damageable);
            skill.Value.Use();
        }
    }
}
