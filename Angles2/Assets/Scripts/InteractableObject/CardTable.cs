using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardTable : MonoBehaviour, IInteractable
{
    Action<List<SkillUpgradeData>> CreateCards;
    OutlineComponent _outlineComponent;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        CardController cardController = FindObjectOfType<CardController>();
        CreateCards = cardController.CreateCards;

        _outlineComponent = GetComponentInChildren<OutlineComponent>();
        _outlineComponent.Initialize();
    }

    public void OnInteractEnter(InteractEnterData data)
    {
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnInteract);
    }

    public void OnInteract(InteractData data)
    {
        List<SkillUpgradeData> upgradeDatas = data.ReturnSkillUpgradeDatas?.Invoke();

        if (upgradeDatas.Count == 0) return;
        CreateCards?.Invoke(upgradeDatas);
    }

    public void OnInteractExit(InteractExitData data)
    {
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
    }
}
