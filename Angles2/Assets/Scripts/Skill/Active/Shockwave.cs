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

    public Shockwave(ShockwaveData data, BaseFactory effectFactory) : base(Type.Basic, data._maxUpgradePoint)
    {
        _data = data;
        _delayTimer = new Timer();
        _targets = new List<ITarget>();

        _effectFactory = effectFactory;
    }

    public override void OnAdd()
    {
        _useConstraint = new NoConstraintComponent();
    }

    public override void OnUpdate()
    {
        BaseEffect effect;

        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_targets.Count == 0) return;

                CastingComponent castingComponent = _castingData.MyObject.GetComponent<CastingComponent>();
                if (castingComponent != null) castingComponent.CastSkill(_data._delay);

                _delayTimer.Start(_data._delay);
                break;
            case Timer.State.Finish:

                effect = _effectFactory.Create(BaseEffect.Name.ShockwaveEffect);
                effect.ResetPosition(_castingData.MyTransform.position);
                effect.Play();

                DamageableData damageData =
                new DamageableData.DamageableDataBuilder().
                SetDamage(new DamageData(_data._damage, _upgradeableRatio.TotalDamageRatio))
                .SetTargets(_data._targetTypes)
                .Build();

                Damage.HitCircleRange(damageData, _castingData.MyTransform.position, _data._range, true, Color.red, 3);

                _delayTimer.Reset();
                break;
            default:
                break;
        }
    }

    public override void OnCaptureEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        _targets.Add(target);
    }

    public override void OnCaptureExit(ITarget target)
    {
        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        _targets.Remove(target);
    }
}
