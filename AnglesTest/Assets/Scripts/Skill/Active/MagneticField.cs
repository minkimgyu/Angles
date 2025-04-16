using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;

public class MagneticField : BaseSkill
{
    List<IDamageable> _damageableTargets;
    Timer _delayTimer;

    MagneticFieldData _data;

    public MagneticField(MagneticFieldData data, IUpgradeVisitor upgrader) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;

        _damageableTargets = new List<IDamageable>();
        _delayTimer = new Timer();
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _detectingStrategy = new DamageableDetectingStrategy(_data.TargetTypes);
        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio
        );

        _delayStrategy = new DelayRoutineStrategy(
           _data.Delay,
           () =>
           {
               return _detectingStrategy.DetectDamageables().Count != 0;
           },
           () => {
               _actionStrategy.Execute(_detectingStrategy.DetectDamageables(), new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));
           }
       );
    }
}
