using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class ContactAttack : BaseSkill
{
    public float _damage;
    public List<ITarget.Type> _targetTypes;

    public ContactAttack(ContactAttackData data) : base(Type.Basic, data._maxUpgradePoint)
    {
        _damage = data._damage;
        _targetTypes = data._targetTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        Debug.Log("ContactAttack");

        Vector3 contactPos = collision.contacts[0].point;
        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(contactPos);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetTypes);
        Damage.HitContact(damageData, collision.gameObject);
    }
}
