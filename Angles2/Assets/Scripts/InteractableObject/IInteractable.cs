using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteractable
{
    public enum Name
    {
        Coin,
        CardTable
    }

    void OnInteractEnter();
    void OnInteractEnter(IFollowable followable);

    void OnInteract(List<SkillUpgradeData> skillDatas);
    void OnInteractExit();
}
