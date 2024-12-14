using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingComponent : MonoBehaviour
{
    [SerializeField] Transform _castingTransform;

    private void Start()
    {
        _castingTransform.localScale = Vector3.zero;
    }

    public void CastSkill(float duration)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        _castingTransform.localScale = startScale;
        _castingTransform.DOScale(endScale, duration).OnComplete(() => { _castingTransform.localScale = Vector3.zero; });
    }

    private void OnDestroy()
    {
        _castingTransform.DOKill();
    }
}
