using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseLife : MonoBehaviour, IDamageable, ITarget
{
    protected UpgradeableStat<float> Hp
    {
        get { return _hp; }
    }

    UpgradeableStat<float> _maxHp;
    UpgradeableStat<float> _hp;

    UpgradeableStat<float> _autoHpRecoveryPoint; // 일정 시간마다 체력 회복
    UpgradeableStat<float> _damageReductionRatio; // 데미지 감소 수치

    ITarget.Type _targetType;
    BaseEffect.Name _destroyEffectName;

    //LifeData _lifeData;
    protected Timer _groggyTimer = new Timer();
    protected Timer _autoHealTimer = new Timer();

    public enum Size
    {
        Small, // 1 x 1
        Medium, // 2 x 2, 3 X 3
    }

    //protected Size _size;

    // 생성 순으로 정렬 필요
    public enum Name
    {
        Player, // 0

        YellowTriangle, // 1
        YellowRectangle,
        YellowPentagon,
        YellowHexagon,

        RedTriangle,
        RedRectangle,
        RedPentagon,
        RedHexagon,



        OperaTriangle,
        OperaRectangle,
        OperaPentagon,
        OperaHexagon,

        Tricon,
        Rhombus,
        Pentagonic,
        Hexahorn,
        Octavia,

        Bomb,

        GreenTriangle,
        GreenRectangle,
        GreenPentagon,
        GreenHexagon,

        Hexatric
    }

    public enum AliveState
    {
        Normal, // 일반
        Immunity, // 데미지 면역
        Invincible, // 무적 상태 --> 더 상위 개념
        Groggy, // 기절
    }

    public enum LifeState
    {
        Alive,
        Die,
    }

    //protected ITarget.Type _targetType;

    protected LifeState _lifeState = LifeState.Alive;
    protected AliveState _aliveState = AliveState.Normal;

    protected Action<float> OnHpChangeRequested; // 체력 변화 시 전달

    // 생성 이벤트
    protected BaseFactory _effectFactory;
    protected DisplayPointComponent _displayDamageComponent;

    // pathfind 이벤트 추가
    //public virtual void InitializeFSM(Func<Vector2, Vector2, BaseEnemy.Size, List<Vector2>> FindPath) { }

    public virtual void Initialize()
    {
        _displayDamageComponent = new DisplayPointComponent(_targetType, _effectFactory);
        _autoHealTimer.Start(oneTick);
    }

    protected void SetUp(LifeData data) 
    {
        _maxHp = data.MaxHp;
        _hp = data.MaxHp.Copy();
        data.OnMaxHpChanged += (value) => { _maxHp.Value += value; _hp.Value += value; };

        _autoHpRecoveryPoint = data.AutoHpRecoveryPoint;
        _damageReductionRatio = data.DamageReductionRatio;
        _destroyEffectName = data.DestroyEffectName;
        _targetType = data.TargetType;
    }

    protected virtual void SetUp(
        LifeData data,
        DropData dropData) 
    {
        SetUp(data);
    }

    public virtual void InjectData(PlayerData data) { SetUp(data); }
    public virtual void InjectData(TriconData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(RhombusData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(PentagonicData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(HexahornData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(OctaviaData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(HexatricData data, DropData dropData) { SetUp(data, dropData); }

    public virtual void InjectData(TriangleData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(RectangleData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(PentagonData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(HexagonData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(OperaHexagonData data, DropData dropData) { SetUp(data, dropData); }
    public virtual void InjectData(GreenPentagonData data, DropData dropData) { SetUp(data, dropData); }

    public void InjectEvent(Action<float> OnHpChangeRequested) { this.OnHpChangeRequested = OnHpChangeRequested; }
    public virtual void InjectEvent(Action OnDieRequested) { }

    public virtual void InjectEffectFactory(BaseFactory effectFactory) { _effectFactory = effectFactory; }

    protected virtual void SetImmunity(bool nowImmunity)
    {
        if (_aliveState == AliveState.Invincible) return; // 무적 상태면 바꾸지 않음

        if (nowImmunity) _aliveState = AliveState.Immunity;
        else _aliveState = AliveState.Normal;
    }

    public virtual void Revive() 
    {
        _lifeState = LifeState.Alive;
        GetHeal(_maxHp.Value);
    }

    public virtual void SetInvincible()
    {
        _aliveState = AliveState.Invincible;
    }

    protected virtual void OnDie()
    {
        BaseEffect effect = _effectFactory.Create(_destroyEffectName);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public virtual void GetHeal(float point)
    {
        if(point == 0) return;

        _hp.Value += point;
        OnHpChangeRequested?.Invoke(_hp.Value / _maxHp.Value);
        _displayDamageComponent.SpawnHealTxt(point, transform.position);

        if (_maxHp.Value < _hp.Value)
        {
            _hp.Value = _maxHp.Value;
        }
    }

    const float oneTick = 10f;

    protected virtual void Update()
    {
        if (_lifeState == LifeState.Die) return;

        if (_autoHealTimer.CurrentState != Timer.State.Running)
        {
            GetHeal(_autoHpRecoveryPoint.Value);
            _autoHealTimer.Reset();
            _autoHealTimer.Start(oneTick);
        }

        switch (_aliveState)
        {
            case AliveState.Groggy:
                if (_groggyTimer.CurrentState == Timer.State.Finish)
                {
                    _aliveState = AliveState.Normal;
                    _groggyTimer.Reset();
                }
                break;
            case AliveState.Normal:
                UpdateOnIdle();
                break;
        }
    }

    protected virtual void UpdateOnIdle() { }

    public virtual void GetDamage(DamageableData damageableData)
    {
        if (_lifeState == LifeState.Alive && _aliveState == AliveState.Immunity) return; // 살아있고 면역 상태의 경우 리턴
        if (_lifeState == LifeState.Alive && _aliveState == AliveState.Invincible) return; // 살아있고 무적 상태의 경우 리턴
        if (_lifeState == LifeState.Die) return; // 죽었거나 무적이면 리턴

        //bool canDamage = damageableData._targetType.Contains(_lifeData.TargetType);
        //if (canDamage == false) return;

        float finalDamage = damageableData.CalculateDamage(_damageReductionRatio.Value);
        _hp.Value -= finalDamage;
        OnHpChangeRequested?.Invoke(_hp.Value / _maxHp.Value);

        ICaster caster = damageableData._caster;
        if (caster != null) caster.GetDamageData(damageableData);

        if (_aliveState == AliveState.Normal && damageableData._groggyDuration > 0)
        {
            _aliveState = AliveState.Groggy; // 일정 시간 후 복귀
            _groggyTimer.Start(damageableData._groggyDuration);
        }

        _displayDamageComponent.SpawnDamageTxt(finalDamage, transform.position);

        if (_hp.Value <= 0)
        {
            _hp.Value = 0;
            _lifeState = LifeState.Die;
            OnDie();
        }
    }

    public Vector3 GetPosition() 
    {
        return transform.position;
    }

    public bool IsTarget(List<ITarget.Type> types)
    {
        return types.Contains(_targetType);
    }
}
