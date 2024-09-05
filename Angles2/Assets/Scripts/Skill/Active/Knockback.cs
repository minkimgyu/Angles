using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Knockback : CooltimeSkill
{
    Vector2 _size;
    Vector2 _offset;
    List<ITarget.Type> _targetTypes;
    BaseFactory _effectFactory;

    List<KnockbackUpgradableData> _upgradableDatas;
    KnockbackUpgradableData CurrentUpgradableData { get { return _upgradableDatas[UpgradePoint]; } }

    public Knockback(KnockbackData data, BaseFactory effectFactory) : base(data._maxUpgradePoint, data._coolTime, data._maxStackCount)
    {
        _upgradableDatas = data._upgradableDatas;
        _size = new Vector2(data._size.x, data._size.y);
        _offset = new Vector2(data._offset.x, data._offset.y);
        _targetTypes = data._targetTypes;

        _effectFactory = effectFactory;
    }

    public override void OnReflect(Collision2D collision) 
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        _stackCount--;

        Debug.Log("Knockback");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.KnockbackEffect);
        effect.ResetSize(CurrentUpgradableData.Size);
        effect.ResetPosition(_castingData.MyTransform.position, _castingData.MyTransform.right);
        effect.Play();

        DamageData damageData = new DamageData(CurrentUpgradableData.Damage, _targetTypes);
        Damage.HitBoxRange(damageData, _castingData.MyTransform.position, _offset, _castingData.MyTransform.right, _size * CurrentUpgradableData.Size, true, Color.red);
    }
}
