using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// ���⿡ ī�� �ڽ�Ʈ �߰� --> ���̽��� ���� cost�� �̾Ƽ� ����ϰų� ���� ����
public struct CardInfoData
{
    public CardInfoData(int cost)
    {
        _cost = cost;
    }

    [JsonProperty] private int _cost;
    [JsonIgnore] public int Cost { get { return _cost; } }
}