using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Statikk : BaseSkill
{
    BaseFactory _effectFactory;
    StatikkData _data;

    public Statikk(StatikkData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader; // 이건 생성자에서 받아서 쓰기
        _effectFactory = effectFactory; // 이건 생성자에서 받아서 쓰기
    }

    public override void OnAdd()
    {
        _useConstraintStrategy = new CooltimeConstraintStrategy(_data, _upgradeableRatio);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override bool OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return false;

        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return false;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Statikk);

        List<Vector2> hitPoints;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat
            (
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data.TargetTypes,
            _data.GroggyDuration
        );
        Damage.HitRaycast(damageData, _data.MaxTargetCount, targetObject.transform.position, _data.Range, out hitPoints, true, Color.red, 3);

        for (int i = 0; i < hitPoints.Count; i++)
        {
            BaseEffect effect = _effectFactory.Create(BaseEffect.Name.LaserEffect);
            effect.ResetColor(new Color(93f / 255f, 177f / 255f, 255f / 255f), new Color(255f / 255f, 255f / 255f, 255f / 255f));
            effect.ResetPosition(Vector3.zero);
            effect.ResetLine(targetObject.transform.position, hitPoints[i]);

            effect.Play();
        }

        return true;
    }
}
