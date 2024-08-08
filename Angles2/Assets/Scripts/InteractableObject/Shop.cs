using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : MonoBehaviour, IInteractable
{
    OutlineComponent _outlineComponent;
    Command<int, int, List<SkillUpgradeData>> RecreatableCardsCommand;
    bool _isActive;

    int _cardCount;
    int _recreateCount;

    public void Initialize(ShopData data)
    {
        _cardCount = data._cardCount;
        _recreateCount = data._recreateCount;

        _isActive = true;
        _outlineComponent = GetComponentInChildren<OutlineComponent>();
        _outlineComponent.Initialize();
    }

    public void AddCommand(Command<int, int, List<SkillUpgradeData>> RecreatableCardsCommand)
    {
        this.RecreatableCardsCommand = RecreatableCardsCommand;
    }

    public void OnInteractEnter(IInteracter interacter)
    {
        if (_isActive == false) return;

        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnInteract);
    }

    public void OnInteract(IInteracter interacter)
    {
        if (_isActive == false) return;

        _isActive = false;
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnDisabled);

        List<SkillUpgradeData> upgradeDatas = interacter.ReturnSkillUpgradeDatas();
        if (upgradeDatas.Count == 0) return;
        RecreatableCardsCommand.Execute(_cardCount, _recreateCount, upgradeDatas);
    }

    public void OnInteractExit(IInteracter interacter)
    {
        if (_isActive == false) return;

        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
    }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}