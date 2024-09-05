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
        Portal,
        Heart
    }

    virtual void Initialize(CardTableData data) { }
    virtual void Initialize(ShopData data) { }
    virtual void Initialize(SkillBubbleData data) { }
    virtual void Initialize(CoinData data) { }

    virtual void AddAddCointEvent(Action<int> AddCoint) { }
    virtual void AddCreateCardsEvent(Action<int, List<SkillUpgradeData>> CreateCards) { }
    virtual void AddCreateCardsEvent(Action<int, int, List<SkillUpgradeData>> CreateCards) { }

    GameObject ReturnGameObject() { return default; }

    void ResetPosition(Vector3 pos);

    void OnInteractEnter(IInteracter interacter);
    void OnInteract(IInteracter interacter);
    void OnInteractExit(IInteracter interacter);
}
