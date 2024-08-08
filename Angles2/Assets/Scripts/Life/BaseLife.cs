using System;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseLife : MonoBehaviour, IDamageable, ITarget
{
    protected float _maxHp;
    protected float _hp;

    protected Timer _groggyTimer;
    //protected Action<float> OnHpChange;

    //protected Action OnDieRequested;

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

    protected BaseEffect.Name _destoryEffect;

    // 옵져버 델리게이트
    protected Action<float, float> OnHpChangeRequested; // 체력 변화 시 전달
    protected Action OnDieRequested; // 사망 시 호출
    //

    // 생성 이벤트
    protected Func<BaseEffect.Name, BaseEffect> CreateEffect;
    //

    public abstract void Initialize();
    public virtual void ResetData(PlayerData data) { }
    public virtual void ResetData(TriangleData data) { }
    public virtual void ResetData(RectangleData data) { }
    public virtual void ResetData(PentagonData data) { }
    public virtual void ResetData(HexagonData data) { }

    public virtual void SetTarget(IPos target) { }

    //public virtual void AddSkill(BaseSkill.Name skillName) { }
    //public virtual void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { }

    // Enemy 전용
    public virtual void AddObserverEvent(Action OnDieRequested, Action<DropData, Vector3> OnDropRequested) { }

    // Player 전용
    public virtual void AddObserverEvent(Action OnDieRequested, Action<float> OnDachRatioChangeRequested, Action<float> OnChargeRatioChangeRequested,
        Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested, Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested, Action<float, float> OnHpChangeRequested,
        Action<bool> OnShowShootDirectionRequested, Action<Vector3, Vector2> OnUpdateShootDirectionRequested) { }

    public virtual void AddCreateEvent(Func<BaseEffect.Name, BaseEffect> CreateEffect) { }

    public virtual void AddCreateEvent(Func<BaseEffect.Name, BaseEffect> CreateEffect,
        Func<BaseSkill.Name, BaseSkill> CreateSkill)
    { }

    protected virtual void SetInvincible(bool nowInvincible)
    {
        if (nowInvincible) _aliveState = AliveState.Invincible;
        else _aliveState = AliveState.Normal;
    }

    protected virtual void OnDie()
    {
        OnDieRequested?.Invoke();

        BaseEffect effect = CreateEffect?.Invoke(_destoryEffect);
        effect.ResetPosition(transform.position);
        effect.Play();

        Destroy(gameObject);
    }

    public virtual void GetHeal(float point)
    {
        _hp += point;
        OnHpChangeRequested?.Invoke(_hp, _maxHp);

        if (_maxHp < _hp)
        {
            _hp = _maxHp;
        }
    }

    void SpawnDamageTxt(DamageData damageData)
    {
        if (damageData.ShowTxt == false) return;

        BaseEffect effect = CreateEffect?.Invoke(BaseEffect.Name.DamageTextEffect);

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
        if (_lifeState == LifeState.Die) return;

        bool canDamage = damageData.DamageableTypes.Contains(_targetType);
        if (canDamage == false) return;

        _hp -= damageData.Damage;
        OnHpChangeRequested?.Invoke(_hp, _maxHp);

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

    public Vector3 ReturnPosition() 
    { 
        if (transform == null) return Vector3.zero;
        else return transform.position;
    }

    public bool IsTarget(List<ITarget.Type> types)
    {
        return types.Contains(_targetType);
    }
}
