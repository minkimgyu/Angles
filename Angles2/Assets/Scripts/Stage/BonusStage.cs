using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusStage : BaseStage
{
    [SerializeField] Transform _bonusPostion;

    public override void Spawn(int totalStageCount, int currentStageCount, IFactory factory)
    {
        _events.OnStageClearRequested?.Invoke();

        IInteractable interactableObject = factory.Create(IInteractable.Name.Shop);
        _spawnedObjects.Add(interactableObject.ReturnGameObject());

        interactableObject.ResetPosition(_bonusPostion.position);
        interactableObject.AddCommand(_events.CommandCollection.RecreatableCardsCommand);
    }
}