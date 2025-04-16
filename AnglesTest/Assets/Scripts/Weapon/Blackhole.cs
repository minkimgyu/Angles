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
        _sizeStrategy.ChangeSize(_data.SizeMultiplier);
        _lifeTimeStrategy.ChangeLifetime(_data.Lifetime);
        _detectingStrategy.InjectTargetTypes(_data.TargetTypes);
    }

    public override void InitializeStrategy()
    {
        base.InitializeStrategy();
        AbsorbableCaptureComponent absorbCaptureComponent = GetComponentInChildren<AbsorbableCaptureComponent>();
        _detectingStrategy = new BlackholeDetectingStrategy(absorbCaptureComponent);
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(() => { Destroy(gameObject); });
        _sizeStrategy = new ChangeableSizeStrategy(transform);
        _actionStrategy = new BlackholeAttackStrategy(_data, transform, _detectingStrategy.GetForceTargets);
        _moveStrategy = new NoMoveStrategy();
    }
}
