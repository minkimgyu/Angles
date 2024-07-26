using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Knockback : CooltimeSkill
{
    Vector2 _size;
    Vector2 _offset;
    float _damage;
    List<ITarget.Type> _targetTypes;

    public Knockback(KnockbackData data) : base(data._maxUpgradePoint, data._coolTime, data._maxStackCount)
    {
        _damage = data._damage;
        _size = new Vector2(data._size.x, data._size.y);
        _offset = new Vector2(data._offset.x, data._offset.y);
        _targetTypes = data._targetTypes;
    }

    public override void OnReflect(Collision2D collision) 
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        _stackCount--;

        Debug.Log("Knockback");

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.KnockbackEffect);
        effect.ResetPosition(_castingData.MyTransform.position, _castingData.MyTransform.right);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetTypes);
        Damage.HitBoxRange(damageData, _castingData.MyTransform.position, _offset, _castingData.MyTransform.right, _size, true, Color.red);
    }
}
