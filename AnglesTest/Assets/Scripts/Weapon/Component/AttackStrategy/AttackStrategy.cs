using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackStrategy
{
    void OnUpdate() { }
    void OnTargetEnter(Collider2D collision2D) { }

    BaseWeapon CreateProjectileWeapon() { return null; }
    void OnLifetimeCompleted() { }
}

public class NoAttackStrategy : IAttackStrategy
{
}