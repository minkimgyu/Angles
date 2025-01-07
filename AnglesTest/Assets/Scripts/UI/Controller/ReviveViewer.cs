using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ReviveViewer : BaseViewer
{
    [SerializeField] TMP_Text _reviveTxt;
    [SerializeField] Button _reviveBtn;
    [SerializeField] Button _cancelBtn;

    Action OnReviveRequested;
    Action OnCancelRequested;

    public void Initialize(Action OnReviveRequested, Action OnCancelRequested)
    {
        this.OnReviveRequested = OnReviveRequested;
        this.OnCancelRequested = OnCancelRequested;
        TurnOnViewer(false);

        _reviveTxt.text = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.TabToRevive);
    }
}
