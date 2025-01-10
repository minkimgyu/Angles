using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AdViewer : BaseViewer
{
    [SerializeField] TMP_Text _reviveTxt;
    [SerializeField] TMP_Text _yesTxt;
    [SerializeField] TMP_Text _noTxt;

    [SerializeField] Button _reviveBtn;
    [SerializeField] Button _cancelBtn;

    Action OnReviveRequested;
    Action OnCancelRequested;

    public void Initialize(Action OnReviveRequested, Action OnCancelRequested, string reviveTxt)
    {
        _reviveBtn.onClick.AddListener(() => { OnReviveRequested?.Invoke(); });
        _cancelBtn.onClick.AddListener(() => { OnCancelRequested?.Invoke(); });

        this.OnReviveRequested = OnReviveRequested;
        this.OnCancelRequested = OnCancelRequested;
        TurnOnViewer(false);

        _reviveTxt.text = reviveTxt;
        _yesTxt.text = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Yes);
        _noTxt.text = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.No);
    }
}
