using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUIController : MonoBehaviour
{
    [SerializeField] RectTransform _dashViewerParent;
    List<BaseViewer> _viwers;

    public void Initialize(int dashCount)
    {
        _viwers = new List<BaseViewer>();

        for (int i = 0; i < dashCount; i++)
        {
            BaseViewer viewer = ViewerFactory.Create(BaseViewer.Name.DashViewer);
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
