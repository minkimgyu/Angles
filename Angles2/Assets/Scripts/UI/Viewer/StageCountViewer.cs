using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageCountViewer : BaseViewer
{
    [SerializeField] TMP_Text _stageCountTxt;

    public override void UpdateViewer(int currentStageCount) 
    {
        _stageCountTxt.text = currentStageCount.ToString();
    }

    public override void TurnOnViewer(bool show)
    {
        gameObject.SetActive(show);
    }
}
