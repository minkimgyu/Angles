using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SkinInfoModel
{
    public enum State
    {
        Lock, // ������ ����
        UnSelected, // ��� ������ ����
        Selected // �� ����
    }

    SkinInfoViewer _skinInfoViewer;

    public SkinInfoModel(SkinInfoViewer skinInfoViewer)
    {
        _skinInfoViewer = skinInfoViewer;
    }

    public string UpgradeBtnTxt
    {
        set { _skinInfoViewer.ChangeUpgradeBtnText(value); }
    }

    public bool NowSelect
    {
        set
        {
            string equipped = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Equipped);
            string equip = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Equip);

            if (value == true) _skinInfoViewer.ChangeUpgradeBtnText(equipped);
            else _skinInfoViewer.ChangeUpgradeBtnText(equip);
        }
    }

    public bool NowUnlock
    {
        set 
        {
            string buy = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Buy);
            string equip = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Equip);

            if (value == false) _skinInfoViewer.ChangeUpgradeBtnText(buy);
            else _skinInfoViewer.ChangeUpgradeBtnText(equip);
            _skinInfoViewer.ActivateLockImg(!value);
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
        Buy, // ������ ����
        UnSelected, // ��� ������ ����
        Selected // �� ����
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
        _skinInfoModel.NowUnlock = true;
        _skinInfoModel.Cost = 0;
    }

    // dk
    public void PickSkin(Sprite statSprite, bool nowUnlock, bool nowSelect, string name, int cost, string description)
    {
        _skinInfoModel.StatSprite = statSprite;
        _skinInfoModel.Name = name;
        _skinInfoModel.Description = description;

        _skinInfoModel.NowUnlock = nowUnlock;
        if (!nowUnlock)
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
