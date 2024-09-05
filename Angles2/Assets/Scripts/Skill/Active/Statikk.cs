using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Statikk : CooltimeSkill
{
    List<ITarget.Type> _targetTypes;

    List<StatikkUpgradableData> _upgradableDatas;
    StatikkUpgradableData CurrentUpgradableData { get { return _upgradableDatas[UpgradePoint]; } }

    BaseFactory _effectFactory;

    public Statikk(StatikkData data, BaseFactory effectFactory) : base(data._maxUpgradePoint, data._coolTime, data._maxStackCount)
    {
        _upgradableDatas = data._upgradableDatas;
        _targetTypes = data._targetTypes;
        _effectFactory = effectFactory;
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
        DamageData damageData = new DamageData(CurrentUpgradableData.Damage, _targetTypes);

        Damage.HitRaycast(damageData, CurrentUpgradableData.MaxTargetCount, collision.transform.position, CurrentUpgradableData.Range, out hitPoints, true, Color.red, 3);

        for (int i = 0; i < hitPoints.Count; i++)
        {
            BaseEffect effect = _effectFactory.Create(BaseEffect.Name.LaserEffect);
            effect.ResetPosition(collision.transform.position);
            effect.ResetLine(hitPoints[i]);

            effect.Play();
        }
    }
}
