using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여기에 카드 코스트 추가 --> 케이스에 따라서 cost를 뽑아서 사용하거나 하지 않음
public class CardInfoData
{
    const float _costOffset = 0.3f;

    public CardInfoData(BaseSkill.Name iconName, string info, int cost)
    {
        _IconName = iconName;
        _info = info;
        _cost = cost;
    }

    private BaseSkill.Name _IconName;
    public BaseSkill.Name IconName { get { return _IconName; } }

    private string _info;
    public string Info { get { return _info; } }

    private int _cost;
    public int Cost { get { return _cost + Mathf.RoundToInt(Random.Range(_cost - _cost * _costOffset, _cost + _cost * _costOffset)); } }
}