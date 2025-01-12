using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LobbyTopModel
{
    LobbyTopViewer _lobbyViewer;

    public LobbyTopModel(LobbyTopViewer lobbyViewer)
    {
        _lobbyViewer = lobbyViewer;
    }

    string _adDuration;
    public string AdDuration
    {
        get { return _adDuration; }
        set
        {
            if (_adDuration == string.Empty)
            {
                _lobbyViewer.ChangeAdDuration();
                return;
            }

            string leftTimeTxt = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.ForLeftAd);
            string leftTxt = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Left);
            _adDuration = value;
            _lobbyViewer.ChangeAdDuration(leftTimeTxt, _adDuration, leftTxt);
        }
    }

    bool _activeAdBtn;
    public bool ActiveAdBtn
    {
        get { return _activeAdBtn; }
        set
        {
            _activeAdBtn = value;
            _lobbyViewer.ActiveAdBtn(_activeAdBtn);
        }
    }

    int _goldCount;
    public int GoldCount
    {
        get { return _goldCount; }
        set
        {
            _goldCount = value;
            _lobbyViewer.ChangeGoldCount(_goldCount);

            ISaveable saveable = ServiceLocater.ReturnSaveManager();
            saveable.ChangeCoinCount(_goldCount);
        }
    }
}

public class LobbyTopViewer : MonoBehaviour
{
    [SerializeField] TMP_Text _goldTxt;
    [SerializeField] Button _settingBtn;

    [SerializeField] Button _adBtn;
    [SerializeField] TMP_Text _adDurationTxt;

    public void Initialize(Action OnClickAdBtn, Action OnClickSettingBtn)
    {
        _adBtn.onClick.AddListener(() => { OnClickAdBtn?.Invoke(); });
        _settingBtn.onClick.AddListener(() => { OnClickSettingBtn?.Invoke(); });
    }

    public void ActiveAdBtn(bool active)
    {
        _adBtn.interactable = active;
    }

    public void ChangeGoldCount(int gold)
    {
        _goldTxt.text = gold.ToString(); // 여기서 골드 데이터 바꾸기
    }

    public void ChangeAdDuration(string frontTxt = "", string adDuration = "", string backTxt = "")
    {
        _adDurationTxt.text = $"{frontTxt}\n{adDuration} {backTxt}"; // 여기서 골드 데이터 바꾸기
    }
}
