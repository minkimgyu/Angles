using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    List<BaseSkill> _skills; // 사용 중인 스킬
    CastingData _castingData;

    public void Initialize()
    {
        _castingData = new CastingData(gameObject, transform);
        _skills = new List<BaseSkill>();
    }

    public void AddSkill(BaseSkill skill)
    {
        // 만들어서 넣어주기
        skill.Initialize(_castingData);
        skill.OnAdd();
        _skills.Add(skill);
    }

    public void AddSkill(List<BaseSkill.Name> skillNames)
    {
        for (int i = 0; i < skillNames.Count; i++)
        {
            BaseSkill skill = SkillFactory.Create(skillNames[i]);
            AddSkill(skill);
        }
    }

    public void RemoveSkill(BaseSkill skill)
    {
        _skills.Remove(skill);
    }

    public void OnReflect(Collision2D collision)
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].CanUse() == false) continue;
            _skills[i].OnReflect(collision);
        }
    }

    public void OnUpdate()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            _skills[i].OnUpdate();
        }
    }

    public void OnCaptureEnter(ITarget target)
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].CanUse() == false) continue;
            _skills[i].OnCaptureEnter(target);
        }
    }

    public void OnCaptureExit(ITarget target)
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].CanUse() == false) continue;
            _skills[i].OnCaptureEnter(target);
        }
    }

    public void OnCaptureEnter(ITarget target, IDamageable damageable) 
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].CanUse() == false) continue;
            _skills[i].OnCaptureEnter(target, damageable);
        }
    }

    public void OnCaptureExit(ITarget target, IDamageable damageable) 
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].CanUse() == false) continue;
            _skills[i].OnCaptureExit(target, damageable);
        }
    }
}
