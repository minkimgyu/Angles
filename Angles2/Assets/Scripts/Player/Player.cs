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

        List<BaseSkill.Name> _skillNames;

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

        void ResetPosition(Vector2 pos) { transform.position = pos; }

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

            _skillNames = data._skillNames;
        }

        public override void Initialize()
        {
            _groggyTimer = new Timer();
            _hp = _maxHp;

            _targetType = ITarget.Type.Blue;

            _interactableCaptureComponent = GetComponentInChildren<InteractableCaptureComponent>();
            _interactableCaptureComponent.Initialize(OnInteractableEnter, OnInteractableExit);

            SkillUIController skillUIController = FindObjectOfType<SkillUIController>();
            skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active });

            _skillController = GetComponent<SkillController>();
            _skillController.Initialize(skillUIController.AddViewer, skillUIController.RemoveViewer);
            _skillController.AddSkill(BaseSkill.Name.ContactAttack);

            BaseViewer hpViewer = ViewerFactory.Create(BaseViewer.Name.HpViewer);
            hpViewer.Initialize();
            hpViewer.SetFollower(this);
            OnHpChange = hpViewer.UpdateViewer;

            DashUIController dashUIController = FindObjectOfType<DashUIController>();
            dashUIController.Initialize(_dashCount);
            UpdateDashViewer = dashUIController.UpdateViewer;

            CardController cardController = FindObjectOfType<CardController>();
            cardController.Initialize((name) => _skillController.AddSkill(name));

            CameraController cameraController = FindObjectOfType<CameraController>();
            cameraController.Initialize();
            cameraController.SetTracker(this);

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

            BaseViewer hpViewer = ViewerFactory.Create(BaseViewer.Name.HpViewer);
            hpViewer.Initialize();
            hpViewer.SetFollower(this);
            OnHpChange = hpViewer.UpdateViewer;

            BaseViewer directionViewer = ViewerFactory.Create(BaseViewer.Name.DirectionViewer);
            directionViewer.Initialize();
            directionViewer.SetFollower(this);

            _actionFSM = new FSM<ActionState>();
            Dictionary<ActionState, BaseState<ActionState>> actionStates = new Dictionary<ActionState, BaseState<ActionState>>();
            actionStates.Add(ActionState.Ready, new ReadyState(_actionFSM));

            actionStates.Add(ActionState.Charge, new ChargeState(_actionFSM, _minJoystickLength, transform, ChangeBodyScale, SetInvincible,
                (value) => { directionViewer.OnOffViewer(value); _moveComponent.ApplyDirection = !value; },
                directionViewer.UpdateViewer,
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
                //Debug.Log(_currentDashCount);
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

        void OnInteractableEnter(IInteractable interactable)
        {
            Debug.Log(interactable);

            _interactableObj = interactable;

            InteractEnterData enterData = new InteractEnterData(this);
            interactable.OnInteractEnter(enterData);
        }

        void OnInteract()
        {
            if (_interactableObj == null) return;

            InteractData interactData = new InteractData(_skillController.ReturnSkillUpgradeDatas, _skillController.AddSkill, ResetPosition);
            _interactableObj.OnInteract(interactData);
        }

        void OnInteractableExit(IInteractable interactable)
        {
            _interactableObj = null;

            InteractExitData exitData = new InteractExitData();
            interactable.OnInteractExit(exitData);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
            if(interactable != null && interactable == _interactableObj)
            {
                OnInteract();
            }

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
                _actionFSM.OnChargeStart
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