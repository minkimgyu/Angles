using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 그냥 Viewer로 구현

public class PopUpViewer : MonoBehaviour
{
    public enum State
    {
        ShortOfGold,
        NowMaxUpgrade
    }

    Dictionary<State, string> _alarmInfos;
    [SerializeField] GameObject _content;
    [SerializeField] TMP_Text _infoText;
    [SerializeField] Button _confirmBtn;

    public void Initialize(Dictionary<State, string> alarmInfo)
    {
        _alarmInfos = alarmInfo;
        _confirmBtn.onClick.AddListener(() => Activate(false));
        Activate(false);
    }

    void Activate(bool on) => _content.SetActive(on);

    public void UpdateInfo(State state)
    {
        if (_alarmInfos.ContainsKey(state) == false) return;

        Activate(true);
        _infoText.text = _alarmInfos[state];
    }
}
