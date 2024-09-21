using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : BaseSkill
{
    List<IDamageable> _damageableTargets;
    Timer _delayTimer;

    MagneticFieldData _data;

    public MagneticField(MagneticFieldData data, IUpgradeVisitor upgrader) : base(Type.Basic, data._maxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;

        _damageableTargets = new List<IDamageable>();
        _delayTimer = new Timer();
    }

    public override void OnAdd() 
    {
        _useConstraint = new NoConstraintComponent();
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnUpdate()
    {
        if (_delayTimer.CurrentState != Timer.State.Running && _damageableTargets.Count == 0) return;

        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                _delayTimer.Start(_data._delay);
                break;
            case Timer.State.Finish:

                DamageableData damageData =
                new DamageableData.DamageableDataBuilder().
                SetDamage(new DamageData(_data._damage, _upgradeableRatio.TotalDamageRatio))
                .SetTargets(_data._targetTypes)
                .Build();

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
        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        _damageableTargets.Add(damageable);
    }

    public override void OnCaptureExit(ITarget target, IDamageable damageable)
    {
        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        _damageableTargets.Remove(damageable);
    }
}
