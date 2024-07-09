using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Player.FSM;

namespace Player
{
    public class Player : Life
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

        [SerializeField] float _moveSpeed = 5f;
        [SerializeField] float _dashSpeed = 10f;
        [SerializeField] float _dashDuration = 1.5f;

        [SerializeField] float _minShootValue = 0.6f;
        [SerializeField] float _shootSpeed = 15f;
        [SerializeField] float _shootDuration = 1.5f;

        [SerializeField] int _maxDashCount = 3;
        [SerializeField] int _currentDashCount = 3;

        [SerializeField] int _dashUseCount = 1;

        float _dashFillDuration = 0;
        [SerializeField] float _maxDashFillDuration = 5f;

        float DashRatio { get { return _dashFillDuration / _maxDashFillDuration; } }
        float TotalDashRatio { get { return _currentDashCount + DashRatio; } }

        MoveComponent _moveComponent;
        OutlineComponent _outlineComponent;
        SkillController _skillController;

        Action<float> UpdateViewer;

        float _maxScale = 0.3f;
        float MinScale { get { return _maxScale * 0.6f; } }

        void ChangeBodyScale(bool xAxis, float ratio)
        {
            if(xAxis) transform.localScale = 
                    Vector3.Lerp(
                        new Vector3(MinScale, _maxScale, transform.localScale.z), 
                        new Vector3(_maxScale, _maxScale, transform.localScale.z), ratio);

            else transform.localScale = 
                    Vector3.Lerp(
                        new Vector3(_maxScale, MinScale, transform.localScale.z),
                        new Vector3(_maxScale, _maxScale, transform.localScale.z), ratio);
        }

        protected override void SetInvincible(bool nowInvincible)
        {
            base.SetInvincible(nowInvincible);
            _moveComponent.FreezeRotation(nowInvincible);

            if (nowInvincible) _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnInvincible);
            else _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
        }

        void SetState(MovementState state) => _movementFSM.SetState(state);
        void SetState(MovementState state, Vector2 vec2, string message) => _movementFSM.SetState(state, vec2, message);

        void RevertToPreviousState(Vector2 vec2, string message) => _actionFSM.RevertToPreviousState(vec2, message);

        void SetState(ActionState state) => _actionFSM.SetState(state);
        void SetState(ActionState state, Vector2 vec2, string message) => _actionFSM.SetState(state, vec2, message);


        protected override void Start()
        {
            base.Start();
            _targetType = ITarget.Type.Blue;

            _skillController = GetComponent<SkillController>();
            _skillController.Initialize();

            DashUIController dashUIController = FindObjectOfType<DashUIController>();
            dashUIController.Initialize(_maxDashCount);

            CameraController cameraController = FindObjectOfType<CameraController>();
            cameraController.Initialize();
            cameraController.OnFollowRequested(() => transform.position);

            UpdateViewer = dashUIController.UpdateViewer;

            _outlineComponent = GetComponent<OutlineComponent>();
            _outlineComponent.Initialize();

            _moveComponent = GetComponent<MoveComponent>();
            _moveComponent.Initialize();

            DirectionViewer directionViewer = FindObjectOfType<DirectionViewer>();
            directionViewer.Initialize();

            Dictionary<MovementState, BaseState> movementStates = new Dictionary<MovementState, BaseState>();
            movementStates.Add(MovementState.Stop, new StopState(SetState, _moveComponent.Stop));

            movementStates.Add(MovementState.Move,
                new MoveState(_moveSpeed, CanUseDash, OnUseDash, _moveComponent.Move, SetState, SetState));

            movementStates.Add(MovementState.Dash,
                new DashState(_dashSpeed, _dashDuration, ChangeBodyScale, OnEndDash, SetState, _moveComponent.AddForce));

            _movementFSM = new FSM<MovementState>();
            _movementFSM.Inintialize(movementStates, MovementState.Stop);


            Dictionary<ActionState, BaseState> actionStates = new Dictionary<ActionState, BaseState>();
            actionStates.Add(ActionState.Ready, new ReadyState(SetState));

            actionStates.Add(ActionState.Charge, new ChargeState(_minShootValue, transform, ChangeBodyScale, SetInvincible,
                (value) => { directionViewer.OnOffDirectionSprite(value); _moveComponent.ApplyDirection = !value; },
                directionViewer.UpdatePosition,
                _moveComponent.FaceDirection, SetState, SetState));

            actionStates.Add(ActionState.Shoot,
                new ShootState(_shootSpeed, _shootDuration,
                ChangeBodyScale, _skillController.OnReflect, SetInvincible, SetState, SetState, _moveComponent.AddForce));

            actionStates.Add(ActionState.Reflect, new ReflectState(transform, RevertToPreviousState));

            _actionFSM = new FSM<ActionState>();
            _actionFSM.Inintialize(actionStates, ActionState.Ready);

            AddEvent();
        }

        protected override void OnDie()
        {
            ClearEvent();
        }

        bool CanUseDash() { return _currentDashCount >= _dashUseCount; }
        void OnUseDash() 
        { 
            _currentDashCount -= _dashUseCount;
            _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnDash);
        }

        void OnEndDash()
        {
            _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
        }

        void FillDashCount()
        {
            if (_maxDashCount == _currentDashCount) return;

            _dashFillDuration += Time.deltaTime;
            if (DashRatio >= 1)
            {
                _currentDashCount++;
                _dashFillDuration = 0;
            }
        }

        private void FixedUpdate()
        {
            switch (_lifeState)
            {
                case LifeState.Alive:
                    _movementFSM.OnFixedUpdate();
                    break;
                default:
                    break;
            }
        }


        private void Update()
        {
            switch (_lifeState)
            {
                case LifeState.Alive:
                    FillDashCount();
                    UpdateViewer?.Invoke(TotalDashRatio);

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
    }

}