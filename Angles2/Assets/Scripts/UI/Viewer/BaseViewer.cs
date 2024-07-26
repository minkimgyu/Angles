using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseViewer : MonoBehaviour
{
    public enum Name
    {
        CardViewer,
        HpViewer,
        SkillViewer,
        DashViewer,
        DirectionViewer,
    }

    public virtual void Initialize() { }
    public virtual void Initialize(Sprite skillIcon) { }
    public virtual void Initialize(SKillCardData cardData, Action OnClick) { }

    public virtual void AddChildUI(Transform child) { }

    public virtual void SetFollower(IFollowable followTarget) { }

    public virtual void UpdateViewer(float skillCoolTimeRatio, int stackCount, bool showStackCount) { }
    public virtual void UpdateViewer(Vector3 pos, Vector2 direction) { }
    public virtual void UpdateViewer(float ratio) { }

    public virtual void OnOffViewer(bool show) { }
}
