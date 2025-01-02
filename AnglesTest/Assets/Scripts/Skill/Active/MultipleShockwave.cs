using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MultipleShockwave : BaseSkill
{
    int _waveCount;

    const float _originWaveSize = 1;
    float _waveSize;

    Timer _waveTimer;

    List<ITarget.Type> _targetTypes;
    List<ITarget> _targets;
    Timer _delayTimer;

    MultipleShockwaveData _data;
    BaseFactory _effectFactory;

    public MultipleShockwave(MultipleShockwaveData data, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _waveCount = 0;
        _targetTypes = data.TargetTypes;

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

                _waveSize = _originWaveSize;
                _delayTimer.Start(_data.Delay);
                break;
            case Timer.State.Finish:

                if(_waveTimer.CurrentState == Timer.State.Ready || _waveTimer.CurrentState == Timer.State.Finish)
                {
                    Transform casterTransform = _caster.GetComponent<Transform>();

                    BaseEffect effect = _effectFactory.Create(BaseEffect.Name.MultipleShockwaveEffect);
                    effect.ResetPosition(casterTransform.position);
                    effect.Play();
                    effect.ResetSize(_waveSize);

                    ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Shockwave, casterTransform.position);

                    Debug.Log("MultipleShockwave " + _waveSize);
                    Debug.Log("MultipleShockwave " + _data.Range);

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


                    Damage.HitCircleRange(damageData, casterTransform.position, _data.Range * _waveSize, true, Color.red, 3);

                    _waveTimer.Reset();
                    _waveTimer.Start(_data.WaveDelay);
                    _waveSize += _data.WaveSizeMultiply;
                    _waveCount++;

                    Debug.Log(_waveCount);
                    if (_waveCount == _data.MaxWaveCount)
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