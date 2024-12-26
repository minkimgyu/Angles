using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseEnemy : BaseLife, ICaster, IFollowable, IForce
{
    protected SkillController _skillController;
    protected float _moveSpeed;

    protected MoveComponent _moveComponent;
    Vector3 _dir;

    protected DropData _dropData;
    Action OnDieRequested;

    public override void Initialize()
    {
        base.Initialize();
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _skillController = GetComponent<SkillController>();
        _skillController.Initialize(new NoUpgradeableData(), this);

        OnHpChangeRequested += (float ratio) => _skillController.OnDamaged(ratio);
    }

    protected override void UpdateOnIdle()
    {
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

    public bool CanApplyForce()
    {
        return _lifeState == LifeState.Alive && (_aliveState == AliveState.Normal || _aliveState == AliveState.Groggy);
    }

    public void ApplyForce(Vector3 direction, float force, ForceMode2D mode)
    {
        _moveComponent.AddForce(direction, force, mode);
    }

    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        return _skillController.ReturnSkillUpgradeDatas();
    }

    public void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { _skillController.AddSkill(skillName, skill); }

    public bool CanFollow() { return true; }
    public Vector3 ReturnFowardDirection() { return transform.forward; }
}