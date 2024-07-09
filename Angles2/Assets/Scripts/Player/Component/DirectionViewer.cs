using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionViewer : MonoBehaviour
{
    [SerializeField] GameObject _directionSprite;

    public void Initialize()
    {
        OnOffDirectionSprite(false);
    }

    public void UpdatePosition(Vector3 pos, Vector2 direction)
    {
        transform.position = pos;
        transform.right = direction;
    }

    public void OnOffDirectionSprite(bool show)
    {
        _directionSprite.SetActive(show);
    }
}
