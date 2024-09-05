using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusStage : BaseStage
{
    [SerializeField] Transform _bonusPostion;

    public override void Spawn(int totalStageCount, int currentStageCount)
    {
        //_events.OnStageClearRequested?.Invoke();
        _baseStageController.OnStageClearRequested();

        IInteractable interactableObject = _factoryCollection.ReturnFactory(FactoryCollection.Type.Interactable).Create(IInteractable.Name.Shop);
        _spawnedObjects.Add(interactableObject.ReturnGameObject());
        interactableObject.ResetPosition(_bonusPostion.position);
        //interactableObject.AddCreateCardsEvent(_events.RecreatableCardsCommand);
    }
}