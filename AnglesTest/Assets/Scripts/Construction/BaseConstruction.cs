using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConstruction : MonoBehaviour
{
    protected IWeaponTargetingStrategy _targetingStrategy;
    protected IWeaponDetectingStrategy _detectingStrategy;
    protected IWeaponActionStrategy _actionStrategy;
    protected IWeaponMoveStrategy _moveStrategy;

    protected virtual void Update()
    {
        _moveStrategy.OnUpdate();
        _actionStrategy.OnUpdate();
    }

    protected virtual void FixedUpdate()
    {
        _moveStrategy.OnFixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _targetingStrategy.OnTriggerEnter(collider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _moveStrategy.OnCollisionEnter(collision);
    }

    public virtual void InitializeStrategy()
    {
        _targetingStrategy = new NoTargetingStrategy();
        _detectingStrategy = new NoDetectingStrategy();
        _actionStrategy = new NoAttackStrategy();
        _moveStrategy = new NoMoveStrategy();
    }

    private void Start()
    {
        Initialize();
    }

    public virtual void Initialize() { InitializeStrategy(); }
}