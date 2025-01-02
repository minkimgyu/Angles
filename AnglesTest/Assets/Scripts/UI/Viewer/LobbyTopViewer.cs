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

    public void Initialize(Action OnClickSettingBtn) => _settingBtn.onClick.AddListener(() => { OnClickSettingBtn?.Invoke(); });

    public void ChangeGoldCount(int gold)
    {
        _goldTxt.text = gold.ToString(); // 여기서 골드 데이터 바꾸기
    }
}
