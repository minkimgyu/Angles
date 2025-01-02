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

    public SelfDestruction(SelfDestructionData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
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
        if (ratio > _data.HpRatioOnInvoke) return;
        if (_delayTimer.CurrentState != Timer.State.Ready) return;

        _delayTimer.Start(_data.Delay);
        CastingComponent castingComponent = _caster.GetComponent<CastingComponent>();
        if (castingComponent == null) return;

        castingComponent.CastSkill(_data.Delay);
    }

    public override void OnUpdate()
    {
        if (_delayTimer.CurrentState == Timer.State.Finish)
        {
            Debug.Log("SelfDestruction");
            BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
            if (effect == null) return;

            Transform casterTransform = _caster.GetComponent<Transform>();

            effect.ResetPosition(casterTransform.position);
            effect.Play();

            IDamageable damageable = _caster.GetComponent<IDamageable>();
            if (damageable == null) return;

            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, casterTransform.position, 0.3f);

            DamageableData selfDamageData = new DamageableData(_caster, new DamageStat(Damage.InstantDeathDamage));
            Damage.Hit(selfDamageData, damageable);

            DamageableData damageData = new DamageableData
            (
                _caster,
               new DamageStat
               (
                    _data.Damage,
                    _upgradeableRatio.AttackDamage,
                    _data.AdRatio,
                    _upgradeableRatio.TotalDamageRatio
               ),
                _data.TargetTypes
            );


            Damage.HitCircleRange(damageData, casterTransform.position, _data.Range, true, Color.red, 3);
        }
    }
}
