using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SkinInfoModel
{
    public enum State
    {
        Lock, // 구매한 상태
        UnSelected, // 잠금 해제한 상태
        Selected // 고른 상태
    }

    SkinInfoViewer _skinInfoViewer;
    Dictionary<State, string> _btnInfos;

    public SkinInfoModel(SkinInfoViewer skinInfoViewer, Dictionary<State, string> btnInfos)
    {
        _skinInfoViewer = skinInfoViewer;
        _btnInfos = btnInfos;
    }

    public string UpgradeBtnTxt
    {
        set { _skinInfoViewer.ChangeUpgradeBtnText(value); }
    }

    public bool NowSelect
    {
        set
        {
            if (value == true) _skinInfoViewer.ChangeUpgradeBtnText(_btnInfos[State.Selected]);
            else _skinInfoViewer.ChangeUpgradeBtnText(_btnInfos[State.UnSelected]);
        }
    }

    public bool NowLock
    {
        set 
        {
            if(value == true) _skinInfoViewer.ChangeUpgradeBtnText(_btnInfos[State.Lock]);
            else _skinInfoViewer.ChangeUpgradeBtnText(_btnInfos[State.UnSelected]);
            _skinInfoViewer.ActivateLockImg(value);
        }
    }

    public Sprite StatSprite
    {
        set { _skinInfoViewer.ChangeStatImage(value); }
    }

    public string Name
    {
        set { _skinInfoViewer.ChangeStatName(value); }
    }

    public int Cost
    {
        set
        {
            if (value == 0)
            {
                _skinInfoViewer.ActivateCost(false);
            }
            else
            {
                _skinInfoViewer.ActivateCost(true);
            }

            _skinInfoViewer.ChangeStatCost(value);
        }
    }

    public string Description
    {
        set
        {
            if (value == string.Empty)
            {
                _skinInfoViewer.ActivateDescription(false);
            }
            else
            {
                _skinInfoViewer.ActivateDescription(true);
            }

            _skinInfoViewer.ChangeStatDescription(value);
        }
    }
}

public class SkinInfoController
{
    public enum State
    {
        Buy, // 구매한 상태
        UnSelected, // 잠금 해제한 상태
        Selected // 고른 상태
    }

    SkinInfoModel _skinInfoModel;

    public SkinInfoController(SkinInfoModel statInfoModel)
    {
        _skinInfoModel = statInfoModel;
    }

    public void SelectSkin()
    {
        _skinInfoModel.NowSelect = true;
    }

    public void BuySkin()
    {
        _skinInfoModel.NowLock = false;
        _skinInfoModel.Cost = 0;
    }

    // dk
    public void PickSkin(Sprite statSprite, bool nowLock, bool nowSelect, string name, int cost, string description)
    {
        _skinInfoModel.StatSprite = statSprite;
        _skinInfoModel.Name = name;
        _skinInfoModel.Description = description;

        _skinInfoModel.NowLock = nowLock;
        if (nowLock)
        {
            _skinInfoModel.Cost = cost;
        }
        else
        {
            _skinInfoModel.Cost = 0;
            if (nowSelect) _skinInfoModel.NowSelect = true;
            else _skinInfoModel.NowSelect = false;
        }
    }
}
