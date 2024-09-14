using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : BaseSkill
{
    List<ITarget.Type> _targetTypes;
    List<IDamageable> _damageableTargets;
    Timer _delayTimer;

    List<MagneticFieldUpgradableData> _upgradableDatas;
    MagneticFieldUpgradableData CurrentUpgradableData { get { return _upgradableDatas[UpgradePoint]; } }

    public MagneticField(MagneticFieldData data) : base(Type.Basic, data._maxUpgradePoint)
    {
        _upgradableDatas = data._upgradableDatas;
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
                _delayTimer.Start(CurrentUpgradableData.Delay);
                break;
            case Timer.State.Finish:
                DamageData damageData = new DamageData(CurrentUpgradableData.Damage, _targetTypes, 0, false, Color.red);
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

        _damageableTargets.Add(damageable);
    }

    public override void OnCaptureExit(ITarget target, IDamageable damageable)
    {
        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        _damageableTargets.Remove(damageable);
    }
}
