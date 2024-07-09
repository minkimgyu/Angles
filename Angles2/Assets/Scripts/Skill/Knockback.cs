using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Knockback : ActiveSkill
{
    Vector2 _size;
    Vector2 _offset;
    float _damage;
    List<ITarget.Type> _targetType;

    public Knockback(float probability, float damage, Vector2 size, Vector2 offset, List<ITarget.Type> damageableTypes)
    {
        _probability = probability;
        _damage = damage;
        _size = size;
        _offset = offset;
        _targetType = damageableTypes;
    }

    public override void OnReflect(Collision2D collision) 
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;
        Debug.Log("Knockback");

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Knockback);
        effect.ResetPosition(_castingData.MyTransform.position, _castingData.MyTransform.up);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetType);
        Damage.HitBoxRange(damageData, _castingData.MyTransform.position, _offset, _castingData.MyTransform.up, _size, true, Color.red);
    }
}
