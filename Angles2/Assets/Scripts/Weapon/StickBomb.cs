using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class StickBomb : BaseWeapon
{
    Timer _lifeTimer = null;
    float _range = 0;
    float _explosionDelay = 0;

    IFollowable _followable;

    public override void Initialize(StickyBombData data) 
    {
        _damage = data._damage;
        _range = data._range;
        _explosionDelay = data._explosionDelay;

        _lifeTimer = new Timer();
        _lifeTimer.Start(_explosionDelay);
    }

    public override void ResetFollower(IFollowable followable) 
    {
        _followable = followable;
    }

    void Explode()
    {
        Debug.Log("Explode");

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Explosion);
        effect.ResetPosition(transform.position);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetTypes);
        Damage.HitCircleRange(damageData, transform.position, _range, true, Color.red, 3);
    }

    void Update()
    {
        if (_followable == null) return;

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
