using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// ���⿡ ī�� �ڽ�Ʈ �߰� --> ���̽��� ���� cost�� �̾Ƽ� ����ϰų� ���� ����
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