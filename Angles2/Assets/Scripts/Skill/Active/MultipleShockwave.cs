using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MultipleShockwave : BaseSkill
{
    float _waveSizeMultiply;
    float _waveDelay;

    int _waveCount;
    float _waveSize;

    int _maxWaveCount;
    Timer _waveTimer;

    float _delay;
    float _damage;
    float _range;
    List<ITarget.Type> _targetTypes;
    List<ITarget> _targets;
    Timer _delayTimer;

    BaseFactory _effectFactory;

    public MultipleShockwave(MultipleShockwaveData data, BaseFactory effectFactory) : base(Type.Basic, data._maxUpgradePoint)
    {
        _waveSizeMultiply = data._waveDelay;
        _maxWaveCount = data._maxWaveCount;
        _waveDelay = data._waveDelay;
        _waveCount = 0;
        _waveSize = 1;

        _delay = data._delay;
        _damage = data._damage;
        _range = data._range;
        _targetTypes = data._targetTypes;

        _delayTimer = new Timer();
        _waveTimer = new Timer();
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

                if(_waveTimer.CurrentState == Timer.State.Ready || _waveTimer.CurrentState == Timer.State.Finish)
                {
                    BaseEffect effect = _effectFactory.Create(BaseEffect.Name.MultipleShockwaveEffect);
                    effect.ResetPosition(_castingData.MyTransform.position);
                    effect.Play();
                    effect.ResetSize(_waveSize);

                    DamageData damageData = new DamageData(_damage, _targetTypes, 0, false, Color.red);
                    Damage.HitCircleRange(damageData, _castingData.MyTransform.position, _range * _waveSize, true, Color.red, 3);

                    _waveTimer.Start(_waveDelay);
                    _waveCount++;
                    _waveSize += _waveSizeMultiply;
                    if (_waveCount == _maxWaveCount)
                    {
                        _waveCount = 0;
                        _waveTimer.Reset();
                        _delayTimer.Reset();
                    }    
                }

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