using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseWeapon : MonoBehaviour
{
    public enum Name
    {
        Blade,

        ShooterBullet,
        PentagonBullet,
        PentagonicBullet,
        HexahornBullet,

        ShooterRocket,

        Blackhole,
        StickyBomb,

        RifleShooter,
        RocketShooter,

        TrackableMissile,
    }

    protected IWeaponTargetingStrategy _targetingStrategy;
    protected IWeaponDetectingStrategy _detectingStrategy;
    protected IWeaponActionStrategy _actionStrategy;
    protected IWeaponMoveStrategy _moveStrategy;

    protected ILifetimeStrategy _lifeTimeStrategy;
    protected ISizeStrategy _sizeStrategy;

    protected virtual void Update()
    {
        _moveStrategy.OnUpdate();
        _lifeTimeStrategy.OnUpdate();
        _actionStrategy.OnUpdate();
    }

    protected virtual void FixedUpdate()
    {
        _moveStrategy.OnFixedUpdate();
    }

    // 이하 5개는 스킬에서 데이터를 받아서 사용할 수 있게 만들기
    public virtual void InjectData(RocketData data) { }
    public virtual void InjectData(BulletData data) { }
    public virtual void InjectData(StickyBombData data) { }
    public virtual void InjectData(BladeData data) { }
    public virtual void InjectData(BlackholeData data) { }
    public virtual void InjectData(ShooterData data) { }
    public virtual void InjectData(TrackableMissileData data) { }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _targetingStrategy.OnTriggerEnter(collider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _moveStrategy.OnCollisionEnter(collision);
    }

    public virtual void Activate() 
    {
        _lifeTimeStrategy.OnActivate();
        _sizeStrategy.OnActivate();
    }

    public virtual void ModifyData(BladeDataModifier modifier) { }
    public virtual void ModifyData(BlackholeDataModifier modifier) { }
    public virtual void ModifyData(StickyBombDataModifier modifier) { }
    public virtual void ModifyData(ShooterDataModifier modifier) { }
    public virtual void ModifyData(BulletDataModifier modifier) { }
    public virtual void ModifyData(RocketDataModifier modifier) { }
    public virtual void ModifyData(TrackableMissileDataModifier modifier) { }

    public virtual void InitializeStrategy()
    {
        _targetingStrategy = new NoTargetingStrategy();
        _detectingStrategy = new NoDetectingStrategy();
        _actionStrategy = new NoAttackStrategy();
        _moveStrategy = new NoMoveStrategy();
        _lifeTimeStrategy = new NoLifetimeStrategy();
        _sizeStrategy = new NoSizeStrategy();
    }

    public virtual void Initialize() { InitializeStrategy(); }
    public virtual void Initialize(BaseFactory factory) { InitializeStrategy(); }

    public virtual void InjectFollower(IFollowable followable) 
    {
        _moveStrategy.InjectFollower(followable);
    }

    public virtual void ResetPosition(Vector3 pos) { transform.position = pos; }
    public virtual void ResetPosition(Vector3 pos, Vector3 direction) { transform.position = pos; transform.right = direction; }
}