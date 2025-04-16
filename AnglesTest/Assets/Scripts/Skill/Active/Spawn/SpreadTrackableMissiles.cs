using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;
using static UnityEngine.GraphicsBuffer;

public class SpreadTrackableMissiles : BaseSkill
{
    BaseFactory _weaponFactory;
    SpreadTrackableMissilesData _data;
    List<ITarget> _targets;

    public SpreadTrackableMissiles(SpreadTrackableMissilesData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _targets = new List<ITarget>();
        _weaponFactory = weaponFactory;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _detectingStrategy = new Skill.Strategy.TargetDetectingStrategy(_data.TargetTypes);

        _actionStrategy = new SpreadMissileStrategy(
            _caster,
           _upgradeableRatio,
           _data.BulletCount,
           _data.AdRatio,
           0,
           _data.DistanceFromCaster,
           _data.BulletName,
           _weaponFactory,
           _data.TargetTypes);

        _soundStrategy = new PlaySoundStrategy(ISoundPlayable.SoundName.SpreadBullets);

        _delayStrategy = new DelayRoutineStrategy(
           _data.Delay,
           () =>
           {
               return _detectingStrategy.DetectTargets().Count > 0;
           },
           () =>
           {
               CastingComponent castingComponent = _caster.GetComponent<CastingComponent>();
               if (castingComponent == null) return;

               castingComponent.CastSkill(_data.Delay);
           },
           () =>
           {
               bool canDetect = _detectingStrategy.DetectTargets().Count > 0;
               if (canDetect == false) return;

               _actionStrategy.Execute(new SpreadMissileStrategy.ChangeableData(_data.Damage), _detectingStrategy.DetectTargets());
               _soundStrategy.PlaySound();
           }
       );
    }
}
