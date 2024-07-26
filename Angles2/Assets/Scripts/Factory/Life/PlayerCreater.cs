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

    public List<BaseSkill.Name> _skillNames;

    public PlayerData(float maxHp, ITarget.Type targetType,
        float moveSpeed, float dashSpeed, float dashDuration, 
        float shootSpeed, float shootDuration,
        float minJoystickLength, int maxDashCount, 
        int dashConsumeCount, float dashRestoreDuration,

        float shrinkScale, float normalScale, List<BaseSkill.Name> skillNames) : base(maxHp, targetType)
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

        _skillNames = skillNames;
    }
}

public class PlayerCreater : LifeCreater
{
    public override BaseLife Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseLife life = obj.GetComponent<Player.Player>();
        if (life == null) return null;

        PlayerData playerData = Database.Instance.LifeDatas[BaseLife.Name.Player] as PlayerData;
        life.ResetData(playerData);
        life.Initialize();

        return life;
    }
}
