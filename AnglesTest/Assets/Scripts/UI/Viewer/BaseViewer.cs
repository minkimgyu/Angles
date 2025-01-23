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

        ArrowPointViewer,

        ChapterSelectViewer,
        StatViewer,
        SkinViewer,
        CloudSaveViewer,

        OkViewer, // 확인 Viewer
        OkCancelViewer, // 확인 취소 Viewer
    }

    public virtual void Initialize() { }
    public virtual void TurnOnViewer(bool show) { gameObject.SetActive(show); }
}
