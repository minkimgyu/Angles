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
    }

    public override void OnAdd()
    {
        _useConstraint = new NoConstraintComponent();
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

            DamageableData damageData =

            new DamageableData.DamageableDataBuilder().
            SetDamage(new DamageData(Damage.InstantDeathDamage, _upgradeableRatio.TotalDamageRatio))
            .SetTargets(_data._targetTypes)
            .Build();

            Damage.HitCircleRange(damageData, _castingData.MyObject.transform.position, _data._range, true, Color.red, 3);

            _delayTimer.Reset();
        }
    }

    public override void OnCaptureEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        if (_delayTimer.CurrentState != Timer.State.Ready) return;
        _delayTimer.Start(_data._delay);
    }
}
