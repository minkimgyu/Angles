using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseEnemy : BaseLife, ISkillAddable, IFollowable //, IFlock, , IForce
{
    //FlockCaptureComponent _flockCaptureComponent;
    //ObstacleCaptureComponent _obstacleCaptureComponent;

    protected SkillController _skillController;
    protected List<BaseSkill.Name> _skillNames;
    protected float _moveSpeed;

    //FlockComponent _flockComponent;
    protected MoveComponent _moveComponent;

    Vector3 _dir;

    //List<IFlock> _nearAgents;
    //List<IObstacle> _obstacles;

    //protected IPos _followTarget;

    //protected float _offsetFromCenter;
    protected DropData _dropData;

    Action OnDieRequested;

    BuffFloat _totalDamageRatio;
    BuffFloat _totalCooltimeRatio;

    //public override void SetTarget(IPos follower)
    //{
    //    _followTarget = follower;
    //}

    public override void Initialize()
    {
        _totalDamageRatio = new BuffFloat(1, 1, 1);
        _totalCooltimeRatio = new BuffFloat(1, 1, 1);

        _groggyTimer = new Timer();
        _hp = _maxHp;

        //_nearAgents = new List<IFlock>();
        //_obstacles = new List<IObstacle>();

        //_flockCaptureComponent = GetComponentInChildren<FlockCaptureComponent>();
        //_flockCaptureComponent.Initialize(_nearAgents.Add, (item) => { _nearAgents.Remove(item); });

        //_obstacleCaptureComponent = GetComponentInChildren<ObstacleCaptureComponent>();
        //_obstacleCaptureComponent.Initialize(_obstacles.Add, (item) => { _obstacles.Remove(item); });

        //_flockComponent = GetComponent<FlockComponent>();
        //_flockComponent.Initialize();

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _skillController = GetComponent<SkillController>();
        _skillController.Initialize(_totalDamageRatio, _totalCooltimeRatio);
    }

    protected override void Update()
    {
        base.Update();
        _skillController.OnUpdate();
    }

    public override void AddObserverEvent(Action OnDieRequested)
    {
        this.OnDieRequested = OnDieRequested;
    }

    protected override void OnDie()
    {
        // 아이템 드랍 기능 넣기
        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.DropItem, _dropData, transform.position);
        OnDieRequested?.Invoke();
        base.OnDie();
    }

    public override void AddEffectFactory(BaseFactory _effectFactory) 
    {
        this._effectFactory = _effectFactory;
    }

    //protected void ResetDirection()
    //{
    //    if ((_followTarget as UnityEngine.Object) == null) return;

    //    Vector3 targetPos = _followTarget.ReturnPosition();
    //    BehaviorData data = new BehaviorData(_nearAgents, _obstacles, targetPos, _offsetFromCenter);
    //    _dir = _flockComponent.ReturnDirection(data);
    //}

    //protected void MoveToDirection()
    //{
    //    if (_aliveState == AliveState.Groggy) return; // 그로기 상태인 경우 실행 X
    //    _moveComponent.Move(_dir.normalized, _moveSpeed);
    //}

    //public Vector3 ReturnFowardDirection()
    //{
    //    if (transform == null) return Vector3.zero;
    //    return transform.right;
    //}

    //public bool CanFollow()
    //{
    //    return _lifeState == LifeState.Alive;
    //}

    //public bool CanAbsorb()
    //{
    //    return _lifeState == LifeState.Alive;
    //}

    public void ApplyForce(Vector3 pos, float speed, ForceMode2D forceMode)
    {
        Vector3 direction = pos - transform.position;
        _moveComponent.AddForce(direction, speed, forceMode);
    }

    public void AddSkill(BaseSkill.Name name) { }
    public void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { _skillController.AddSkill(skillName, skill); }
    public List<SkillUpgradeData> ReturnSkillUpgradeDatas() { return default; }

    public bool CanFollow() { return true; }
    public Vector3 ReturnFowardDirection() { return transform.forward; }
}