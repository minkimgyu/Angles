using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : BaseWeapon
{
    AbsorbableCaptureComponent _absorbCaptureComponent;
    BlackholeData _data;

    public override void ResetPosition(Vector3 pos)
    {
        base.ResetPosition(pos);
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Blackhole, pos);
    }

    public override void InjectData(BlackholeData data)
    {
        _data = data;
    }

    public override void ModifyData(BlackholeDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }

    public override void Initialize() 
    {
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, () => { Destroy(gameObject); });
        _sizeStrategy = new ChangeableSizeStrategy(transform, _data);
        _attackStrategy = new BlackholeAttackStrategy(_data, transform);

        _absorbCaptureComponent = GetComponentInChildren<AbsorbableCaptureComponent>();
        _absorbCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(IForce absorbable, IDamageable damageable, ITarget target)
    {
        _attackStrategy.OnTargetEnter(absorbable, damageable, target);
    }

    void OnExit(IForce absorbable, IDamageable damageable, ITarget target)
    {
        _attackStrategy.OnTargetExit(absorbable, damageable, target);
    }

    protected override void Update()
    {
        base.Update();
        _attackStrategy.OnUpdate();
    }
}
