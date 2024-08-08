using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteractable
{
    enum Name
    {
        Coin,
        SkillBubble,
        CardTable,
        Shop,
        Portal
    }

    virtual void Initialize(CardTableData data) { }
    virtual void Initialize(ShopData data) { }
    virtual void Initialize(SkillBubbleData data) { }
    virtual void Initialize(CoinData data) { }

    virtual void AddCommand(Command<int, List<SkillUpgradeData>> CreateCardsCommand) { }
    virtual void AddCommand(Command<int, int, List<SkillUpgradeData>> RecreatableCardsCommand) { }
    virtual void AddCommand(Command<int> AddCoint) { }

    // Create 함수에서 pos나 trasform을 받을 수 있게 개량해보자 --> 취소
    GameObject ReturnGameObject() { return default; }

    void ResetPosition(Vector3 pos);

    void OnInteractEnter(IInteracter interacter);
    void OnInteract(IInteracter interacter);
    void OnInteractExit(IInteracter interacter);
}
