using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Player : BaseLife, IFollowable, IInteracter, ICaster, IStatUpgradable, IForce
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

    PlayerData _playerData;

    [SerializeField] Transform _bottomPoint;
    public Vector2 BottomPoint => _bottomPoint.localPosition;

    private void OnGUI()
    {
#if UNITY_EDITOR
        int size = 25;
        Color color = Color.red;

        int startY = 120;
        int gap = 30;

        List<Tuple<string, float>> datas = new List<Tuple<string, float>>
        {
            { new Tuple<string, float>("MaxHp", _playerData.MaxHp) },
            { new Tuple<string, float>("AutoHpRecoveryPoint", _playerData.AutoHpRecoveryPoint) },
            { new Tuple<string, float>("DamageReductionRatio", _playerData.DamageReductionRatio) },

            { new Tuple<string, float>("AliveState", (int)_aliveState) },

            { new Tuple<string, float>("AttackDamage", _playerData.AttackDamage) },
            { new Tuple<string, float>("TotalDamageRatio", _playerData.TotalDamageRatio) },
            { new Tuple<string, float>("TotalCooltimeRatio", _playerData.TotalCooltimeRatio) },
            { new Tuple<string, float>("TotalRandomRatio", _playerData.TotalRandomRatio) },

            { new Tuple<string, float>("MoveSpeed", _playerData.MoveSpeed) },
            { new Tuple<string, float>("DrainRatio", _playerData.DrainRatio) },
            { new Tuple<string, float>("DrainPercentage", _playerData.DrainPercentage) },

            { new Tuple<string, float>("ChargeDuration", _playerData.ChargeDuration) },
            { new Tuple<string, float>("ShootDuration", _playerData.ShootDuration) },
            { new Tuple<string, float>("ShootSpeed", _playerData.ShootSpeed) },
        };

        for (int i = 0; i < datas.Count; i++)
        {
            GUIStyle style = new GUIStyle();
            Rect rect = new Rect(30, startY + gap * i, Screen.width, Screen.height);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = size;
            style.normal.textColor = color;

            string text = $"{datas[i].Item1} : {datas[i].Item2}";
            GUI.Label(rect, text, style);
        }
#elif UNITY_ANDROID
#endif
    }


    float DashRatio { get { return _currentDashFillDuration / _playerData.DashRestoreDuration; } }
    float TotalDashRatio { get { return _currentDashCount + DashRatio; } }

    SpriteRenderer _spriteRenderer;

    public void ApplySkinSprite(Sprite skin) => _spriteRenderer.sprite = skin;

    bool CanUseDash() { return _currentDashCount >= _playerData.DashConsumeCount; }

    MoveComponent _moveComponent;
    OutlineComponent _outlineComponent;
    SkillController _skillController;

    [SerializeField] InteractableCaptureComponent _closeInteractableCaptureComponent;
    [SerializeField] InteractableCaptureComponent _farInteractableCaptureComponent;

    List<IInteractable> _interactableObjects;

    public override void GetDamage(DamageableData damageableData)
    {
        base.GetDamage(damageableData);
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Hit);
    }

    void ChangeBodyScale(bool xAxis, float ratio)
    {
        float minScale = _playerData.ShrinkScale;
        float maxScale = _playerData.NormalScale;

        if(xAxis) transform.localScale = 
                Vector3.Lerp(
                    new Vector3(minScale, maxScale, transform.localScale.z), 
                    new Vector3(maxScale, maxScale, transform.localScale.z), ratio);

        else transform.localScale = 
                Vector3.Lerp(
                    new Vector3(maxScale, minScale, transform.localScale.z),
                    new Vector3(maxScale, maxScale, transform.localScale.z), ratio);
    }

    protected override void SetImmunity(bool nowImmunity)
    {
        base.SetImmunity(nowImmunity);
        _moveComponent.FreezeRotation(nowImmunity);

        if (nowImmunity) _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnImmunity);
        else _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
    }

    public override void ResetData(PlayerData data)
    {
        //BuffValueCommand speedModifyCommand = new BuffValueCommand();
        // 값이 바뀌는 변수들은 BuffFloat나 BuffInt를 사용해서 최소, 최대 값을 지정해주고
        // 참조 타입으로 설정했으므로 버프로 인해 수정되면 알아서 값이 반영되게 된다.
        base.ResetData(data);
        _targetType = data.TargetType;
        _playerData = data;
    }

    Action OnGetSkillTutorialEvent;

    public void InjectTutorialEvent(
        Action MoveStartTutorialEvent,
        Action ShootingTutorialEvent,
        Action CollisionTutorialEvent,
        Action CancelShootingTutorialEvent,
        Action OnGetSkillTutorialEvent)
    {
        ITutorialInjector moveStateInjector = _movementFSM.GetState(MovementState.Move);
        moveStateInjector.InjectTutorialEvent(MoveStartTutorialEvent);

        ITutorialInjector shootingInjector = _actionFSM.GetState(ActionState.Shoot);
        shootingInjector.InjectTutorialEvent(ShootingTutorialEvent, CollisionTutorialEvent, CancelShootingTutorialEvent);

        this.OnGetSkillTutorialEvent = OnGetSkillTutorialEvent;
    }

    public void AddSkill(BaseSkill.Name name, BaseSkill skill) 
    {
        OnGetSkillTutorialEvent?.Invoke(); // 튜토리얼 이벤트 실행
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
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _displayDamageComponent = new DisplayPointComponent(_targetType, _effectFactory);

        _farInteractableCaptureComponent.Initialize(OnInteractableEnter, OnInteractableExit); // 원거리 인터랙션
        _closeInteractableCaptureComponent.Initialize(OnInteract); // 근접 인터렉션

        _skillController = GetComponent<SkillController>();
        _skillController.Initialize(_playerData, this);

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

    public override void Revive() 
    {
        base.Revive();
        _skillController.OnRevive();
        gameObject.SetActive(true);
    }

    protected override void OnDie()
    {
        gameObject.SetActive(false);
        EventBusManager.Instance.MainEventBus.Publish(MainEventBus.State.GameOver);
    }

    void OnUseDash() 
    { 
        _currentDashCount -= _playerData.DashConsumeCount;
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnDash);
    }

    void OnEndDash()
    {
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
    }

    void FillDashCount()
    {
        if (_playerData.MaxDashCount == _currentDashCount) return;

        _currentDashFillDuration += Time.deltaTime;
        if (DashRatio >= 1)
        {
            _currentDashCount++;
            _currentDashFillDuration = 0;
        }
    }

    void FixedUpdate()
    {
        if (_aliveState == AliveState.Groggy) return;

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
        if (_aliveState == AliveState.Groggy) return;

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
        // 다른 방식으로 들어감
        // 아예 트리거 방식으로 상호작용하는 기능 설계
        //IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
        //if (interactable != null && _interactableObjects.Contains(interactable) == true)
        //{
        //    OnInteract(interactable);
        //}
        //else
        //{
            _actionFSM.OnCollisionEnter(collision); // 위와 같은 경우가 아닌 경우 OnCollisionEnter2D 이벤트 진행
        //}
    }

    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    // 다른 방식으로 들어감
    //    // 아예 트리거 방식으로 상호작용하는 기능 설계
    //    IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();
    //    if (interactable != null && _interactableObjects.Contains(interactable) == true)
    //    {
    //        OnInteract(interactable);
    //    }
    //}

    public void OnLeftInputStart()
    {
        if (_lifeState == LifeState.Die) return;

        _movementFSM.OnMoveStart();
    }
    public void OnLeftInput(Vector2 direction)
    {
        if (_lifeState == LifeState.Die) return;

        _movementFSM.OnMove(direction);
        _actionFSM.OnMove(direction);
    }
    public void OnLeftInputEnd()
    {
        if (_lifeState == LifeState.Die) return;

        _movementFSM.OnMoveEnd();
    }

    public void OnRightInputStart()
    {
        if (_lifeState == LifeState.Die) return;

        _actionFSM.OnChargeStart();
    }

    public void OnRightInput(Vector2 direction)
    {
        if (_lifeState == LifeState.Die) return;

        _actionFSM.OnCharge(direction);
    }
    public void OnRightInputEnd()
    {
        if (_lifeState == LifeState.Die) return;

        _actionFSM.OnChargeEnd();
    }

    public void OnRightDoubleTab()
    {
        if (_lifeState == LifeState.Die) return;

        _movementFSM.OnDash();
    }

    public bool CanFollow() { return gameObject as UnityEngine.Object != null; }

    public Vector3 ReturnFowardDirection()
    {
        return transform.right;
    }

    public ICaster GetCaster() { return this; }

    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        return _skillController.ReturnSkillUpgradeDatas();
    }

    public void Upgrade(IStatModifier stat, int level)
    {
        stat.Visit(_playerData, level);
    }

    public void Upgrade(IStatModifier stat)
    {
        stat.Visit(_playerData);
    }

    public void GetDamageData(DamageableData damageData)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random > _playerData.DrainPercentage) return; // 10% 확률로 가능

        GetHeal(damageData._damageStat.TotalDamage * _playerData.DrainRatio);
    }

    public bool CanApplyForce()
    {
        return _lifeState == LifeState.Alive && (_aliveState == AliveState.Normal || _aliveState == AliveState.Groggy);
    }

    public void ApplyForce(Vector3 direction, float force, ForceMode2D mode)
    {
        _moveComponent.AddForce(direction, force, mode);
    }
}