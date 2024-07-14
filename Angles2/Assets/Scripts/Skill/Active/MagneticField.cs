using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : ActiveSkill
{
    float _delay;
    float _damage;
    List<ITarget.Type> _targetTypes;
    List<IDamageable> _damageableTargets;
    Timer _delayTimer;

    public MagneticField(MagneticFieldData data) : base(data._probability)
    {
        _delay = data._delay;
        _damage = data._damage;
        _targetTypes = data._targetTypes;

        _damageableTargets = new List<IDamageable>();
        _delayTimer = new Timer();
    }

    public override void OnUpdate()
    {
        if (_delayTimer.CurrentState != Timer.State.Running && _damageableTargets.Count == 0) return;

        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                _delayTimer.Start(_delay);
                break;
            case Timer.State.Finish:
                DamageData damageData = new DamageData(_damage, _targetTypes, 0, Color.red);
                for (int i = 0; i < _damageableTargets.Count; i++)
                {
                    _damageableTargets[i].GetDamage(damageData);
                }
                _delayTimer.Reset();
                break;
            default:
                break;
        }
    }

    public override void OnCaptureEnter(ITarget target, IDamageable damageable)
    {
        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        Debug.Log(damageable);
        _damageableTargets.Add(damageable);
    }

    public override void OnCaptureExit(ITarget target, IDamageable damageable)
    {
        Debug.Log(damageable);

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        Debug.Log(damageable);
        _damageableTargets.Remove(damageable);
    }
}
