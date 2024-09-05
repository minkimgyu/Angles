using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.FSM;
using System;

namespace Player
{
    public class Player : BaseLife, IFollowable, IInteracter, ISkillUser, IBuffUsable
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

        BuffFloat _totalDamageRatio;
        BuffFloat _totalCooltimeRatio;

        BuffFloat _dashSpeed;
        BuffFloat _dashDuration;

        BuffFloat _shootingDuration;
        BuffFloat _chargeDuration;

        float _moveSpeed;
        float _shootSpeed;

        float _minJoystickLength;

        int _maxDashCount;

        int _dashConsumeCount;
        BuffFloat _dashRestoreDuration;

        float _shrinkScale;
        float _normalScale;

        float _currentDashFillDuration = 0;
        int _currentDashCount;

        List<BaseSkill.Name> _skillNames;

        float DashRatio { get { return _currentDashFillDuration / _dashRestoreDuration.Value; } }
        float TotalDashRatio { get { return _currentDashCount + DashRatio; } }
        bool CanUseDash() { return _currentDashCount >= _dashConsumeCount; }

        MoveComponent _moveComponent;
        OutlineComponent _outlineComponent;
        SkillController _skillController;
        BuffController _buffController;

        InteractableCaptureComponent _interactableCaptureComponent;
        List<IInteractable> _interactableObjects;

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
            //BuffValueCommand speedModifyCommand = new BuffValueCommand();
            // 값이 바뀌는 변수들은 BuffFloat나 BuffInt를 사용해서 최소, 최대 값을 지정해주고
            // 참조 타입으로 설정했으므로 버프로 인해 수정되면 알아서 값이 반영되게 된다.
            _maxHp = data._maxHp;
            _targetType = data._targetType;

            _moveSpeed = data._moveSpeed;

            _totalDamageRatio = new BuffFloat(data._minTotalDamageRatio, data._maxTotalDamageRatio, data._totalDamageRatio);
            _totalCooltimeRatio = new BuffFloat(data._minTotalCooltimeRatio, data._maxTotalCooltimeRatio, data._totalCooltimeRatio);

            _chargeDuration = new BuffFloat(data._minChargeDuration, data._maxChargeDuration, data._chargeDuration);
            _dashSpeed = new BuffFloat(data._minDashSpeed, data._maxDashSpeed, data._dashSpeed);
            _dashDuration = new BuffFloat(data._minDashDuration, data._maxDashDuration, data._dashDuration);
            _shootingDuration = new BuffFloat(data._minShootDuration, data._maxShootDuration, data._shootDuration);

            _shootSpeed = data._shootSpeed;

            _minJoystickLength = data._minJoystickLength;

            _maxDashCount = data._maxDashCount;
            _dashConsumeCount = data._dashConsumeCount;

            _dashRestoreDuration = new BuffFloat(data._minDashRestoreDuration, data._maxDashRestoreDuration, data._dashRestoreDuration);

            _shrinkScale = data._shrinkScale;
            _normalScale = data._normalScale;

            _skillNames = data._skillNames;
        }

        public void AddSkill(BaseSkill.Name name, BaseSkill skill) 
        {
            _skillController.AddSkill(name, skill);
        }

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
            _skillController.Initialize(_totalDamageRatio, _totalCooltimeRatio);

            _outlineComponent = GetComponent<OutlineComponent>();
            _outlineComponent.Initialize();

            _moveComponent = GetComponent<MoveComponent>();
            _moveComponent.Initialize();

            _buffController = GetComponent<BuffController>();
            _buffController.Initialize( 
                new Dictionary<BaseBuff.Type, BuffCommand> 
                {
                    { BaseBuff.Type.TotalDamage, new BuffRatioCommand(_totalDamageRatio) },
                    { BaseBuff.Type.TotalCooltime, new BuffRatioCommand(_totalCooltimeRatio) },

                    { BaseBuff.Type.ShootingDuration, new BuffValueCommand(_shootingDuration) },
                    { BaseBuff.Type.ShootingChargeDuration, new BuffValueCommand(_chargeDuration) },

                    { BaseBuff.Type.DashSpeed, new BuffValueCommand(_dashSpeed) },
                    { BaseBuff.Type.DashChargeDuration, new BuffValueCommand(_dashDuration) },
                }
            );


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
                { ActionState.Charge, new ChargeState(_actionFSM, _minJoystickLength, _chargeDuration, transform,_moveComponent, ChangeBodyScale, SetInvincible) },
                { 
                    ActionState.Shoot, new ShootState(_actionFSM, _shootSpeed, _shootingDuration, transform, _moveComponent, ChangeBodyScale, 
                    _skillController.OnReflect, SetInvincible) 
                }
            };
            _actionFSM.Inintialize(actionStates, ActionState.Ready);
        }

        protected override void OnDie()
        {
            ClearEvent();
            MainEventBus.Publish(MainEventBus.State.GameOver);
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
                    ObserverEventBus.Publish(ObserverEventBus.State.OnDashRatioChange, TotalDashRatio);
                    //OnDachRatioChangeRequested?.Invoke(TotalDashRatio);

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

        public void AddBuff(BaseBuff.Name name, BaseBuff buff)
        {
            _buffController.AddBuff(name, buff);
        }

        public void RemoveBuff(BaseBuff.Name name)
        {
            _buffController.RemoveBuff(name);
        }
    }
}