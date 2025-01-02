using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class ContactAttack : BaseSkill
{
    ContactAttackData _data;
    BaseFactory _effectFactory;

    public ContactAttack(ContactAttackData data, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
    }

    public override void OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return;

        IDamageable damageable = targetObject.GetComponent<IDamageable>();
        if (damageable == null) return;

        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        Debug.Log("ContactAttack");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(contactPos);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio),
            _data.TargetTypes,
            _data.GroggyDuration
        );

        Damage.Hit(damageData, damageable);
    }
}
