using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

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

    public bool CanAbsorb()
    {
        return _lifeState == LifeState.Alive;
    }

    public void ApplyForce(Vector3 pos, float speed, ForceMode2D forceMode)
    {
        Vector3 direction = pos - transform.position;
        _moveComponent.AddForce(direction, speed, forceMode);
    }

    public override void GetDamage(DamageableData damageableData)
    {
        base.GetDamage(damageableData);
        
    }

    public List<SkillUpgradeData> ReturnSkillUpgradeDatas()
    {
        return _skillController.ReturnSkillUpgradeDatas();
    }

    public void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { _skillController.AddSkill(skillName, skill); }

    public bool CanFollow() { return true; }
    public Vector3 ReturnFowardDirection() { return transform.forward; }
}