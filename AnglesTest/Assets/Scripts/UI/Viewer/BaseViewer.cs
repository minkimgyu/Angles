using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseViewer : MonoBehaviour
{
    public enum Name
    {
        CardViewer,
        CostCardViewer,

        HpViewer,
        SkillViewer,
        DashViewer,
        DirectionViewer,

        ChapterSelectViewer,
        StatViewer,
        SkinViewer
    }

    public virtual void Initialize() { }
    public virtual void TurnOnViewer(bool show) { gameObject.SetActive(show); }
}
