using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointViewer : BaseViewer
{
    RectTransform _arrowRectTransform;

    public override void Initialize()
    {
        _arrowRectTransform = GetComponent<RectTransform>();
    }

    public void UpdatePosition(Vector2 pos, Vector2 direction)
    {
        // ȭ��ǥ UI ��ġ �� ȸ�� ����
        _arrowRectTransform.anchoredPosition = pos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _arrowRectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
