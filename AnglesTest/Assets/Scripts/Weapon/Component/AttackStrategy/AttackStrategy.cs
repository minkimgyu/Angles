using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackStrategy
{
    void OnUpdate() { }

    void OnTargetEnter(ITarget target) { }
    void OnTargetEnter(IDamageable damageable) { }
    void OnTargetEnter(IForce absorbable, IDamageable damageable, ITarget target) { }
    void OnTargetEnter(Collider2D collision2D) { }

    void OnTargetExit(ITarget target) { }
    void OnTargetExit(IDamageable damageable) { }
    void OnTargetExit(IForce absorbable, IDamageable damageable, ITarget target) { }
    void OnTargetExit(Collider2D collision2D) { }

    BaseWeapon CreateProjectileWeapon() { return null; }
    void OnLifetimeCompleted() { }
}

public class NoAttackStrategy : IAttackStrategy
{
}