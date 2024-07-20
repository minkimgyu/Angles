using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardTable : MonoBehaviour, IInteractable
{
    Action<List<SkillUpgradeData>> CreateCards;

    public void OnInteract(List<SkillUpgradeData> containedSkillNames)
    {
        CreateCards?.Invoke(containedSkillNames);
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        CardController cardController = FindObjectOfType<CardController>();
        CreateCards = cardController.CreateCards;
    }

    public void OnInteractEnter() { }
    public void OnInteractEnter(IFollowable followable) { }
    public void OnInteractExit() { }
}
