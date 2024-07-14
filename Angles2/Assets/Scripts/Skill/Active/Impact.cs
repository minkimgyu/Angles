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
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        Debug.Log("Impact");

        Vector3 contactPos = collision.contacts[0].point;
        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Impact);
        effect.ResetPosition(contactPos);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetTypes);
        Damage.HitCircleRange(damageData, contactPos, _range, true, Color.red, 3);
    }
}
