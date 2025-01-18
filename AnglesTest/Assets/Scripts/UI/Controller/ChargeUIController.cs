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

    float _alpha;
    public float Alpha
    {
        get => _alpha;
        set
        {
            _alpha = value;
            _chargeViewer.UpdateChargeAlpha(_alpha);
        }
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
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.State.OnChargeRatioChange, new ChangeRatioCommand(UpdateChargeHandler));
        _chargeModel = new ChargeModel(_chargeViewer);
    }

    public void UpdateChargeHandler(float fillRatio, float handleAlpha)
    {
        _chargeModel.Ratio = fillRatio;
        _chargeModel.Alpha = handleAlpha;
    }
}
