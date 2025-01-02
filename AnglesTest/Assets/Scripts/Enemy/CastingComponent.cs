using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingComponent : MonoBehaviour
{
    [SerializeField] Transform _castingTransform;
    Vector3 _endScale;

    private void Start()
    {
        _endScale = _castingTransform.localScale;
        _castingTransform.localScale = Vector3.zero;
    }

    public void CastSkill(float duration)
    {
        Vector3 startScale = Vector3.zero;

        _castingTransform.localScale = startScale;
        _castingTransform.DOScale(_endScale, duration).OnComplete(() => { _castingTransform.localScale = Vector3.zero; });
    }

    private void OnDestroy()
    {
        _castingTransform.DOKill();
    }
}
