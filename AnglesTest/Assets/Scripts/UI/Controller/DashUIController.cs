using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashModel
{
    DashCountViewer _dashViewer;

    public DashModel(DashCountViewer dashViewer)
    {
        _dashViewer = dashViewer;
    }

    float _ratio;
    public float Ratio
    {
        get => _ratio;
        set
        {
            _ratio = value;
            _dashViewer.UpdateDashRatio(_ratio);
        }
    }
}

public class DashUIController : MonoBehaviour
{
    [SerializeField] RectTransform _dashViewerParent;
    List<DashModel> _dashModels;
    const int _maxDashCount = 3;

    public void Initialize(BaseFactory viewerFactory)
    {
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.State.OnDashRatioChange, new ChangeRatioCommand(UpdateViewer));
        _dashModels = new List<DashModel>();

        for (int i = 0; i < _maxDashCount; i++)
        {
            DashCountViewer viewer = (DashCountViewer)viewerFactory.Create(BaseViewer.Name.DashViewer);
            viewer.transform.SetParent(_dashViewerParent);
            _dashModels.Add(new DashModel(viewer));
        }
    }

    public void UpdateViewer(float fillRatio)
    {
        float totalRatio = fillRatio;

        for (int i = 0; i < _dashModels.Count; i++)
        {
            float ratio = Mathf.Clamp(totalRatio, 0, 1);
            _dashModels[i].Ratio = ratio;
            totalRatio -= ratio;
        }
    }
}
