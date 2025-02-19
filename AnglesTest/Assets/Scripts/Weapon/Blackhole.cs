using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : BaseWeapon
{
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
        modifier.Visit(_data);
    }

    public override void InitializeStrategy()
    {
        AbsorbableCaptureComponent absorbCaptureComponent = GetComponentInChildren<AbsorbableCaptureComponent>();
        _targetStrategy = new ForceTargetingStrategy(absorbCaptureComponent, _data);
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, () => { Destroy(gameObject); });
        _sizeStrategy = new ChangeableSizeStrategy(transform, _data);
        _attackStrategy = new BlackholeAttackStrategy(_data, transform, _targetStrategy.GetForceTargets);
        _moveStrategy = new NoMoveStrategy();
    }
}
