using Skill.Strategy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponActionStrategy
{
    void OnUpdate() { }
    void Execute(List<IDamageable> damageables, DamageableData damageableData) { }
    void Execute(IDamageable damageable, DamageableData damageableData) { }
}

public class NoAttackStrategy : IWeaponActionStrategy
{
}

public class HitTargetStrategy : IWeaponActionStrategy
{
    public void Execute(List<IDamageable> damageables, DamageableData damageableData)
    {
        if (damageables == null) return;

        for (int i = 0; i < damageables.Count; i++)
        {
            damageables[i].GetDamage(damageableData);
        }
    }

    public void Execute(IDamageable damageable, DamageableData damageableData)
    {
        if (damageable == null) return;
        damageable.GetDamage(damageableData);
    }
}