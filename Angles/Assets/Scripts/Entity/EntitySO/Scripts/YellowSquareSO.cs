using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "YellowSquareSO", menuName = "Scriptable Object/EntitySO/YellowSquareSO")]
public class YellowSquareSO : BaseEntitySO
{
    [SerializeField]
    bool immortality;

    [SerializeField]
    BuffFloat hp;

    [SerializeField]
    BuffFloat speed;

    [SerializeField]
    BuffFloat stunTime;

    [SerializeField]
    BuffFloat weight;

    [SerializeField]
    BuffFloat mass;

    [SerializeField]
    BuffFloat drag;

    [SerializeField]
    string dieEffectName;

    [SerializeField]
    string[] skillNames;

    [SerializeField]
    BuffInt score;

    [SerializeField]
    BuffFloat skillUseDistance;

    [SerializeField]
    BuffFloat skillUseOffsetDistance;

    [SerializeField]
    BuffFloat followDistance;

    [SerializeField]
    BuffFloat followOffsetDistance;

    public override Entity Create()
    {
        YellowSquareEnemy enemy = ObjectPooler.SpawnFromPool<YellowSquareEnemy>(name);

        enemy.Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score,
            skillUseDistance, skillUseOffsetDistance, followDistance, followOffsetDistance);

        // 다음과 같이 초기화

        return enemy;
    }
}
