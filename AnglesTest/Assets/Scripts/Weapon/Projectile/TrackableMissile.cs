using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AttackStrategy
//공격 방식을 정하는 전략

// ProjectileWeapon 없애고 IProjectable 이거 상속해서 쓰자

// 일정 시간있다가 범위 데미지 StickyBombAttackStrategy
// 충돌 시 데미지 BulletAttackStrategy
// 범위 안에 있으면 데미지 RocketAttackStrategy
// 범위 안에 있으면 데미지 + 흡수 BlackholeAttackStrategy
 
 
// .
// .
// .

public class TrackableMissile : BaseWeapon, IProjectable
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        _attackStrategy.OnTargetEnter(collider);
    }

    BaseFactory _effectFactory;
    TrackableMissileData _data;

    void OnHit()
    {
        SpawnHitEffect();
        Destroy(gameObject);
    }

    void SpawnHitEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void ModifyData(TrackableMissileDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }

    public override void InjectData(TrackableMissileData data)
    {
        _data = data;
    }

    ITarget _target;
    float _moveSpeed;
    Pathfinder _pathfinder;

    public void Shoot(ITarget target, float speed)
    {
        _target = target;
        _moveSpeed = speed;
    }

    List<Vector2> _movePoints;
    int _index = 0;
    float _closeDistance = 0.5f;
    Vector2 _dir;

    Timer _pathfinderTimer;
    float _pathfindGap = 0.5f;

    void ChangeDirection()
    {
        if (_movePoints == null) return;
        if (_index >= _movePoints.Count)
        {
            _dir = Vector2.zero;
            return;
        }

        DrawMovePoints();

        Vector2 nextMovePos = _movePoints[_index];
        _dir = (nextMovePos - (Vector2)transform.position).normalized;

        bool nowCloseToNextPoint = Vector2.Distance(transform.position, nextMovePos) < _closeDistance;
        if (nowCloseToNextPoint) _index++;
    }

    void DrawMovePoints()
    {
#if UNITY_EDITOR
        for (int i = 1; i < _movePoints.Count; i++)
        {
            Debug.DrawLine(_movePoints[i - 1], _movePoints[i], Color.cyan);
        }
#endif
    }

    protected override void Update()
    {
        base.Update();
         
        _movePoints = _pathfinder.FindPath(transform.position, _target.GetPosition(), BaseLife.Size.Small);
        _index = 0;

        _pathfinderTimer.Reset();
        _pathfinderTimer.Start(_pathfindGap);
    }

    private void FixedUpdate()
    {
        _moveComponent.Move(_dir, _moveSpeed);
    }

    protected float _force;
    protected MoveComponent _moveComponent;

    public override void Initialize(BaseFactory effectFactory)
    {
        _effectFactory = effectFactory;
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, () => { Destroy(gameObject); });
        _sizeStrategy = new NoSizeStrategy();
        _attackStrategy = new BulletAttackStrategy(_data, OnHit);

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }
}
