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

    public Knockback(KnockbackData data) : base(data._probability)
    {
        _probability = data._probability;
        _damage = data._damage;
        _size = new Vector2(data._size.x, data._size.y);
        _offset = new Vector2(data._offset.x, data._offset.y);
        _targetType = data._targetType;
    }

    public override void OnReflect(Collision2D collision) 
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;
        Debug.Log("Knockback");

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Knockback);
        effect.ResetPosition(_castingData.MyTransform.position, _castingData.MyTransform.right);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetType);
        Damage.HitBoxRange(damageData, _castingData.MyTransform.position, _offset, _castingData.MyTransform.right, _size, true, Color.red);
    }
}
