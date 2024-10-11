using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class SelfDestruction : BaseSkill
{
    Timer _delayTimer;

    SelfDestructionData _data;
    BaseFactory _effectFactory;

    public SelfDestruction(SelfDestructionData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Basic, data._maxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _effectFactory = effectFactory;
        _delayTimer = new Timer();
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnDamaged(float ratio)
    {
        if (ratio > _data._hpRatioOnInvoke) return;
        if (_delayTimer.CurrentState != Timer.State.Ready) return;

        _delayTimer.Start(_data._delay);
        CastingComponent castingComponent = _castingData.MyObject.GetComponent<CastingComponent>();
        if (castingComponent == null) return;

        castingComponent.CastSkill(_data._delay);
    }

    public override void OnUpdate()
    {
        if (_delayTimer.CurrentState == Timer.State.Finish)
        {
            Debug.Log("SelfDestruction");
            BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
            if (effect == null) return;

            effect.ResetPosition(_castingData.MyObject.transform.position);
            effect.Play();

            IDamageable damageable = _castingData.MyObject.GetComponent<IDamageable>();
            if (damageable == null) return;

            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, _castingData.MyTransform.position, 0.3f);

            DamageableData selfDamage = new DamageableData.DamageableDataBuilder().
            SetDamage(new DamageData(Damage.InstantDeathDamage, _upgradeableRatio.TotalDamageRatio))
            .Build();

            damageable.GetDamage(selfDamage);

            DamageableData damageData =
            new DamageableData.DamageableDataBuilder().
            SetDamage(new DamageData(_data._damage, _upgradeableRatio.TotalDamageRatio))
            .SetTargets(_data._targetTypes)
            .Build();

            Damage.HitCircleRange(damageData, _castingData.MyObject.transform.position, _data._range, true, Color.red, 3);
        }
    }
}
