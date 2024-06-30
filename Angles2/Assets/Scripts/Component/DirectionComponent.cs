using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionComponent : MonoBehaviour
{
    [SerializeField] GameObject _directionSprite;

    public void OnOffDirectionSprite(bool show)
    {
        _directionSprite.SetActive(show);
    }
}
