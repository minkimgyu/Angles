using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Player.FSM;

namespace Player
{
    public class Player : BaseLife, IFollowable
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

        float _dashSpeed;
        float _dashDuration;

        float _shootSpeed;
        float _shootDuration;

        float _minJoystickLength;

        int _dashCount;

        int _dashConsumeCount;
        float _dashRestoreDuration;

        float _shrinkScale;
        float _normalScale;

        float _currentDashFillDuration = 0;
        int _currentDashCount;

        float DashRatio { get { return _currentDashFillDuration / _dashRestoreDuration; } }
        float TotalDashRatio { get { return _dashCount + DashRatio; } }
        bool CanUseDash() { return _dashCount >= _dashConsumeCount; }

        MoveComponent _moveComponent;
        OutlineComponent _outlineComponent;
        SkillController _skillController;

        InteractableCaptureComponent _interactableCaptureComponent;
        IInteractable _interactableObj;

        Action<float> UpdateDashViewer;

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

            _dashSpeed = data._dashSpeed;
            _dashDuration = data._dashDuration;

            _shootSpeed = data._shootSpeed;
            _shootDuration = data._shootDuration;

            _minJoystickLength = data._minJoystickLength;

            _dashCount = data._dashCount;
            _dashConsumeCount = data._dashConsumeCount;
            _dashRestoreDuration = data._dashRestoreDuration;

            _shrinkScale = data._shrinkScale;
            _normalScale = data._normalScale;
        }

        void OnInteractableEnter(IInteractable interactable)
        {
            _interactableObj = interactable;
            interactable.OnInteractEnter(this);
        }

        void OnClickRightScreen()
        {
            _actionFSM.OnChargeStart();
            if (_interactableObj == null) return;

            List<SkillUpgradeData> skillUpgradeDatas = _skillController.ReturnSkillUpgradeDatas();
            _interactableObj.OnInteract(skillUpgradeDatas);
        }

        void OnInteractableExit(IInteractable interactable)
        {
            _interactableObj = null;
            interactable.OnInteractExit();
        }

        public override void Initialize()
        {
            _groggyTimer = new Timer();
            _hp = _maxHp;

            _targetType = ITarget.Type.Blue;

            _interactableCaptureComponent = GetComponentInChildren<InteractableCaptureComponent>();
            _interactableCaptureComponent.Initialize(OnInteractableEnter, OnInteractableExit);

            _skillController = GetComponent<SkillController>();
            _skillController.Initialize();

            //BaseSkill impact = SkillFactory.Create(BaseSkill.Name.Impact);
            //BaseSkill knockback = SkillFactory.Create(BaseSkill.Name.Knockback);
            //BaseSkill statikk = SkillFactory.Create(BaseSkill.Name.Statikk);

            //BaseSkill spawnBlackhole = SkillFactory.Create(BaseSkill.Name.SpawnBlackhole);
            //BaseSkill spawnBlade = SkillFactory.Create(BaseSkill.Name.SpawnBlade);
            //BaseSkill spawnShooter = SkillFactory.Create(BaseSkill.Name.SpawnShooter);
            //BaseSkill spawnStickyBomb = SkillFactory.Create(BaseSkill.Name.SpawnStickyBomb);

            _skillController.AddSkill(BaseSkill.Name.Impact);
            _skillController.AddSkill(BaseSkill.Name.Knockback);
            _skillController.AddSkill(BaseSkill.Name.Statikk);

            HpViewer hpViewer = FindObjectOfType<HpViewer>();
            hpViewer.Initialize();
            hpViewer.SetTracker(this);

            OnHpChange = hpViewer.OnHpChange;

            DashUIController dashUIController = FindObjectOfType<DashUIController>();
            dashUIController.Initialize(_dashCount);

            CardController cardController = FindObjectOfType<CardController>();
            cardController.Initialize((name) => _skillController.AddSkill(name));

            CameraController cameraController = FindObjectOfType<CameraController>();
            cameraController.Initialize();
            cameraController.SetTracker(this);

            UpdateDashViewer = dashUIController.UpdateViewer;

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
            Dictionary<MovementState, BaseState<MovementState>> movementStates = new Dictionary<MovementState, BaseState<MovementState>>();

            movementStates.Add(MovementState.Stop, new StopState(_movementFSM, _moveComponent.Stop));

            movementStates.Add(MovementState.Move,
                new MoveState(_movementFSM, _moveSpeed, CanUseDash, OnUseDash, _moveComponent.Move));

            movementStates.Add(MovementState.Dash,
                new DashState(_movementFSM, _dashSpeed, _dashDuration, ChangeBodyScale, OnEndDash, _moveComponent.AddForce));

            _movementFSM.Inintialize(movementStates, MovementState.Stop);

            DirectionViewer directionViewer = FindObjectOfType<DirectionViewer>();
            directionViewer.Initialize();

            _actionFSM = new FSM<ActionState>();
            Dictionary<ActionState, BaseState<ActionState>> actionStates = new Dictionary<ActionState, BaseState<ActionState>>();
            actionStates.Add(ActionState.Ready, new ReadyState(_actionFSM));

            actionStates.Add(ActionState.Charge, new ChargeState(_actionFSM, _minJoystickLength, transform, ChangeBodyScale, SetInvincible,
                (value) => { directionViewer.OnOffDirectionSprite(value); _moveComponent.ApplyDirection = !value; },
                directionViewer.UpdatePosition,
                _moveComponent.FaceDirection));

            actionStates.Add(ActionState.Shoot,
                new ShootState(_actionFSM, _shootSpeed, _shootDuration,
                ChangeBodyScale, _skillController.OnReflect, SetInvincible, _moveComponent.Stop, _moveComponent.AddForce));

            actionStates.Add(ActionState.Reflect, new ReflectState(_actionFSM, transform));

            _actionFSM.Inintialize(actionStates, ActionState.Ready);
        }

        protected override void OnDie()
        {
            ClearEvent();
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
            if (_dashCount == _currentDashCount) return;

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
                    UpdateDashViewer?.Invoke(TotalDashRatio);

                    _skillController.OnUpdate();

                    _movementFSM.OnUpdate();
                    _actionFSM.OnUpdate();
                    break;
                default:
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _actionFSM.OnCollisionEnter(collision);
        }

        void ClearEvent()
        {
            InputManager.ClearEvent(InputManager.Side.Left);
            InputManager.ClearEvent(InputManager.Side.Right);
        }

        void AddEvent()
        {
            InputManager.AddEvent(
               InputManager.Side.Left,
               InputManager.Type.OnInputStart,
               _movementFSM.OnMoveStart
           );

            InputManager.AddEvent(
                InputManager.Side.Left,
                InputManager.Type.OnInput,
                (value) => { _movementFSM.OnMove(value); _actionFSM.OnMove(value); }
            );

            InputManager.AddEvent(
                InputManager.Side.Left,
                InputManager.Type.OnInputEnd,
                _movementFSM.OnMoveEnd
            );

            InputManager.AddEvent(
              InputManager.Side.Right,
              InputManager.Type.OnDoubleTab,
              _movementFSM.OnDash
          );


            InputManager.AddEvent(
               InputManager.Side.Right,
               InputManager.Type.OnInputStart,
               OnClickRightScreen
           );

            InputManager.AddEvent(
               InputManager.Side.Right,
               InputManager.Type.OnInput,
              _actionFSM.OnCharge
            );

            InputManager.AddEvent(
                InputManager.Side.Right,
                InputManager.Type.OnInputEnd,
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