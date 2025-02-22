using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StageResultViewer : BaseViewer
{
    [SerializeField] TMP_Text _stageResult;
    float _fadeDuration = 2;

    private void OnDestroy()
    {
        _stageResult.DOKill();
    }

    public override void TurnOnViewer(bool show)
    {
        _stageResult.DOKill();

        if (show == false)
        {
            _stageResult.color = new Color(_stageResult.color.r, _stageResult.color.g, _stageResult.color.b, 0);
        }
        else
        {
            _stageResult.color = new Color(_stageResult.color.r, _stageResult.color.g, _stageResult.color.b, 1);
        }
    }

    public void UpdateResultInfo(string info)
    {
        _stageResult.text = $"<{info}>";
        _stageResult.DOFade(1, _fadeDuration).OnComplete(() => { _stageResult.DOFade(0, _fadeDuration); });
    }
}
