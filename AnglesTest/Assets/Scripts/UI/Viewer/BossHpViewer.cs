using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpViewer : HpViewer
{
    public override void TurnOnViewer(bool show)
    {
        gameObject.SetActive(show);
    }
}
