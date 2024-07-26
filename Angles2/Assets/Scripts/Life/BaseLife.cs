using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseLife : MonoBehaviour, IDamageable, ITarget
{
    protected float _maxHp;
    protected float _hp;

    protected Timer _groggyTimer;
    protected Action<float> OnHpChange;

    protected Action OnDieRequested;

    public enum Name
    {
        Player,
        Triangle,
        Rectangle,
        Pentagon,
        Hexagon
    }

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

    public void AddDieEvent(Action OnDieRequested) { this.OnDieRequested = OnDieRequested; }

    protected virtual void SetInvincible(bool nowInvincible)
    {
        if (nowInvincible) _aliveState = AliveState.Invincible;
        else _aliveState = AliveState.Normal;
    }

    public virtual void ResetData(PlayerData data) { }
    public virtual void ResetData(TriangleData data) { }
    public virtual void ResetData(RectangleData data) { }
    public virtual void ResetData(PentagonData data) { }
    public virtual void ResetData(HexagonData data) { }

    public virtual void Initialize() { }

    protected virtual void OnDie()
    {
        OnDieRequested?.Invoke();
    }

    public virtual void GetHeal(float point)
    {
        _hp += point;
        OnHpChange?.Invoke(_hp / _maxHp);

        if (_maxHp < _hp)
        {
            _hp = _maxHp;
        }
    }

    void SpawnDamageTxt(DamageData damageData)
    {
        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.DamageTextEffect);
        effect.Initialize();

        effect.ResetPosition(transform.position);
        effect.ResetText(damageData.Damage);
        effect.ResetColor(damageData.DamageTxtColor);

        effect.Play();
    }

    protected virtual void FixedUpdate()
    {
        if (_lifeState == LifeState.Die) return;
    }

    protected virtual void Update()
    {
        if (_lifeState == LifeState.Die) return;

        switch (_aliveState)
        {
            case AliveState.Normal:
                break;
            case AliveState.Invincible:
                break;
            case AliveState.Groggy:
                if (_groggyTimer.CurrentState == Timer.State.Finish)
                {
                    _aliveState = AliveState.Normal;
                    _groggyTimer.Reset();
                }
                break;
            default:
                break;
        }
    }

    public virtual void GetDamage(DamageData damageData)
    {
        if (_lifeState == LifeState.Alive && _aliveState == AliveState.Invincible) return;

        bool canDamage = damageData.DamageableTypes.Contains(_targetType);
        if (canDamage == false) return;

        _hp -= damageData.Damage;
        OnHpChange?.Invoke(_hp / _maxHp);

        if (_aliveState == AliveState.Normal && damageData.GroggyDuration > 0)
        {
            _aliveState = AliveState.Groggy; // 일정 시간 후 복귀
            _groggyTimer.Start(damageData.GroggyDuration);
        }

        SpawnDamageTxt(damageData);

        if (_hp <= 0)
        {
            _hp = 0;
            _lifeState = LifeState.Die;
            OnDie();
        }
    }

    public Vector3 ReturnPosition() { return transform.position; }

    public bool IsTarget(List<ITarget.Type> types)
    {
        return types.Contains(_targetType);
    }
}
