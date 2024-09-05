using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Shockwave : BaseSkill
{
    float _delay;
    float _damage;
    float _range;
    List<ITarget.Type> _targetTypes;
    List<ITarget> _targets;
    Timer _delayTimer;

    BaseFactory _effectFactory;

    public Shockwave(ShockwaveData data, BaseFactory effectFactory) : base(Type.Basic, data._maxUpgradePoint)
    {
        _delay = data._delay;
        _damage = data._damage;
        _range = data._range;
        _targetTypes = data._targetTypes;

        _delayTimer = new Timer();
        _targets = new List<ITarget>();

        _effectFactory = effectFactory;
    }

    public override void OnUpdate()
    {
        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_targets.Count == 0) return;

                _delayTimer.Start(_delay);
                break;
            case Timer.State.Finish:

                BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ShockwaveEffect);
                effect.ResetPosition(_castingData.MyTransform.position);
                effect.Play();

                DamageData damageData = new DamageData(_damage, _targetTypes, 0, false, Color.red);
                Damage.HitCircleRange(damageData, _castingData.MyTransform.position, _range, true, Color.red, 3);

                _delayTimer.Reset();
                break;
            default:
                break;
        }
    }

    public override void OnCaptureEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        _targets.Add(target);
    }

    public override void OnCaptureExit(ITarget target)
    {
        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        _targets.Remove(target);
    }
}
