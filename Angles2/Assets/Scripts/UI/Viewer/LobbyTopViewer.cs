using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        }
    }
}

public class LobbyTopViewer : MonoBehaviour
{
    [SerializeField] TMP_Text _goldTxt;

    public void ChangeGoldCount(int gold)
    {
        _goldTxt.text = gold.ToString();
    }
}
