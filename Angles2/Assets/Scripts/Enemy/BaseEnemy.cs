using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseEnemy : BaseLife, IFlock, IFollowable, IAbsorbable
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

    protected BaseEffect.Name _destoryEffect;

    protected IPos _followTarget;

    protected float _offsetFromCenter;

    public override void Initialize()
    {
        _followTarget = FindObjectOfType<Player.Player>();
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
        _skillController.AddSkill(_skillNames);
    }

    protected override void Update()
    {
        base.Update();
        _skillController.OnUpdate();
    }

    protected void ResetDirection()
    {
        if (_followTarget == null) return;

        Vector3 targetPos = _followTarget.ReturnPosition();
        BehaviorData data = new BehaviorData(_nearAgents, _obstacles, targetPos, _offsetFromCenter);
        _dir = _flockComponent.ReturnDirection(data);
    }

    protected void MoveToDirection()
    {
        if (_aliveState == AliveState.Groggy) return; // 그로기 상태인 경우 실행 X
        _moveComponent.Move(_dir.normalized, _moveSpeed);
    }
    protected override void OnDie()
    {
        BaseEffect effect = EffectFactory.Create(_destoryEffect);
        effect.ResetPosition(transform.position);
        effect.Play();

        Destroy(gameObject);
    }

    public Vector3 ReturnFowardDirection()
    {
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

    public void Absorb(Vector3 pos, float speed)
    {
        Vector3 direction = pos - transform.position;
        _moveComponent.AddForce(direction, speed);
    }
}