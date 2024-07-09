using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUIController : MonoBehaviour
{
    [SerializeField] DashCountViewer _dashViewerPrefab;
    [SerializeField] RectTransform _dashViewerParent;

    List<DashCountViewer> _viwers;

    public void Initialize(int dashCount)
    {
        _viwers = new List<DashCountViewer>();

        for (int i = 0; i < dashCount; i++)
        {
            DashCountViewer viewer = Instantiate(_dashViewerPrefab, _dashViewerParent);
            _viwers.Add(viewer);
        }
    }

    public void UpdateViewer(float fillRatio)
    {
        float totalRatio = fillRatio;

        for (int i = 0; i < _viwers.Count; i++)
        {
            float ratio = Mathf.Clamp(totalRatio, 0, 1);
            _viwers[i].FillViewer(ratio);
            totalRatio -= ratio;
        }
    }
}
