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
        Player,

        YellowTriangle,
        YellowRectangle,
        YellowPentagon,
        YellowHexagon,

        RedTriangle,
        RedRectangle,
        RedPentagon,
        RedHexagon,

        Tricon,
        Rhombus,
        Pentagonic,

        Bomb
    }

    public enum AliveState
    {
        Normal, // �Ϲ�
        Immunity, // ������ �鿪
        Invincible, // ����
        Groggy, // ����
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

    protected Action<float> OnHpChangeRequested; // ü�� ��ȭ �� ����

    // ���� �̺�Ʈ
    protected BaseFactory _effectFactory;
    protected DisplayPointComponent _displayDamageComponent;

    // pathfind �̺�Ʈ �߰�
    public virtual void InitializeFSM(Func<Vector2, Vector2, BaseEnemy.Size, List<Vector2>> FindPath) { }

    public virtual void Initialize()
    {
        _displayDamageComponent = new DisplayPointComponent(_targetType, _effectFactory);
        _autoHealTimer.Start(oneTick);
    }

    void SetUp(LifeData data) { _lifeData = data; }
    public virtual void ResetData(PlayerData data) { SetUp(data); }
    public virtual void ResetData(TriconData data) { SetUp(data); }
    public virtual void ResetData(RhombusData data) { SetUp(data); }
    public virtual void ResetData(PentagonicData data) { SetUp(data); }
    public virtual void ResetData(TriangleData data) { SetUp(data); }
    public virtual void ResetData(RectangleData data) { SetUp(data); }
    public virtual void ResetData(PentagonData data) { SetUp(data); }
    public virtual void ResetData(HexagonData data) { SetUp(data); }

    public void AddObserverEvent(Action<float> OnHpChangeRequested) { this.OnHpChangeRequested = OnHpChangeRequested; }
    public virtual void AddObserverEvent(Action OnDieRequested) { }

    public virtual void AddEffectFactory(BaseFactory effectFactory) { _effectFactory = effectFactory; }

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

        _lifeData._hp += point;
        OnHpChangeRequested?.Invoke(_lifeData._hp / _lifeData.MaxHp);
        _displayDamageComponent.SpawnHealTxt(point, transform.position);

        if (_lifeData.MaxHp < _lifeData._hp)
        {
            _lifeData._hp = _lifeData.MaxHp;
        }
    }

    const float oneTick = 10f;

    protected virtual void Update()
    {
        if (_lifeState == LifeState.Die) return;

        if (_autoHealTimer.CurrentState != Timer.State.Running)
        {
            GetHeal(_lifeData._autoHpRecoveryPoint);
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
        }
    }

    public virtual void GetDamage(DamageableData damageableData)
    {
        if (_lifeState == LifeState.Alive && (_aliveState == AliveState.Immunity || _aliveState == AliveState.Invincible)) return; // �鿪 ���°ų� ���� ������ ��� ����
        if (_lifeState == LifeState.Die) return;

        bool canDamage = damageableData._targetType.Contains(_targetType);
        if (canDamage == false) return;

        float finalDamage = damageableData.CalculateDamage(_lifeData._damageReductionRatio);
        _lifeData._hp -= finalDamage;
        OnHpChangeRequested?.Invoke(_lifeData._hp / _lifeData.MaxHp);

        ICaster caster = damageableData._caster;
        if (caster != null) caster.GetDamageData(damageableData);

        if (_aliveState == AliveState.Normal && damageableData._groggyDuration > 0)
        {
            _aliveState = AliveState.Groggy; // ���� �ð� �� ����
            _groggyTimer.Start(damageableData._groggyDuration);
        }

        _displayDamageComponent.SpawnDamageTxt(finalDamage, transform.position);

        if (_lifeData._hp <= 0)
        {
            _lifeData._hp = 0;
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
