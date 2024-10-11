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

    // 1, 2, 3, 4, 5
    // 5

    public bool CanUpgrade() { return _upgradeCount <= _maxUpgradeCount; } // 만약 같다면 더 이상 업그레이드 불가능
}

public class SkillController : MonoBehaviour
{
    public class CastingData
    {
        public CastingData(GameObject myObject, Transform myTransform)
        {
            MyObject = myObject;
            MyTransform = myTransform;
        }

        public GameObject MyObject { get; }
        public Transform MyTransform { get; }
    }

    public class UpgradeableData
    {
        public UpgradeableData(float totalDamageRatio, float totalCooltimeRatio, float totalRandomRatio)
        {
            _totalDamageRatio = totalDamageRatio;
            _totalCooltimeRatio = totalCooltimeRatio;
            _totalRandomRatio = totalRandomRatio;
        }

        float _totalDamageRatio;
        public float TotalDamageRatio { get { return _totalDamageRatio; } set { _totalDamageRatio = value; } }

        float _totalCooltimeRatio;
        public float TotalCooltimeRatio { get { return _totalCooltimeRatio; } set { _totalCooltimeRatio = value; } }

        float _totalRandomRatio;
        public float TotalRandomRatio { get { return _totalRandomRatio; } set { _totalRandomRatio = value; } }
    }


    Dictionary<BaseSkill.Name, BaseSkill> _skillDictionary;
    CastingData _castingData;
    IUpgradeableRatio _upgradeableRatio;

    public Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested;
    public Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested;

    public void Initialize(IUpgradeableRatio upgradeableRatio)
    {
        _castingData = new CastingData(gameObject, transform);
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

        skill.Initialize(_castingData, _upgradeableRatio);
        skill.OnAdd();
        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.State.OnAddSkill, name, skill);

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

    public void OnDamaged(float ratio)
    {
        foreach (var skill in _skillDictionary)
        {
            if (skill.Value.CanUse() == false) continue;
            skill.Value.OnDamaged(ratio);
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
