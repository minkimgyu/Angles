using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageCountViewer : BaseViewer
{
    [SerializeField] TMP_Text _stageCountTxt;

    public void UpdateStaegCount(string currentStageCount) 
    {
        _stageCountTxt.text = currentStageCount;
    }

    public override void TurnOnViewer(bool show)
    {
        gameObject.SetActive(show);
    }
}
