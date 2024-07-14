using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : BaseLifeData
{
    public float _moveSpeed;

    public float _dashSpeed;
    public float _dashDuration;

    public float _shootSpeed;
    public float _shootDuration;

    public float _minJoystickLength;

    public int _dashCount;

    public int _dashConsumeCount;
    public float _dashRestoreDuration;

    public float _shrinkScale;
    public float _normalScale;

    public PlayerData(float maxHp, ITarget.Type targetType,
        float moveSpeed, float dashSpeed, float dashDuration, 
        float shootSpeed, float shootDuration,
        float minJoystickLength, int maxDashCount, 
        int dashConsumeCount, float dashRestoreDuration,

        float shrinkScale, float normalScale) : base(maxHp, targetType)
    {
        _moveSpeed = moveSpeed;
        _dashSpeed = dashSpeed;
        _dashDuration = dashDuration;

        _shootSpeed = shootSpeed;
        _shootDuration = shootDuration;

        _minJoystickLength = minJoystickLength;
        _dashCount = maxDashCount;

        _dashConsumeCount = dashConsumeCount;
        _dashRestoreDuration = dashRestoreDuration;

        _shrinkScale = shrinkScale;
        _normalScale = normalScale;
    }
}

public class PlayerCreater : LifeCreater<PlayerData>
{
    public override BaseLife Create()
    {
        BaseLife life = Object.Instantiate(_prefab);
        life.ResetData(_data);
        life.Initialize();

        return life;
    }
}
