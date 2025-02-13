using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineComponent : MonoBehaviour
{
    public enum Condition
    {
        OnIdle,
        OnDash,

        OnImmunity,
        OnGroggy,
        OnInteract,

        OnEnabled,
        OnDisabled
    }

    [SerializeField] Dictionary<Condition, Color> _outlineColorDictionary;
    SpriteRenderer _spriteRenderer;

    public void Initialize()
    {
        _outlineColorDictionary = new Dictionary<Condition, Color>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _outlineColorDictionary.Add(Condition.OnIdle, _spriteRenderer.color);


        _outlineColorDictionary.Add(Condition.OnImmunity, new Color(20 / 255f, 217 / 255f, 148 / 255f));
        _outlineColorDictionary.Add(Condition.OnDash, new Color(20 / 255f, 217 / 255f, 148 / 255f));


        _outlineColorDictionary.Add(Condition.OnInteract, new Color(0f, 255f / 255f, 255f / 255f));
        _outlineColorDictionary.Add(Condition.OnEnabled, new Color(0f, 255f / 255f, 255f / 255f));
        _outlineColorDictionary.Add(Condition.OnDisabled, new Color(255 / 255f, 73 / 255f, 69 / 255f));
    }

    public void OnOutlineChange(Condition condition)
    {
        if (_outlineColorDictionary.ContainsKey(condition) == false) return;
        _spriteRenderer.color = _outlineColorDictionary[condition];
    }
}
