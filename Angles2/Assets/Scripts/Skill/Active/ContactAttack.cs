using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class ContactAttack : BaseSkill
{
    ContactAttackData _data;
    BaseFactory _effectFactory;

    public ContactAttack(ContactAttackData data, BaseFactory effectFactory) : base(Type.Basic, data._maxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        Debug.Log("ContactAttack");

        Vector3 contactPos = collision.contacts[0].point;
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(contactPos);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data._damage,
                _upgradeableRatio.AttackDamage,
                _data._adRatio,
                _upgradeableRatio.TotalDamageRatio),
            _data._targetTypes,
            _data._groggyDuration
        );

        Damage.Hit(damageData, damageable);
    }
}
