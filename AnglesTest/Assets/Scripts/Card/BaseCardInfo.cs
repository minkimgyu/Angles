using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// 여기에 카드 코스트 추가 --> 케이스에 따라서 cost를 뽑아서 사용하거나 하지 않음
public struct CardInfoData
{
    public CardInfoData(string name, string description, int cost)
    {
        _name = name;
        _description = description;
        _cost = cost;
    }

    [JsonProperty] private string _name;
    [JsonIgnore] public string Name { get { return _name; } }

    [JsonProperty] private string _description;
    [JsonIgnore] public string Description { get { return _description; } }

    [JsonProperty] private int _cost;
    [JsonIgnore] public int Cost { get { return _cost; } }
}