using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUIController : MonoBehaviour
{
    [SerializeField] RectTransform _dashViewerParent;
    List<BaseViewer> _viwers;
    const int _maxDashCount = 3;

    public void Initialize(System.Func<BaseViewer.Name, BaseViewer> SpawnViewer)
    {
        _viwers = new List<BaseViewer>();

        for (int i = 0; i < _maxDashCount; i++)
        {
            BaseViewer viewer = SpawnViewer?.Invoke(BaseViewer.Name.DashViewer);
            viewer.transform.SetParent(_dashViewerParent);
            _viwers.Add(viewer);
        }
    }

    public void UpdateViewer(float fillRatio)
    {
        float totalRatio = fillRatio;

        for (int i = 0; i < _viwers.Count; i++)
        {
            float ratio = Mathf.Clamp(totalRatio, 0, 1);
            _viwers[i].UpdateViewer(ratio);
            totalRatio -= ratio;
        }
    }
}
