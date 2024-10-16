using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Knockback : BaseSkill
{
    BaseFactory _effectFactory;
    KnockbackData _data;

    public Knockback(KnockbackData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
        _upgrader = upgrader;
    }

    public override void OnAdd()
    {
        _useConstraint = new CooltimeConstraint(_data, _upgradeableRatio);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnReflect(Collision2D collision) 
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;


        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Knockback);
        Debug.Log("Knockback");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.KnockbackEffect);
        effect.ResetSize(_data._rangeMultiplier);
        effect.ResetPosition(_castingData.MyTransform.position, _castingData.MyTransform.right);
        effect.Play();

        DamageableData damageData =

        new DamageableData.DamageableDataBuilder().
        SetDamage(new DamageData(_data._damage, _upgradeableRatio.TotalDamageRatio))
        .SetTargets(_data._targetTypes)
        .SetGroggyDuration(_data._groggyDuration)
        .Build();

        Damage.HitBoxRange(damageData, _castingData.MyTransform.position, _data._offset.V2, _castingData.MyTransform.right,
            _data._size.V2 * _data._rangeMultiplier, true, Color.red);
    }
}
