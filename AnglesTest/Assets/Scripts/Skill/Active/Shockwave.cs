using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Shockwave : BaseSkill
{
    ShockwaveData _data;
    BaseFactory _effectFactory;
    Timer _delayTimer;
    List<ITarget> _targets;

    public Shockwave(ShockwaveData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _delayTimer = new Timer();
        _targets = new List<ITarget>();

        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnUpdate()
    {
        BaseEffect effect;

        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_targets.Count == 0) return;

                CastingComponent castingComponent = _caster.GetComponent<CastingComponent>();
                if (castingComponent != null) castingComponent.CastSkill(_data.Delay);

                _delayTimer.Start(_data.Delay);
                break;
            case Timer.State.Finish:

                Transform casterTransform = _caster.GetComponent<Transform>();
                ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Shockwave, casterTransform.position, 0.6f);

                effect = _effectFactory.Create(BaseEffect.Name.ShockwaveEffect);
                effect.ResetPosition(casterTransform.position);
                effect.Play();

                DamageableData damageData = new DamageableData
                (
                    _caster,
                    new DamageStat(
                        _data.Damage,
                        _upgradeableRatio.AttackDamage,
                        _data.AdRatio,
                        _upgradeableRatio.TotalDamageRatio
                    ),
                    _data.TargetTypes
                );

                Damage.HitCircleRange(damageData, casterTransform.position, _data.Range * _data.SizeMultiplier, true, Color.red, 3);

                _delayTimer.Reset();
                break;
            default:
                break;
        }
    }

    public override void OnCaptureEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        _targets.Add(target);
    }

    public override void OnCaptureExit(ITarget target)
    {
        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        _targets.Remove(target);
    }
}
