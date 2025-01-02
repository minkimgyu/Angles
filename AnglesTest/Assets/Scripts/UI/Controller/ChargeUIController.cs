using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeModel
{
    ChargeViewer _chargeViewer;

    public ChargeModel(ChargeViewer dashViewer)
    {
        _chargeViewer = dashViewer;
    }

    float _ratio;
    public float Ratio
    {
        get => _ratio;
        set
        {
            _ratio = value;
            _chargeViewer.UpdateChargeRatio(_ratio);
        }
    }
}

public class ChargeUIController : MonoBehaviour
{
    [SerializeField] ChargeViewer _chargeViewer;
    ChargeModel _chargeModel;

    public void Initialize()
    {
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.State.OnChargeRatioChange, new ChangeRatioCommand(UpdateViewer));
        _chargeModel = new ChargeModel(_chargeViewer);
    }

    public void UpdateViewer(float fillRatio)
    {
        _chargeModel.Ratio = fillRatio;
    }
}
