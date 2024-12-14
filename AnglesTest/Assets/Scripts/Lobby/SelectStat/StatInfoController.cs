using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatInfoModel
{
    StatInfoViewer _statInfoViewer;

    public StatInfoModel(StatInfoViewer statInfoViewer)
    {
        _statInfoViewer = statInfoViewer;
    }

    public Sprite StatSprite
    {
        set { _statInfoViewer.ChangeStatImage(value); }
    }

    public string Name
    {
        set { _statInfoViewer.ChangeStatName(value); }
    }

    public int Level
    {
        set 
        {
            _statInfoViewer.ChangeStatLevel(value); 
        }
    }

    public int Cost
    {
        set 
        {
            if (value == 0)
            {
                _statInfoViewer.ActivateCost(false);
            }
            else
            {
                _statInfoViewer.ActivateCost(true);
            }

            _statInfoViewer.ChangeStatCost(value); 
        }
    }

    public string Description
    {
        set 
        { 
            if(value == string.Empty)
            {
                _statInfoViewer.ActivateDescription(false);
            }
            else
            {  
                _statInfoViewer.ActivateDescription(true);
            }

            _statInfoViewer.ChangeStatDescription(value); 
        }
    }
}

public class StatInfoController
{
    StatInfoModel _statInfoModel;

    public StatInfoController(StatInfoModel statInfoModel)
    {
        _statInfoModel = statInfoModel;
    }

    public void UpdateStat(int level, int cost, string description)
    {
        _statInfoModel.Level = level;
        _statInfoModel.Cost = cost;
        _statInfoModel.Description = description;
    }

    public void UpdateStat(Sprite statSprite, string name, int level, int cost, string description)
    {
        _statInfoModel.StatSprite = statSprite;
        _statInfoModel.Name = name;
        _statInfoModel.Level = level;
        _statInfoModel.Cost = cost;
        _statInfoModel.Description = description;
    }
}
