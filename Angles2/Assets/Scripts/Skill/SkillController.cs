using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour, ICondition
{
    List<BaseSkill> _skills; // 사용 중인 스킬
    CastingData _castingData;

    public void Initialize()
    {
        _castingData = new CastingData(transform);
        _skills = new List<BaseSkill>();

        BaseSkill knockback = new Knockback(1, 100, new Vector2(5.5f, 3), new Vector2(1.5f, 0),
            new List<ITarget.Type> { ITarget.Type.Red }
        );

        BaseSkill impact = new Impact(1, 100, 5,
            new List<ITarget.Type> { ITarget.Type.Red }
        );

        BaseSkill stickBomb = new StickBomb(1, 100, 5, 3,
            new List<ITarget.Type> { ITarget.Type.Red }
        );

        BaseSkill statikk = new Statikk(1, 100, 5, 3,
            new List<ITarget.Type> { ITarget.Type.Red }
        );

        AddSkill(statikk);
    }

    public void AddSkill(BaseSkill skill)
    {
        skill.Initialize(_castingData);
        _skills.Add(skill);
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

    public void OnTrigger(Collider2D collider)
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].CanUse() == false) continue;
            _skills[i].OnTrigger(collider);
        }
    }

    public void OnAdd()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].CanUse() == false) continue;
            _skills[i].OnAdd();
        }
    }

    public void OnUpdate()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            _skills[i].OnUpdate();
        }
    }
}
