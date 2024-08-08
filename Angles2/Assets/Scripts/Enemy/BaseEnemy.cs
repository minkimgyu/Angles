using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseEnemy : BaseLife, IFlock, IFollowable, IForce, ISkillUser
{
    FlockCaptureComponent _flockCaptureComponent;
    ObstacleCaptureComponent _obstacleCaptureComponent;

    protected SkillController _skillController;
    protected List<BaseSkill.Name> _skillNames;
    protected float _moveSpeed;

    FlockComponent _flockComponent;
    protected MoveComponent _moveComponent;

    Vector3 _dir;

    List<IFlock> _nearAgents;
    List<IObstacle> _obstacles;

    protected IPos _followTarget;

    protected float _offsetFromCenter;
    protected DropData _dropData;

    public override void SetTarget(IPos follower)
    {
        _followTarget = follower;
    }

    public override void Initialize()
    {
        _groggyTimer = new Timer();
        _hp = _maxHp;

        _nearAgents = new List<IFlock>();
        _obstacles = new List<IObstacle>();

        _flockCaptureComponent = GetComponentInChildren<FlockCaptureComponent>();
        _flockCaptureComponent.Initialize(_nearAgents.Add, (item) => { _nearAgents.Remove(item); });

        _obstacleCaptureComponent = GetComponentInChildren<ObstacleCaptureComponent>();
        _obstacleCaptureComponent.Initialize(_obstacles.Add, (item) => { _obstacles.Remove(item); });

        _flockComponent = GetComponent<FlockComponent>();
        _flockComponent.Initialize();

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _skillController = GetComponent<SkillController>();
        _skillController.Initialize();
    }

    protected override void Update()
    {
        base.Update();
        _skillController.OnUpdate();
    }

    protected override void OnDie()
    {
        // 아이템 드랍 기능 넣기
        OnDropRequested?.Invoke(_dropData, transform.position);
        base.OnDie();
    }

    Action<DropData, Vector3> OnDropRequested; // 드랍 시 호출

    public override void AddObserverEvent(Action OnDieRequested, Action<DropData, Vector3> OnDropRequested)
    {
        this.OnDieRequested = OnDieRequested;
        this.OnDropRequested = OnDropRequested;
    }

    public override void AddCreateEvent(Func<BaseEffect.Name, BaseEffect> CreateEffect) 
    {
        this.CreateEffect = CreateEffect;
    }

    protected void ResetDirection()
    {
        if ((_followTarget as UnityEngine.Object) == null) return;

        Vector3 targetPos = _followTarget.ReturnPosition();
        BehaviorData data = new BehaviorData(_nearAgents, _obstacles, targetPos, _offsetFromCenter);
        _dir = _flockComponent.ReturnDirection(data);
    }

    protected void MoveToDirection()
    {
        if (_aliveState == AliveState.Groggy) return; // 그로기 상태인 경우 실행 X
        _moveComponent.Move(_dir.normalized, _moveSpeed);
    }

    public Vector3 ReturnFowardDirection()
    {
        if (transform == null) return Vector3.zero;
        return transform.right;
    }

    public bool CanFollow()
    {
        return _lifeState == LifeState.Alive;
    }

    public bool CanAbsorb()
    {
        return _lifeState == LifeState.Alive;
    }

    public void ApplyForce(Vector3 pos, float speed, ForceMode2D forceMode)
    {
        Vector3 direction = pos - transform.position;
        _moveComponent.AddForce(direction, speed, forceMode);
    }

    public void AddSkill(BaseSkill.Name name) { }
    public void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { _skillController.AddSkill(skillName, skill); }
}