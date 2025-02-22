using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class BombData : LifeData
{
    public float _damage;
    public float _attackRange;
    public float _groggyDuration;

    public BombData(float maxHp, float damage, float attackRange, float groggyDuration) : base(maxHp, ITarget.Type.Construction, BaseEffect.Name.TriangleDestroyEffect)
    {
        _damage = damage;
        _attackRange = attackRange;
        _groggyDuration = groggyDuration;
    }

    public override LifeData Copy()
    {
        return new BombData(_maxHp, _damage, _attackRange, _groggyDuration);
    }
}

public class Bomb : BaseLife
{
    BombData _data;

    public void ResetData(BombData data)
    {
        _data = data;
    }

    public void Initialize(BaseFactory effectFactory)
    {
        _effectFactory = effectFactory;
    }

    protected override void OnDie()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
        // 이펙트와 함께 적용

        DamageableData attackDamageData = new DamageableData
        (
            new DamageStat(_data._damage),
            _data._groggyDuration
        );

        Damage.HitCircleRange(attackDamageData, transform.position, _data._attackRange, true, Color.green, 3);
    }
}
