using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfoData
{
    public CardInfoData(BaseSkill.Name iconName, string info)
    {
        _IconName = iconName;
        _info = info;
    }

    [SerializeField]
    private BaseSkill.Name _IconName;
    public BaseSkill.Name IconName { get { return _IconName; } }

    [SerializeField]
    private string _info;
    public string Info { get { return _info; } }
}