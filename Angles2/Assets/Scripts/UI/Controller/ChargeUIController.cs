using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeModel
{
    BaseViewer _chargeViewer;

    public ChargeModel(BaseViewer dashViewer)
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
            _chargeViewer.UpdateViewer(_ratio);
        }
    }
}

public class ChargeUIController : MonoBehaviour
{
    [SerializeField] BaseViewer _chargeViewer;
    ChargeModel _chargeModel;

    public void Initialize()
    {
        ObserverEventBus.Register(ObserverEventBus.State.OnChargeRatioChange, new ChangeRatioCommand(UpdateViewer));
        _chargeModel = new ChargeModel(_chargeViewer);
    }

    public void UpdateViewer(float fillRatio)
    {
        _chargeModel.Ratio = fillRatio;
    }
}
