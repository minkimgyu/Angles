using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class SelfDestruction : BaseSkill
{
    float _delay;
    float _damage;
    float _range;

    List<ITarget.Type> _targetTypes;
    Timer _delayTimer;

    BaseFactory _effectFactory;

    public SelfDestruction(SelfDestructionData data, BaseFactory effectFactory) : base(Type.Basic, data._maxUpgradePoint)
    {
        _damage = data._damage;
        _range = data._range;
        _targetTypes = data._targetTypes;

        _effectFactory = effectFactory;
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

            damageable.GetDamage(new DamageData(Damage.InstantDeathDamage));

            DamageData damageData = new DamageData(_damage);
            Damage.HitCircleRange(damageData, _castingData.MyObject.transform.position, _range, true, Color.red, 3);

            _delayTimer.Reset();
        }
    }

    public override void OnCaptureEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        if (_delayTimer.CurrentState != Timer.State.Ready) return;
        _delayTimer.Start(_delay);
    }
}
