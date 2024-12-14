using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartStage : BaseStage
{
    [SerializeField] Transform _bonusPostion;
    Portal _portal;

    PlayerSpawner _playerSpawner;

    public override void Initialize(BaseStageController baseStageController, CoreSystem coreSystem)
    {
        base.Initialize(baseStageController, coreSystem);

        _portal = GetComponentInChildren<Portal>();
        _portal.Initialize(_baseStageController.OnMoveToNextStageRequested);

        InputController inputController = FindObjectOfType<InputController>();
        _playerSpawner = new PlayerSpawner(
            coreSystem.FactoryCollection,
            inputController,
            coreSystem.AddressableHandler.SkinIconAsset,
            coreSystem.Database.SkinDatas,
            coreSystem.Database.SkinModifiers,
            coreSystem.Database.StatDatas,
            coreSystem.Database.StatModifiers
        );
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
        _baseStageController.OnStageClearRequested();

        Vector3 entryPos = ReturnEntryPosition();
        _playerSpawner.Spawn(entryPos);

        IInteractable interactableObject = _coreSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Interactable).Create(IInteractable.Name.CardTable);
        _spawnedObjects.Add(interactableObject.ReturnGameObject());
        interactableObject.ResetPosition(_bonusPostion.position);
    }
}