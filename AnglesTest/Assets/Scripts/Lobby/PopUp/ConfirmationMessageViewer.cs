using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationMessageViewer : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] TMP_Text _infoText;

    [SerializeField] Button _confirmBtn;
    [SerializeField] TMP_Text _confirmText;

    public void Initialize()
    {
        _confirmText.text = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Yes);
        _confirmBtn.onClick.AddListener(() => Activate(false));
        Activate(false);
    }

    public void Activate(bool on) => _content.SetActive(on);

    public void UpdateInfo(string state)
    {
        _infoText.text = state;
    }
}
