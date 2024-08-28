using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour, IUpgradable
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

    protected float _damage;
    protected List<ITarget.Type> _targetTypes;

    // ���� 5���� ��ų���� �����͸� �޾Ƽ� ����� �� �ְ� �����
    public virtual void ResetData(RocketData data) { }
    public virtual void ResetData(BulletData data) { }
    public virtual void ResetData(StickyBombData data) { }
    public virtual void ResetData(BladeData data) { }
    public virtual void ResetData(BlackholeData data) { }
    public virtual void ResetData(ShooterData data) { }

    public virtual void Initialize() { }
    public virtual void Initialize(System.Func<Name, BaseWeapon> SpawnWeapon) { }
    public virtual void Initialize(System.Func<BaseEffect.Name, BaseEffect> SpawnEffect) { }

    public virtual void ResetFollower(IFollowable followable) { }
    public virtual void ResetSize(float ratio) { transform.localScale *= ratio; }
    public virtual void ResetPosition(Vector3 pos) { transform.position = pos; }
    public virtual void ResetPosition(Vector3 pos, Vector3 direction) { transform.position = pos; transform.right = direction; }

    //public void ResetDamage(float damage) { _damage = damage; } --> �̰� ��� ���� ResetData�� ����Ѵ�.
    public void ResetTargetTypes(List<ITarget.Type> types) { _targetTypes = types; }


    protected int _maxUpgradePoint;
    public int MaxUpgradePoint { get { return _maxUpgradePoint; } }


    protected int _upgradePoint = 1;
    public int UpgradePoint { get { return _upgradePoint; } }

    public bool CanUpgrade() { return _upgradePoint < _maxUpgradePoint; }

    public virtual void Upgrade(int step)
    {
        _upgradePoint = step;
    }

    public virtual void Upgrade()
    {
        _upgradePoint++;
    }
}
