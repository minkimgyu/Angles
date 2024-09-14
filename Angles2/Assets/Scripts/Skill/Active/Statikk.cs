using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Statikk : BaseSkill
{
    int _stackCount;
    BaseFactory _effectFactory;
    StatikkData _data;

    public Statikk(StatikkData data, StatikkUpgrader upgrader, BaseFactory effectFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _data = data;
        _useConstraint = new CooltimeConstraint(_data._maxStackCount, _data._coolTime);
        _upgradeVisitor = upgrader; // 이건 생성자에서 받아서 쓰기
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgradeVisitor.Visit(this, _data);
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        if (_useConstraint.CanUse() == false) return;
        _useConstraint.Use();
        Debug.Log("Statikk");

        List<Vector2> hitPoints;
        DamageData damageData = new DamageData(_data._damage, _data._targetTypes);

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
