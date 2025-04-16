using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skill;

abstract public class BaseEnemy : BaseLife, ICaster, IFollowable, IForce
{
    protected SkillController _skillController;
    //protected float _moveSpeed;

    protected IMoveStrategy _moveStrategy;
    //protected MoveComponent _moveComponent;
    //Vector3 _dir;

    protected DropData _dropData;
    Action OnDieRequested;

    public virtual Vector2 BottomPoint { get { return Vector2.zero; } }

    protected override void SetUp(
       LifeData data,
       DropData dropData) 
    {
        base.SetUp(data, dropData);
        _dropData = dropData;
    }

    public override void Initialize()
    {
        base.Initialize();
        _skillController = GetComponent<SkillController>();
        _skillController.Initialize(new NoUpgradeableData(), this);

        OnHpChangeRequested += (float ratio) => _skillController.OnDamaged(ratio);
    }

    protected override void Update()
    {
        base.Update();

        if (_aliveState == AliveState.Groggy) return;
        _moveStrategy.OnUpdate();
    }

    void FixedUpdate()
    {
        if (_aliveState == AliveState.Groggy) return;
        _moveStrategy.OnFixedUpdate();
    }

    protected override void UpdateOnIdle()
    {
        _skillController.OnUpdate();
    }

    public override void InjectEvent(Action OnDieRequested)
    {
        this.OnDieRequested = OnDieRequested;
    }

    protected override void OnDie()
    {
        // 아이템 드랍 기능 넣기
        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.DropItem, _dropData, transform.position);
        OnDieRequested?.Invoke();
        base.OnDie();
        Destroy(gameObject);
    }

    public override void InjectEffectFactory(BaseFactory effectFactory) 
    {
        _effectFactory = effectFactory;
    }

    public bool CanApplyForce()
    {
        return _lifeState == LifeState.Alive && (_aliveState == AliveState.Normal || _aliveState == AliveState.Groggy);
    }

    public void ApplyForce(Vector3 direction, float force, ForceMode2D mode)
    {
        _moveStrategy.ApplyForce(direction, force, mode);
        //_moveComponent.AddForce(direction, force, mode);
    }

    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        return _skillController.ReturnSkillUpgradeDatas();
    }

    public void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { _skillController.AddSkill(skillName, skill); }

    public bool CanFollow() { return _lifeState == LifeState.Alive; }
    public Vector3 ReturnFowardDirection() { return transform.forward; }
}