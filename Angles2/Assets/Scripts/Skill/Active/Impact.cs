using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Impact : ActiveSkill
{
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public Impact(ImpactData data) : base(data._probability)
    {
        _damage = data._damage;
        _range = data._range;
        _targetTypes = data._targetTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;
        Debug.Log("Impact");

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Impact);
        effect.ResetPosition(_castingData.MyTransform.position, _castingData.MyTransform.right);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetTypes);

        Damage.HitCircleRange(damageData, _castingData.MyTransform.position, _range, true, Color.red, 3);
    }
}
