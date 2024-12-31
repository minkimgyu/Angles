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

        Rocket,

        Blackhole,
        StickyBomb,

        RifleShooter,
        RocketShooter,
    }

    protected BaseLifetimeComponent _lifetimeComponent;
    protected BaseSizeModifyComponent _sizeModifyComponent;

    protected virtual void Update()
    {
        _lifetimeComponent.CheckFinish();
    }

    // 이하 5개는 스킬에서 데이터를 받아서 사용할 수 있게 만들기
    public virtual void ResetData(RocketData data) { }
    public virtual void ResetData(BulletData data) { }
    public virtual void ResetData(StickyBombData data) { }
    public virtual void ResetData(BladeData data) { }
    public virtual void ResetData(BlackholeData data) { }
    public virtual void ResetData(ShooterData data) { }

    //protected ICaster _caster;

    public virtual void Activate() 
    {
        //_caster = caster;
        _lifetimeComponent.Activate();
        _sizeModifyComponent.ResetSize();
    }

    public abstract void ModifyData(List<WeaponDataModifier> modifiers);

    public virtual void Initialize() { }
    public virtual void Initialize(BaseFactory factory) { }

    public virtual void ResetFollower(IFollowable followable) { }
    public virtual void ResetPosition(Vector3 pos) { transform.position = pos; }
    public virtual void ResetPosition(Vector3 pos, Vector3 direction) { transform.position = pos; transform.right = direction; }
}