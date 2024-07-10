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

        BaseSkill impact = SkillFactory.Create(BaseSkill.Name.Impact);
        BaseSkill knockback = SkillFactory.Create(BaseSkill.Name.Knockback);
        BaseSkill statikk = SkillFactory.Create(BaseSkill.Name.Statikk);

        BaseSkill spawnBlackhole = SkillFactory.Create(BaseSkill.Name.SpawnBlackhole);
        BaseSkill spawnBlade = SkillFactory.Create(BaseSkill.Name.SpawnBlade);
        BaseSkill spawnShooter = SkillFactory.Create(BaseSkill.Name.SpawnShooter);
        BaseSkill spawnStickyBomb = SkillFactory.Create(BaseSkill.Name.SpawnStickyBomb);

        AddSkill(spawnBlade);
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
