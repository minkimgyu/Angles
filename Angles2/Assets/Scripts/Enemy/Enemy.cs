using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Enemy : Life, IFlock, IAttachable, IAbsorbable
{
    FlockCaptureComponent _flockCaptureComponent;
    ObstacleCaptureComponent _obstacleCaptureComponent;

    FlockComponent _flockComponent;
    MoveComponent _moveComponent;

    Func<Vector3> ReturnPlayerPos;

    Vector3 _dir;

    List<IFlock> _nearAgents;
    List<IObstacle> _obstacles;

    [SerializeField] float _moveSpeed = 3;

    protected override void Start()
    {
        base.Start();

        _nearAgents = new List<IFlock>();
        _obstacles = new List<IObstacle>();

        _targetType = ITarget.Type.Red;

        Player.Player player = FindObjectOfType<Player.Player>();
        ReturnPlayerPos = () => { return player.transform.position; };

        _flockCaptureComponent = GetComponentInChildren<FlockCaptureComponent>();
        _flockCaptureComponent.Initialize(_nearAgents.Add, (item) => { _nearAgents.Remove(item); });

        _obstacleCaptureComponent = GetComponentInChildren<ObstacleCaptureComponent>();
        _obstacleCaptureComponent.Initialize(_obstacles.Add, (item) => { _obstacles.Remove(item); });

        _flockComponent = GetComponent<FlockComponent>();
        _flockComponent.Initialize();

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }

    private void Update()
    {
        Vector3 playerPos = ReturnPlayerPos();

        BehaviorData data = new BehaviorData(_nearAgents, _obstacles, playerPos);
        _dir = _flockComponent.ReturnDirection(data);
    }

    private void FixedUpdate()
    {
        _moveComponent.Move(_dir.normalized, _moveSpeed);
    }

    protected override void OnDie()
    {
    }

    public Vector3 ReturnFowardDirection()
    {
        return transform.up;
    }

    public bool CanAttach()
    {
        return _lifeState == LifeState.Alive;
    }

    public void Absorb(Vector3 pos, float speed)
    {
        Vector3 direction = transform.position - pos;
        _moveComponent.AddForce(direction, speed);
    }
}