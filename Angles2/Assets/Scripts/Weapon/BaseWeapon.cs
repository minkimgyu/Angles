using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public enum Name
    {
        Blade,
        Bullet,
        Rocket,

        Blackhole,
        StickyBomb,

        ShotgunShooter,
        RifleShooter,
        RocketShooter,
    }

    //protected float _damage;
    protected List<ITarget.Type> _targetTypes;

    // 이하 5개는 스킬에서 데이터를 받아서 사용할 수 있게 만들기
    public virtual void ResetData(RocketData data) { }
    public virtual void ResetData(BulletData data) { }
    public virtual void ResetData(StickyBombData data) { }
    public virtual void ResetData(BladeData data) { }
    public virtual void ResetData(BlackholeData data) { }
    public virtual void ResetData(ShooterData data) { }

    public virtual void Initialize() { }
    public virtual void Initialize(BaseFactory factory) { }

    public virtual void ResetFollower(IFollowable followable) { }
    public virtual void ResetSize(float ratio) { transform.localScale *= ratio; }
    public virtual void ResetPosition(Vector3 pos) { transform.position = pos; }
    public virtual void ResetPosition(Vector3 pos, Vector3 direction) { transform.position = pos; transform.right = direction; }

    //public void ResetDamage(float damage) { _damage = damage; } --> 이거 대신 위의 ResetData를 사용한다.
    public void ResetTargetTypes(List<ITarget.Type> types) { _targetTypes = types; }
}
