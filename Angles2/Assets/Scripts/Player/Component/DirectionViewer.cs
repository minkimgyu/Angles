using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionViewer : BaseViewer
{
    [SerializeField] GameObject _directionSprite;

    public override void Initialize() 
    {
        TurnOnViewer(false);
    }
    public override void TurnOnViewer(bool show) 
    {
        _directionSprite.SetActive(show);
    }

    public override void UpdateViewer(Vector3 pos, Vector2 direction) 
    {
        transform.position = pos;
        transform.right = direction;
    }
}
