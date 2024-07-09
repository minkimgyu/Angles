using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Life : MonoBehaviour, IDamageable, ITarget
{
    [SerializeField] float _maxHp;
    [SerializeField] float _hp;

    public enum AliveState
    {
        Normal, // 일반
        Invincible, // 무적
        Groggy, // 기절
    }

    public enum LifeState
    {
        Alive,
        Die
    }

    protected ITarget.Type _targetType;

    protected LifeState _lifeState = LifeState.Alive;
    protected AliveState _aliveState = AliveState.Normal;

    protected virtual void SetInvincible(bool nowInvincible)
    {
        if (nowInvincible) _aliveState = AliveState.Invincible;
        else _aliveState = AliveState.Normal;
    }

    protected virtual void Start()
    {
        _hp = _maxHp;
    }

    protected abstract void OnDie();

    public virtual void GetHeal(float point)
    {
        _hp += point;
        if (_maxHp < _hp)
        {
            _hp = _maxHp;
        }
    }

    void SpawnDamageTxt(DamageData damageData)
    {
        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.DamageText);
        effect.Initialize();

        effect.ResetPosition(transform.position);
        effect.ResetText(damageData.Damage);

        effect.Play();
    }

    public virtual void GetDamage(DamageData damageData)
    {
        if (_lifeState == LifeState.Alive && _aliveState == AliveState.Invincible) return;

        bool canDamage = damageData.DamageableTypes.Contains(_targetType);
        if (canDamage == false) return;

        _hp -= damageData.Damage;
        SpawnDamageTxt(damageData);

        if (_hp < 0)
        {
            _hp = 0;
            _lifeState = LifeState.Die;
            OnDie();
        }
    }

    public Vector3 ReturnPosition() { return transform.position; }

    public ITarget.Type ReturnTargetType() { return _targetType; }
}
