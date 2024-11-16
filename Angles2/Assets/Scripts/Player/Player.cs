using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static InputController;

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

    private void OnGUI()
    {
#if UNITY_ANDROID && UNITY_EDITOR
        int size = 25;
        Color color = Color.red;

        int startY = 120;
        int gap = 30;

        List<Tuple<string, float>> datas = new List<Tuple<string, float>>
        {
            { new Tuple<string, float>("MaxHp", _playerData.MaxHp) },
            { new Tuple<string, float>("AutoHpRecoveryPoint", _playerData._autoHpRecoveryPoint) },

            { new Tuple<string, float>("DamageReductionRatio", _playerData._damageReductionRatio) },

            { new Tuple<string, float>("AttackDamage", _playerData.AttackDamage) },
            { new Tuple<string, float>("TotalDamageRatio", _playerData.TotalDamageRatio) },
            { new Tuple<string, float>("TotalCooltimeRatio", _playerData.TotalCooltimeRatio) },
            { new Tuple<string, float>("TotalRandomRatio", _playerData.TotalRandomRatio) },

            { new Tuple<string, float>("MoveSpeed", _playerData._moveSpeed) },
            { new Tuple<string, float>("DrainRatio", _playerData._drainRatio) },

            { new Tuple<string, float>("ChargeDuration", _playerData._chargeDuration) },
            { new Tuple<string, float>("ShootDuration", _playerData._shootDuration) },
            { new Tuple<string, float>("ShootSpeed", _playerData._shootSpeed) },
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


    float DashRatio { get { return _currentDashFillDuration / _playerData._dashRestoreDuration; } }
    float TotalDashRatio { get { return _currentDashCount + DashRatio; } }

    //protected override float MaxHp 
    //{   
    //    get => _playerData.MaxHp;
    //    set
    //    {
    //        _playerData.MaxHp = value;
    //        OnHpChangeRequested?.Invoke(Hp / MaxHp); // hp ���� ������
    //    }
    //}
    //protected override float Hp { get => _playerData._hp; set => _playerData._hp = value; }
    //protected override float DamageReductionRatio { get => _playerData._damageReductionRatio; }
    //protected override float AutoRecoveryPoint { get => _playerData._autoHpRecoveryPoint; }

    SpriteRenderer _spriteRenderer;

    public void ApplySkinSprite(Sprite skin) => _spriteRenderer.sprite = skin;

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
        // ���� �ٲ�� �������� BuffFloat�� BuffInt�� ����ؼ� �ּ�, �ִ� ���� �������ְ�
        // ���� Ÿ������ ���������Ƿ� ������ ���� �����Ǹ� �˾Ƽ� ���� �ݿ��ǰ� �ȴ�.
        base.ResetData(data);
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
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _displayDamageComponent = new DisplayPointComponent(_targetType, _effectFactory);
        _interactableCaptureComponent = GetComponentInChildren<InteractableCaptureComponent>();
        _interactableCaptureComponent.Initialize(OnInteractableEnter, OnInteractableExit);

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
        IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
        if (interactable != null && _interactableObjects.Contains(interactable) == true)
        {
            OnInteract(interactable);
        }
        else
        {
            _actionFSM.OnCollisionEnter(collision); // ���� ���� ��찡 �ƴ� ��� OnCollisionEnter2D �̺�Ʈ ����
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
        GetHeal(damageData._damageStat.TotalDamage * _playerData._drainRatio);
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