using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : BaseLife, IFollowable, IInteracter, ISkillUser, IStatUpgradable
{
    public enum MovementState
    {
        Stop,
        Move,
        Dash,
    }

    public enum ActionState
    {
        Ready,
        Charge,
        Shoot,
    }

    FSM<MovementState> _movementFSM;
    FSM<ActionState> _actionFSM;

    float _currentDashFillDuration = 0;
    int _currentDashCount;

    StatUpgrader _statUpgrader;
    PlayerData _playerData;

    float DashRatio { get { return _currentDashFillDuration / _playerData._dashRestoreDuration; } }
    float TotalDashRatio { get { return _currentDashCount + DashRatio; } }
    bool CanUseDash() { return _currentDashCount >= _playerData._dashConsumeCount; }

    MoveComponent _moveComponent;
    OutlineComponent _outlineComponent;
    SkillController _skillController;

    InteractableCaptureComponent _interactableCaptureComponent;
    List<IInteractable> _interactableObjects;

    public override void GetDamage(DamageableData damageableData)
    {
        base.GetDamage(damageableData);
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Hit);
    }

    void ChangeBodyScale(bool xAxis, float ratio)
    {
        float minScale = _playerData._shrinkScale;
        float maxScale = _playerData._normalScale;

        if(xAxis) transform.localScale = 
                Vector3.Lerp(
                    new Vector3(minScale, maxScale, transform.localScale.z), 
                    new Vector3(maxScale, maxScale, transform.localScale.z), ratio);

        else transform.localScale = 
                Vector3.Lerp(
                    new Vector3(maxScale, minScale, transform.localScale.z),
                    new Vector3(maxScale, maxScale, transform.localScale.z), ratio);
    }

    protected override void SetImmunity(bool nowInvincible)
    {
        base.SetImmunity(nowInvincible);
        _moveComponent.FreezeRotation(nowInvincible);

        if (nowInvincible) _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnInvincible);
        else _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
    }

    public override void ResetData(PlayerData data)
    {
        //BuffValueCommand speedModifyCommand = new BuffValueCommand();
        // 값이 바뀌는 변수들은 BuffFloat나 BuffInt를 사용해서 최소, 최대 값을 지정해주고
        // 참조 타입으로 설정했으므로 버프로 인해 수정되면 알아서 값이 반영되게 된다.
        _maxHp = data._maxHp;
        _targetType = data._targetType;
        _playerData = data;
    }

    public void AddSkill(BaseSkill.Name name, BaseSkill skill) 
    {
        _skillController.AddSkill(name, skill);
    }

    public void MovePosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public IFollowable ReturnFollower() { return this; }


    public override void Initialize()
    {
        base.Initialize();
        _interactableObjects = new List<IInteractable>();
        _groggyTimer = new Timer();
        _hp = _maxHp;

        _statUpgrader = new StatUpgrader();
        _displayDamageComponent = new DisplayPointComponent(_targetType, _effectFactory);
        _interactableCaptureComponent = GetComponentInChildren<InteractableCaptureComponent>();
        _interactableCaptureComponent.Initialize(OnInteractableEnter, OnInteractableExit);

        _skillController = GetComponent<SkillController>();
        _skillController.Initialize(_playerData);

        _outlineComponent = GetComponent<OutlineComponent>();
        _outlineComponent.Initialize();

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        InintializeFSM();
    }

    void InintializeFSM()
    {
        _movementFSM = new FSM<MovementState>();
        Dictionary<MovementState, BaseState<MovementState>> movementStates = new Dictionary<MovementState, BaseState<MovementState>>
        {
            { MovementState.Stop, new StopState(_movementFSM, _moveComponent) },
            { MovementState.Move, new MoveState(_movementFSM, _playerData, CanUseDash, OnUseDash, _moveComponent)},
            { MovementState.Dash, new DashState(_movementFSM, _playerData, _moveComponent, ChangeBodyScale, OnEndDash) }
        };
        _movementFSM.Initialize(movementStates, MovementState.Stop);

        _actionFSM = new FSM<ActionState>();
        Dictionary<ActionState, BaseState<ActionState>> actionStates = new Dictionary<ActionState, BaseState<ActionState>>
        {
            { ActionState.Ready, new ReadyState(_actionFSM) },
            { ActionState.Charge, new ChargeState(_actionFSM, _playerData, transform,_moveComponent, ChangeBodyScale, SetImmunity) },
            { 
                ActionState.Shoot, new ShootState(_actionFSM, _playerData, transform, _moveComponent, ChangeBodyScale, 
                _skillController.OnReflect, SetImmunity) 
            }
        };
        _actionFSM.Initialize(actionStates, ActionState.Ready);
    }

    protected override void OnDie()
    {
        EventBusManager.Instance.MainEventBus.Publish(MainEventBus.State.GameOver);
        Destroy(gameObject);
    }

    void OnUseDash() 
    { 
        _currentDashCount -= _playerData._dashConsumeCount;
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnDash);
    }

    void OnEndDash()
    {
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
    }

    void FillDashCount()
    {
        if (_playerData._maxDashCount == _currentDashCount) return;

        _currentDashFillDuration += Time.deltaTime;
        if (DashRatio >= 1)
        {
            _currentDashCount++;
            _currentDashFillDuration = 0;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        switch (_lifeState)
        {
            case LifeState.Alive:
                _movementFSM.OnFixedUpdate();
                break;
            default:
                break;
        }
    }


    protected override void Update()
    {
        base.Update();
        switch (_lifeState)
        {
            case LifeState.Alive:
                FillDashCount();
                EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.State.OnDashRatioChange, TotalDashRatio);
                _skillController.OnUpdate();

                _movementFSM.OnUpdate();
                _actionFSM.OnUpdate();
                break;
            default:
                break;
        }
    }

    void OnInteractableEnter(IInteractable interactable)
    {
        interactable.OnInteractEnter(this);
        _interactableObjects.Add(interactable);
    }

    void OnInteract(IInteractable interactable)
    {
        interactable.OnInteract(this);
    }

    void OnInteractableExit(IInteractable interactable)
    {
        interactable.OnInteractExit(this);
        _interactableObjects.Remove(interactable);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null && _interactableObjects.Contains(interactable) == true)
        {
            OnInteract(interactable);
        }
        else
        {
            _actionFSM.OnCollisionEnter(collision); // 위와 같은 경우가 아닌 경우 OnCollisionEnter2D 이벤트 진행
        }
    }

    public void OnLeftInputStart() => _movementFSM.OnMoveStart();
    public void OnLeftInput(Vector2 direction)
    {
        _movementFSM.OnMove(direction);
        _actionFSM.OnMove(direction);
    }
    public void OnLeftInputEnd() => _movementFSM.OnMoveEnd();

    public void OnRightInputStart() => _actionFSM.OnChargeStart();
    public void OnRightInput(Vector2 direction) => _actionFSM.OnCharge(direction);
    public void OnRightInputEnd() => _actionFSM.OnChargeEnd();
    public void OnRightDoubleTab() => _movementFSM.OnDash();

    public bool CanFollow() { return _lifeState == LifeState.Alive; }

    public Vector3 ReturnFowardDirection()
    {
        return transform.right;
    }

    public ISkillUser ReturnSkillUser()
    {
        return this;
    }

    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        return _skillController.ReturnSkillUpgradeDatas();
    }


    public void Upgrade(StatUpgrader.CooltimeData cooltimeData)
    {
        _statUpgrader.Visit(_playerData, cooltimeData);
    }

    public void Upgrade(StatUpgrader.DamageData damageData)
    {
        _statUpgrader.Visit(_playerData, damageData);
    }

    public void Upgrade(StatUpgrader.DashData dashData)
    {
        _statUpgrader.Visit(_playerData, dashData);
    }

    public void Upgrade(StatUpgrader.ShootingData shootingData)
    {
        _statUpgrader.Visit(_playerData, shootingData);
    }
}