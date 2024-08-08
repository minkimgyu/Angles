using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.FSM;
using System;

namespace Player
{
    public class Player : BaseLife, IFollowable, IInteracter, ISkillUser
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
            Reflect
        }

        FSM<MovementState> _movementFSM;
        FSM<ActionState> _actionFSM;

        float _moveSpeed;
        float _chargeDuration;
        float _maxChargePower;

        float _dashSpeed;
        float _dashDuration;

        float _shootSpeed;
        float _shootDuration;

        float _minJoystickLength;

        int _maxDashCount;

        int _dashConsumeCount;
        float _dashRestoreDuration;

        float _shrinkScale;
        float _normalScale;

        float _currentDashFillDuration = 0;
        int _currentDashCount;

        List<BaseSkill.Name> _skillNames;

        float DashRatio { get { return _currentDashFillDuration / _dashRestoreDuration; } }
        float TotalDashRatio { get { return _currentDashCount + DashRatio; } }
        bool CanUseDash() { return _currentDashCount >= _dashConsumeCount; }

        MoveComponent _moveComponent;
        OutlineComponent _outlineComponent;
        SkillController _skillController;

        InteractableCaptureComponent _interactableCaptureComponent;
        List<IInteractable> _interactableObjects;


        // 옵져버 델리게이트
        Action<float> OnDachRatioChangeRequested; // 대쉬 변수 변경 시 전달
        Action<float> OnChargeRatioChangeRequested; // 차지 변수 변경 시 전달

        Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested; // 스킬 획득 시 전달
        Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested; // 스킬 제거 시 전달

        Action<bool> OnShowShootDirectionRequested; // 슈팅 방향 지시선 온오프 시 전달
        Action<Vector3, Vector2> OnUpdateShootDirectionRequested; // 슈팅 방향 지시선 위치 변경 시 전달
        //

        // 생성 이벤트
        Func<BaseSkill.Name, BaseSkill> CreateSkill;
        //

        void ChangeBodyScale(bool xAxis, float ratio)
        {
            float minScale = _shrinkScale;
            float maxScale = _normalScale;

            if(xAxis) transform.localScale = 
                    Vector3.Lerp(
                        new Vector3(minScale, maxScale, transform.localScale.z), 
                        new Vector3(maxScale, maxScale, transform.localScale.z), ratio);

            else transform.localScale = 
                    Vector3.Lerp(
                        new Vector3(maxScale, minScale, transform.localScale.z),
                        new Vector3(maxScale, maxScale, transform.localScale.z), ratio);
        }

        protected override void SetInvincible(bool nowInvincible)
        {
            base.SetInvincible(nowInvincible);
            _moveComponent.FreezeRotation(nowInvincible);

            if (nowInvincible) _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnInvincible);
            else _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
        }

        public override void ResetData(PlayerData data)
        {
            _maxHp = data._maxHp;
            _targetType = data._targetType;

            _moveSpeed = data._moveSpeed;
            _chargeDuration = data._chargeDuration;
            _maxChargePower = data._maxChargePower;

            _dashSpeed = data._dashSpeed;
            _dashDuration = data._dashDuration;

            _shootSpeed = data._shootSpeed;
            _shootDuration = data._shootDuration;

            _minJoystickLength = data._minJoystickLength;

            _maxDashCount = data._maxDashCount;
            _dashConsumeCount = data._dashConsumeCount;
            _dashRestoreDuration = data._dashRestoreDuration;

            _shrinkScale = data._shrinkScale;
            _normalScale = data._normalScale;

            _skillNames = data._skillNames;
        }

        public override void AddCreateEvent(Func<BaseEffect.Name, BaseEffect> CreateEffect,
        Func<BaseSkill.Name, BaseSkill> CreateSkill)
        {
            this.CreateEffect = CreateEffect;
            this.CreateSkill = CreateSkill;
        }

        public override void AddObserverEvent(Action OnDieRequested, Action<float> OnDachRatioChangeRequested, Action<float> OnChargeRatioChangeRequested,
            Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested, Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested, Action<float, float> OnHpChangeRequested,
            Action<bool> OnShowShootDirectionRequested, Action<Vector3, Vector2> OnUpdateShootDirectionRequested)
        {
            this.OnDieRequested = OnDieRequested;
            this.OnDachRatioChangeRequested = OnDachRatioChangeRequested;
            this.OnChargeRatioChangeRequested = OnChargeRatioChangeRequested;

            _skillController.OnAddSkillRequested = OnAddSkillRequested;
            _skillController.OnRemoveSkillRequested = OnRemoveSkillRequested;

            this.OnHpChangeRequested = OnHpChangeRequested;
            OnHpChangeRequested?.Invoke(_hp, _maxHp);

            this.OnShowShootDirectionRequested = OnShowShootDirectionRequested;
            this.OnUpdateShootDirectionRequested = OnUpdateShootDirectionRequested;
        }

        public void AddSkill(BaseSkill.Name name) 
        {
            BaseSkill skill = CreateSkill?.Invoke(name);
            _skillController.AddSkill(name, skill); 
        } // 생성해서 넣어주기

        public void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { }

        public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
        {
            return _skillController.ReturnSkillUpgradeDatas();
        }

        public void MovePosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public IFollowable ReturnFollower() { return this; }


        public override void Initialize()
        {
            _interactableObjects = new List<IInteractable>();
            _groggyTimer = new Timer();
            _hp = _maxHp;

            _targetType = ITarget.Type.Blue;

            _interactableCaptureComponent = GetComponentInChildren<InteractableCaptureComponent>();
            _interactableCaptureComponent.Initialize(OnInteractableEnter, OnInteractableExit);

            _skillController = GetComponent<SkillController>();
            _skillController.Initialize();

            _outlineComponent = GetComponent<OutlineComponent>();
            _outlineComponent.Initialize();

            _moveComponent = GetComponent<MoveComponent>();
            _moveComponent.Initialize();

            InintializeFSM();
            AddEvent();
        }

        void InintializeFSM()
        {
            _movementFSM = new FSM<MovementState>();
            Dictionary<MovementState, BaseState<MovementState>> movementStates = new Dictionary<MovementState, BaseState<MovementState>>
            {
                { MovementState.Stop, new StopState(_movementFSM, _moveComponent) },
                { MovementState.Move, new MoveState(_movementFSM, _moveSpeed, CanUseDash, OnUseDash, _moveComponent)},
                { MovementState.Dash, new DashState(_movementFSM, _dashSpeed, _dashDuration, _moveComponent, ChangeBodyScale, OnEndDash) }
            };
            _movementFSM.Inintialize(movementStates, MovementState.Stop);

            _actionFSM = new FSM<ActionState>();
            Dictionary<ActionState, BaseState<ActionState>> actionStates = new Dictionary<ActionState, BaseState<ActionState>>
            {
                { ActionState.Ready, new ReadyState(_actionFSM) },
                { 
                    ActionState.Charge, new ChargeState(_actionFSM, _minJoystickLength, _chargeDuration, transform,_moveComponent, ChangeBodyScale, SetInvincible,
                    (chargeRatio) =>{ OnChargeRatioChangeRequested?.Invoke(chargeRatio); },
                    (show) => { OnShowShootDirectionRequested?.Invoke(show); },
                    (position, direction) => { OnUpdateShootDirectionRequested?.Invoke(position, direction); }) 
                },
                { 
                    ActionState.Shoot, new ShootState(_actionFSM, _shootSpeed, _shootDuration, _maxChargePower, transform, _moveComponent, ChangeBodyScale, 
                    _skillController.OnReflect, SetInvincible) 
                }
            };
            _actionFSM.Inintialize(actionStates, ActionState.Ready);
        }

        protected override void OnDie()
        {
            ClearEvent();
            OnDieRequested?.Invoke();
            Destroy(gameObject);
        }

        void OnUseDash() 
        { 
            _currentDashCount -= _dashConsumeCount;
            _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnDash);
        }

        void OnEndDash()
        {
            _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
        }

        void FillDashCount()
        {
            if (_maxDashCount == _currentDashCount) return;

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
                    OnDachRatioChangeRequested?.Invoke(TotalDashRatio);

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
            if(interactable != null &&  _interactableObjects.Contains(interactable) == true)
            {
                OnInteract(interactable);
            }

            _actionFSM.OnCollisionEnter(collision);
        }

        void ClearEvent()
        {
            IInputable inputable = ServiceLocater.ReturnInputController();
            inputable.ClearEvent(IInputable.Side.Left);
            inputable.ClearEvent(IInputable.Side.Right);
        }

        void AddEvent()
        {
            IInputable inputable = ServiceLocater.ReturnInputController();

            inputable.AddEvent(
               IInputable.Side.Left,
               IInputable.Type.OnInputStart,
               _movementFSM.OnMoveStart
           );

            inputable.AddEvent(
                IInputable.Side.Left,
                IInputable.Type.OnInput,
                (value) => { _movementFSM.OnMove(value); _actionFSM.OnMove(value); }
            );

            inputable.AddEvent(
                IInputable.Side.Left,
                IInputable.Type.OnInputEnd,
                _movementFSM.OnMoveEnd
            );

            inputable.AddEvent(
              IInputable.Side.Right,
              IInputable.Type.OnDoubleTab,
              _movementFSM.OnDash
          );


            inputable.AddEvent(
               IInputable.Side.Right,
               IInputable.Type.OnInputStart,
                _actionFSM.OnChargeStart
           );

            inputable.AddEvent(
               IInputable.Side.Right,
               IInputable.Type.OnInput,
              _actionFSM.OnCharge
            );

            inputable.AddEvent(
                IInputable.Side.Right,
                IInputable.Type.OnInputEnd,
                _actionFSM.OnChargeEnd
            );
        }

        public bool CanFollow() { return _lifeState == LifeState.Alive; }

        public Vector3 ReturnFowardDirection()
        {
            return transform.right;
        }
    }
}