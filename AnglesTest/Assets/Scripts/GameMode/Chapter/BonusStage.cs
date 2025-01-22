using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusStage : BaseStage
{
    [SerializeField] Transform _bonusPostion;

    Portal _portal;
    BaseStageController _baseStageController;

    public override void Initialize(BaseStageController baseStageController, AddressableHandler addressableHandler, InGameFactory inGameFactory)
    {
        base.Initialize(baseStageController, addressableHandler, inGameFactory);

        _baseStageController = baseStageController;
        _portal = GetComponentInChildren<Portal>();
        _portal.Initialize(_baseStageController.OnMoveToNextStageRequested);
    }

    public override void ActivePortal(Vector2 movePos = default)
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
        _baseStageController.OnStageClearRequested();

        IInteractable interactableObject = _inGameFactory.GetFactory(InGameFactory.Type.Interactable).Create(IInteractable.Name.Shop);
        _spawnedObjects.Add(interactableObject.ReturnGameObject());
        interactableObject.ResetPosition(_bonusPostion.position);
    }
}