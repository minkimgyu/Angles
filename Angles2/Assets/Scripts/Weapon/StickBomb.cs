using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class StickBomb : BaseWeapon
{
    Timer _lifeTimer = null;
    IFollowable _followable;

    BaseFactory _effectFactory;

    List<StickyBombUpgradableData> _upgradableDatas;
    StickyBombUpgradableData UpgradableData { get { return _upgradableDatas[_upgradePoint - 1]; } }

    public override void ResetData(StickyBombData data)
    {
        _upgradableDatas = data._upgradableDatas;
        _lifeTimer.Start(UpgradableData.ExplosionDelay);
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
        effect.ResetSize(UpgradableData.Range);
        effect.Play();

        DamageData damageData = new DamageData(UpgradableData.Damage, _targetTypes);
        Damage.HitCircleRange(damageData, transform.position, UpgradableData.Range, true, Color.red, 3);
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
