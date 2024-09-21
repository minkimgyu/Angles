using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class StickyBomb : BaseWeapon
{
    Timer _lifeTimer = null;
    IFollowable _followable;

    BaseFactory _effectFactory;

    StickyBombData _data;

    public override void ResetData(StickyBombData data)
    {
        _data = data;
    }

    public override void ModifyData(List<WeaponDataModifier> modifiers)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            _data = modifiers[i].Visit(_data);
        }
    }

    public override void Initialize(BaseFactory effectFactory) 
    {
        _effectFactory = effectFactory;
        _lifeTimer = new Timer();
    }

    public override void ResetFollower(IFollowable followable) 
    {
        _followable = followable;
    }

    void Explode()
    {
        Debug.Log("Explode");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.ResetSize(_data._range);
        effect.Play();

        DamageableData damageData =

        new DamageableData.DamageableDataBuilder().
        SetDamage(new DamageData(_data._damage, _data._totalDamageRatio))
        .SetTargets(_data._targetTypes)
        .Build();

        Damage.HitCircleRange(damageData, transform.position, _data._range, true, Color.red, 3);
    }

    void Update()
    {
        if (_followable as UnityEngine.Object == null) return;

        bool canAttach = _followable.CanFollow();
        if (canAttach == true)
        {
            Vector3 pos = _followable.ReturnPosition();
            transform.position = pos;
        }

        if (_lifeTimer.CurrentState == Timer.State.Running) return;

        Explode();
        Destroy(gameObject);
        return;
    }
}
