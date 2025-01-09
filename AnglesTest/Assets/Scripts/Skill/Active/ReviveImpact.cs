using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveImpact : BaseSkill
{
    ReviveImpactData _data;
    BaseFactory _effectFactory;

    public ReviveImpact(ReviveImpactData data, BaseFactory effectFactory) : base(Type.Passive, data.MaxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
    }

    public override void OnAdd()
    {
        _useConstraint = new NoConstraintComponent();
    }

    public override void OnRevive()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Impact, 0.7f);
        Debug.Log("ReviveImpact");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
        if (effect == null) return;

        Transform casterTransform = _caster.GetComponent<Transform>();

        effect.ResetPosition(casterTransform.position);
        effect.ResetSize(_data.RangeMultiplier);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data.TargetTypes,
            _data.GroggyDuration
        );
        Damage.HitCircleRange(damageData, casterTransform.position, _data.Range * _data.RangeMultiplier, true, Color.red, 3);
    }
}
