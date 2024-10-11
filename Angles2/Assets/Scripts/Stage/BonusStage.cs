using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusStage : BaseStage
{
    [SerializeField] Transform _bonusPostion;

    Portal _portal;

    public override void Initialize(BaseStageController baseStageController, FactoryCollection factoryCollection)
    {
        base.Initialize(baseStageController, factoryCollection);

        _portal = GetComponentInChildren<Portal>();
        _portal.Initialize(_baseStageController.OnMoveToNextStageRequested);
    }

    public override void ActivePortal(Vector2 movePos)
    {
        _portal.Active(movePos);
    }

    public override void Exit()
    {
        base.Exit();
        _portal.Disable();
    }

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