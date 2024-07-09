using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Impact : ActiveSkill
{
    float _range;
    float _damage;
    List<ITarget.Type> _targetType;

    public Impact(float probability, float damage, float range, List<ITarget.Type> damageableTypes)
    {
        _probability = probability;
        _damage = damage;
        _range = range;
        _targetType = damageableTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;
        Debug.Log("Impact");

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Impact);
        effect.ResetPosition(_castingData.MyTransform.position);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetType);

        Damage.HitCircleRange(damageData, _castingData.MyTransform.position, _range, true, Color.red, 3);
    }
}
