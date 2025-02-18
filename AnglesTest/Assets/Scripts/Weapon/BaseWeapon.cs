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
        HexatricBullet,

        ShooterRocket,

        Blackhole,
        StickyBomb,

        RifleShooter,
        RocketShooter,

        TrackableMissile,
    }

    protected IAttackStrategy _attackStrategy;
    protected ILifetimeStrategy _lifeTimeStrategy;
    protected ISizeStrategy _sizeStrategy;

    protected virtual void Update()
    {
        _lifeTimeStrategy.CheckFinish();
    }

    // 이하 5개는 스킬에서 데이터를 받아서 사용할 수 있게 만들기
    public virtual void InjectData(RocketData data) { }
    public virtual void InjectData(BulletData data) { }
    public virtual void InjectData(StickyBombData data) { }
    public virtual void InjectData(BladeData data) { }
    public virtual void InjectData(BlackholeData data) { }
    public virtual void InjectData(ShooterData data) { }
    public virtual void InjectData(TrackableMissileData data) { }

    //protected ICaster _caster;

    public virtual void Activate() 
    {
        //_caster = caster;
        _lifeTimeStrategy.Activate();
        _sizeStrategy.ResetSize();
    }

    public virtual void ModifyData(BladeDataModifier modifier) { }
    public virtual void ModifyData(BlackholeDataModifier modifier) { }
    public virtual void ModifyData(StickyBombDataModifier modifier) { }
    public virtual void ModifyData(ShooterDataModifier modifier) { }
    public virtual void ModifyData(BulletDataModifier modifier) { }
    public virtual void ModifyData(RocketDataModifier modifier) { }
    public virtual void ModifyData(TrackableMissileDataModifier modifier) { }

    public virtual void Initialize() { }
    public virtual void Initialize(BaseFactory factory) { }

    public virtual void ResetFollower(IFollowable followable) { }
    public virtual void ResetPosition(Vector3 pos) { transform.position = pos; }
    public virtual void ResetPosition(Vector3 pos, Vector3 direction) { transform.position = pos; transform.right = direction; }
}