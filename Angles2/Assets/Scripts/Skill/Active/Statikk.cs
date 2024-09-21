using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Statikk : BaseSkill
{
    BaseFactory _effectFactory;
    StatikkData _data;

    public Statikk(StatikkData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader; // 이건 생성자에서 받아서 쓰기
        _effectFactory = effectFactory; // 이건 생성자에서 받아서 쓰기
    }

    public override void OnAdd()
    {
        _useConstraint = new CooltimeConstraint(_data, _upgradeableRatio);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        if (_useConstraint.CanUse() == false) return;
        _useConstraint.Use();

        List<Vector2> hitPoints;

        DamageableData damageData =
        new DamageableData.DamageableDataBuilder().
        SetDamage(new DamageData(_data._damage, _upgradeableRatio.TotalDamageRatio))
        .SetTargets(_data._targetTypes)
        .Build();

        Damage.HitRaycast(damageData, _data._maxTargetCount, collision.transform.position, _data._range, out hitPoints, true, Color.red, 3);

        for (int i = 0; i < hitPoints.Count; i++)
        {
            BaseEffect effect = _effectFactory.Create(BaseEffect.Name.LaserEffect);
            effect.ResetPosition(collision.transform.position);
            effect.ResetLine(hitPoints[i]);

            effect.Play();
        }
    }
}
