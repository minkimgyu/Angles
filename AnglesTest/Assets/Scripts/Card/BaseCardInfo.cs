using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���⿡ ī�� �ڽ�Ʈ �߰� --> ���̽��� ���� cost�� �̾Ƽ� ����ϰų� ���� ����
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