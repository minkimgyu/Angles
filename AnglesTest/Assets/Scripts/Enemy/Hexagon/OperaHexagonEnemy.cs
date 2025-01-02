using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OperaHexagonData : HexagonData
{
    [JsonProperty] private float _freezeDuration;
    [JsonProperty] private float _movableDuration;
    [JsonProperty] private float _freezeSpeed;

    [JsonIgnore] public float FreezeDuration { get => _freezeDuration; set => _freezeDuration = value; }
    [JsonIgnore] public float MovableDuration { get => _movableDuration; set => _movableDuration = value; }
    [JsonIgnore] public float FreezeSpeed { get => _freezeSpeed; set => _freezeSpeed = value; }

    public OperaHexagonData(
        float maxHp,
        ITarget.Type targetType,
        BaseLife.Size size,
        Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed,
        float stopDistance,

        float freezeSpeed,
        float freezeDuration,
        float movableDuration,

        float gap) : base(maxHp, targetType, size, skillDataToAdd, moveSpeed, stopDistance, gap)
    {
        _freezeDuration = freezeDuration;
        _movableDuration = movableDuration;
        _freezeSpeed = freezeSpeed;
    }

    public override LifeData Copy()
    {
        return new OperaHexagonData(
            _maxHp, // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(_skillData), // 딕셔너리 깊은 복사

            _moveSpeed, // TriangleData 고유 값
            _stopDistance,

            _freezeSpeed,
            _freezeDuration,
            _movableDuration,

            _gap
        );
    }
}

public class OperaHexagonEnemy : TrackableEnemy
{
    [SerializeField] TargetCaptureComponent _skillTargetCaptureComponent;

    float _movableDuration;
    float _freezeDuration;
    float _freezeSpeed;

    public override void ResetData(OperaHexagonData data, DropData dropData)
    {
        base.ResetData(data, dropData);
        _size = data.Size;
        _targetType = data.TargetType;
        _moveSpeed = data.MoveSpeed;
        _dropData = dropData;

        _movableDuration = data.MovableDuration;
        _freezeDuration = data.FreezeDuration;
        _freezeSpeed = data.FreezeSpeed;

        _stopDistance = data.StopDistance;
        _gap = data.Gap;
        _destoryEffect = BaseEffect.Name.HexagonDestroyEffect;
    }

    public override void InitializeFSM(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _trackComponent = new FlexibleTrackingComponent(
            _moveComponent,
            transform,
            _size,
            _freezeSpeed,
            _moveSpeed,
            _stopDistance,
            _gap,
            _freezeDuration,
            _movableDuration,
            FindPath
        );
    }

    public override void Initialize()
    {
        base.Initialize();
        _skillTargetCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(ITarget target)
    {
        _skillController.OnCaptureEnter(target);
    }

    void OnExit(ITarget target)
    {
        _skillController.OnCaptureExit(target);
    }
}
