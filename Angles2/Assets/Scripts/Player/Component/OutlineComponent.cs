using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineComponent : MonoBehaviour
{
    public enum Condition
    {
        OnIdle,
        OnDash,
        OnInvincible,
        OnGroggy,
    }

    [SerializeField] OutlineColorDictionary _outlineColorDictionary;
    SpriteRenderer _spriteRenderer;

    public void Initialize()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _outlineColorDictionary.Add(Condition.OnIdle, _spriteRenderer.color);
    }

    public void OnOutlineChange(Condition condition)
    {
        if (_outlineColorDictionary.ContainsKey(condition) == false) return;
        _spriteRenderer.color = _outlineColorDictionary[condition];
    }
}
