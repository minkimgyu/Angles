using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillViewer : BaseViewer
{
    [SerializeField] Image _coolTimeImage;
    [SerializeField] Image _skillIconImage;
    [SerializeField] TMP_Text _skillCountText;

    public override void Initialize(Sprite skillIcon)
    {
        _skillIconImage.sprite = skillIcon;
        _coolTimeImage.fillAmount = 0;
        _skillCountText.text = "";
    }

    public override void UpdateViewer(float skillCoolTimeRatio, int skillCount, bool showStackCount)
    {
        _coolTimeImage.fillAmount = skillCoolTimeRatio;
        if (showStackCount == false) return;

        _skillCountText.text = skillCount.ToString();
    }
}
