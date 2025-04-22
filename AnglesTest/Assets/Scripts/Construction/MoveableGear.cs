using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableGear : BaseConstruction
{
    const float _attackDelay = 1f;
    const float _attackDamage = 5;
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] Transform _gearTransform;

    [SerializeField] List<Transform> _movePoints;

    Vector3 rotationAxis = Vector3.up; // ȸ�� ��
    float rotationSpeed = 90f;         // �ʴ� ȸ�� �ӵ� (��/��)

    Timer _soundTimer;

    public override void Initialize()
    {
        base.Initialize();
        _soundTimer = new Timer();
        _soundTimer.Start(1f);
    }

    protected override void Update()
    {
        base.Update();
        _gearTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (_soundTimer.CurrentState == Timer.State.Finish)
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Blade, transform.position);
            _soundTimer.Reset();
            _soundTimer.Start(_attackDelay);
        }
    }

    public override void InitializeStrategy()
    {
        base.InitializeStrategy();
        DamageableCaptureComponent damageableCaptureComponent = GetComponentInChildren<DamageableCaptureComponent>();
        MoveComponent moveComponent = GetComponent<MoveComponent>();
        moveComponent.Initialize();

        _detectingStrategy = new BladeDetectingStrategy(damageableCaptureComponent);
        _detectingStrategy.InjectTargetTypes(new List<ITarget.Type> { ITarget.Type.Blue, ITarget.Type.Red });

        _actionStrategy = new GearAttackStrategy(
            _attackDelay,
            new DamageableData(new DamageStat(_attackDamage)),
            _detectingStrategy.GetBladeTargets
        );

        _moveStrategy = new MoveBetweenPointsStrategy(_movePoints, transform, moveComponent, _moveSpeed);
    }
}
