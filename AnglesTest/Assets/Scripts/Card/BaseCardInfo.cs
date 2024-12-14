using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여기에 카드 코스트 추가 --> 케이스에 따라서 cost를 뽑아서 사용하거나 하지 않음
public class CardInfoData
{
    public CardInfoData(BaseSkill.Name key, string name, string info, int cost)
    {
        _key = key;
        _name = name;
        _info = info;
        _cost = cost;
    }

    private BaseSkill.Name _key;
    public BaseSkill.Name Key { get { return _key; } }

    private string _name;
    public string Name { get { return _name; } }

    private string _info;
    public string Info { get { return _info; } }

    private int _cost;
    public int Cost { get { return _cost; } }
}