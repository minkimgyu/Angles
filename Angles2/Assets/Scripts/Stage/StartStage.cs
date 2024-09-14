using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartStage : BaseStage
{
    [SerializeField] Transform _bonusPostion;

    public override void Spawn(int totalStageCount, int currentStageCount)
    {
        _baseStageController.OnStageClearRequested();

        Player player = _factoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(BaseLife.Name.Player) as Player;
        Vector3 entryPos = ReturnEntryPosition();
        player.transform.position = entryPos;


        InputController inputManager = FindObjectOfType<InputController>();
        if(inputManager != null)
        {
            inputManager.AddEvent(InputController.Side.Left, InputController.Type.OnInputStart, player.OnLeftInputStart);
            inputManager.AddEvent(InputController.Side.Left, InputController.Type.OnInput, player.OnLeftInput);
            inputManager.AddEvent(InputController.Side.Left, InputController.Type.OnInputEnd, player.OnLeftInputEnd);

            inputManager.AddEvent(InputController.Side.Right, InputController.Type.OnInputStart, player.OnRightInputStart);
            inputManager.AddEvent(InputController.Side.Right, InputController.Type.OnInput, player.OnRightInput);
            inputManager.AddEvent(InputController.Side.Right, InputController.Type.OnInputEnd, player.OnRightInputEnd);

            inputManager.AddEvent(InputController.Side.Right, InputController.Type.OnDoubleTab, player.OnRightDoubleTab);
        }

        IFollowable followable = player.GetComponent<IFollowable>();
        if (followable != null) EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.AddFollableCamera, followable);

        Target = followable;

        BaseFactory viewerFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Viewer);

        BaseViewer hpViewer = viewerFactory.Create(BaseViewer.Name.HpViewer);
        hpViewer.Initialize();
        hpViewer.SetFollower(followable);
        player.AddObserverEvent(hpViewer.UpdateViewer);

        BaseViewer directionViewer = viewerFactory.Create(BaseViewer.Name.DirectionViewer);
        directionViewer.Initialize();
        directionViewer.SetFollower(followable);

        IInteractable interactableObject = _factoryCollection.ReturnFactory(FactoryCollection.Type.Interactable).Create(IInteractable.Name.CardTable);
        _spawnedObjects.Add(interactableObject.ReturnGameObject());
        interactableObject.ResetPosition(_bonusPostion.position);
    }
}