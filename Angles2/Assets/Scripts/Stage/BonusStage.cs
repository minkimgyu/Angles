using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusStage : BaseStage
{
    [SerializeField] Transform _bonusPostion;

    public override void Spawn(StageSpawnData data)
    {
        OnClearRequested?.Invoke();

        IInteractable interactable = InteractableObjectFactory.Create(IInteractable.Name.CardTable);
        interactable.ResetPosition(_bonusPostion.position);
    }
}