using System;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseLife : MonoBehaviour, IDamageable, ITarget
{
    //protected abstract float Hp { get; set; }
    //protected abstract float MaxHp { get; set; }
    //protected abstract float DamageReductionRatio { get; }
    //protected abstract float AutoRecoveryPoint { get; }

    LifeData _lifeData;
    protected Timer _groggyTimer = new Timer();
    protected Timer _autoHealTimer = new Timer();

    public enum Size
    {
        Small, // 1 x 1
        Medium, // 2 x 2, 3 X 3
    }

    protected Size _size;

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

        Bomb
    }

    public enum AliveState
    {
        Normal, // 일반
        Immunity, // 데미지 면역
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

    protected Action<float> OnHpChangeRequested; // 체력 변화 시 전달

    // 생성 이벤트
    protected BaseFactory _effectFactory;
    protected DisplayPointComponent _displayDamageComponent;

    // pathfind 이벤트 추가
    public virtual void InitializeFSM(Func<Vector2, Vector2, BaseEnemy.Size, List<Vector2>> FindPath) { }

    public virtual void Initialize()
    {
        _displayDamageComponent = new DisplayPointComponent(_targetType, _effectFactory);
        _autoHealTimer.Start(oneTick);
    }

    void SetUp(LifeData data) { _lifeData = data; }
    public virtual void ResetData(PlayerData data) { SetUp(data); }
    public virtual void ResetData(TriconData data, DropData dropData) { SetUp(data); }
    public virtual void ResetData(RhombusData data, DropData dropData) { SetUp(data); }
    public virtual void ResetData(PentagonicData data, DropData dropData) { SetUp(data); }
    public virtual void ResetData(HexahornData data, DropData dropData) { SetUp(data); }
    public virtual void ResetData(OctaviaData data, DropData dropData) { SetUp(data); }


    public virtual void ResetData(TriangleData data, DropData dropData) { SetUp(data); }
    public virtual void ResetData(RectangleData data, DropData dropData) { SetUp(data); }
    public virtual void ResetData(PentagonData data, DropData dropData) { SetUp(data); }
    public virtual void ResetData(HexagonData data, DropData dropData) { SetUp(data); }
    public virtual void ResetData(OperaHexagonData data, DropData dropData) { SetUp(data); }

    public void AddObserverEvent(Action<float> OnHpChangeRequested) { this.OnHpChangeRequested = OnHpChangeRequested; }
    public virtual void AddObserverEvent(Action OnDieRequested) { }

    public virtual void AddEffectFactory(BaseFactory effectFactory) { _effectFactory = effectFactory; }

    public virtual void AddTarget(ITarget target) { }

    protected virtual void SetImmunity(bool nowImmunity)
    {
        if (nowImmunity) _aliveState = AliveState.Immunity;
        else _aliveState = AliveState.Normal;
    }

    public virtual void SetInvincible()
    {
        _aliveState = AliveState.Invincible;
    }

    protected virtual void OnDie()
    {
        BaseEffect effect = _effectFactory.Create(_destoryEffect);
        effect.ResetPosition(transform.position);
        effect.Play();

        Destroy(gameObject);
    }

    public virtual void GetHeal(float point)
    {
        if(point == 0) return;

        _lifeData.Hp += point;
        OnHpChangeRequested?.Invoke(_lifeData.Hp / _lifeData.MaxHp);
        _displayDamageComponent.SpawnHealTxt(point, transform.position);

        if (_lifeData.MaxHp < _lifeData.Hp)
        {
            _lifeData.Hp = _lifeData.MaxHp;
        }
    }

    const float oneTick = 10f;

    protected virtual void Update()
    {
        if (_lifeState == LifeState.Die) return;

        if (_autoHealTimer.CurrentState != Timer.State.Running)
        {
            GetHeal(_lifeData.AutoHpRecoveryPoint);
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
        if (_lifeState == LifeState.Alive && (_aliveState == AliveState.Immunity || _aliveState == AliveState.Invincible)) return; // 면역 상태거나 무적 상태의 경우 리턴
        if (_lifeState == LifeState.Die) return;

        bool canDamage = damageableData._targetType.Contains(_targetType);
        if (canDamage == false) return;

        float finalDamage = damageableData.CalculateDamage(_lifeData.DamageReductionRatio);
        _lifeData.Hp -= finalDamage;
        OnHpChangeRequested?.Invoke(_lifeData.Hp / _lifeData.MaxHp);

        ICaster caster = damageableData._caster;
        if (caster != null) caster.GetDamageData(damageableData);

        if (_aliveState == AliveState.Normal && damageableData._groggyDuration > 0)
        {
            _aliveState = AliveState.Groggy; // 일정 시간 후 복귀
            _groggyTimer.Start(damageableData._groggyDuration);
        }

        _displayDamageComponent.SpawnDamageTxt(finalDamage, transform.position);

        if (_lifeData.Hp <= 0)
        {
            _lifeData.Hp = 0;
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
