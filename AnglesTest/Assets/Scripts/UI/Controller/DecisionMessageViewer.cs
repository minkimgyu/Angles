using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DecisionMessageViewer : MonoBehaviour
{
    [SerializeField] TMP_Text _infoTxt;
    [SerializeField] TMP_Text _yesTxt;
    [SerializeField] TMP_Text _noTxt;

    [SerializeField] GameObject _content;

    [SerializeField] Button _okBtn;
    [SerializeField] Button _cancelBtn;

    Action OnOkRequested;
    Action OnCancelRequested;

    public void Activate(bool on) => _content.SetActive(on);

    public void UpdateInfo(string state)
    {
        _infoTxt.text = state;
    }

    public void Initialize(Action OnOkRequested, Action OnCancelRequested)
    {
        _okBtn.onClick.AddListener(() => { OnOkRequested?.Invoke(); });
        _cancelBtn.onClick.AddListener(() => { OnCancelRequested?.Invoke(); });

        this.OnOkRequested = OnOkRequested;
        this.OnCancelRequested = OnCancelRequested;
        Activate(false);

        _yesTxt.text = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Yes);
        _noTxt.text = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.No);
    }
}
