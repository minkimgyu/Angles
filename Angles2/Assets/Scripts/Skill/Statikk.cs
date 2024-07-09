using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Statikk : ActiveSkill
{
    float _damage;
    float _range;
    int _maxTargetCount;
    List<ITarget.Type> _targetType;

    public Statikk(float probability, float damage, float range, int maxTargetCount, List<ITarget.Type> damageableTypes)
    {
        _probability = probability;
        _damage = damage;
        _range = range;
        _maxTargetCount = maxTargetCount;

        _targetType = damageableTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;
        Debug.Log("Statikk");

        List<Vector2> hitPoints;
        DamageData damageData = new DamageData(_damage, _targetType);

        Damage.HitRaycast(damageData, _maxTargetCount, collision.transform.position, _range, out hitPoints, true, Color.red, 3);

        for (int i = 0; i < hitPoints.Count; i++)
        {
            BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Laser);
            effect.ResetPosition(collision.transform.position);
            effect.ResetLine(hitPoints[i]);

            effect.Play();
        }
    }
}
