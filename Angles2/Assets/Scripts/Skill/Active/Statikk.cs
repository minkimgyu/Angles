using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Statikk : CooltimeSkill
{
    float _damage;
    float _range;
    int _maxTargetCount;
    List<ITarget.Type> _targetTypes;
    Func<BaseEffect.Name, BaseEffect> CreateEffect;

    public Statikk(StatikkData data, Func<BaseEffect.Name, BaseEffect> CreateEffect) : base(data._maxUpgradePoint, data._coolTime, data._maxStackCount)
    {
        _damage = data._damage;
        _range = data._range;
        _maxTargetCount = data._maxTargetCount;

        _targetTypes = data._targetTypes;
        this.CreateEffect = CreateEffect;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        if (_stackCount <= 0) return;
        _stackCount--;

        Debug.Log("Statikk");

        List<Vector2> hitPoints;
        DamageData damageData = new DamageData(_damage, _targetTypes);

        Damage.HitRaycast(damageData, _maxTargetCount, collision.transform.position, _range, out hitPoints, true, Color.red, 3);

        for (int i = 0; i < hitPoints.Count; i++)
        {
            BaseEffect effect = CreateEffect?.Invoke(BaseEffect.Name.LaserEffect);
            effect.ResetPosition(collision.transform.position);
            effect.ResetLine(hitPoints[i]);

            effect.Play();
        }
    }
}
