using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartStage : BaseStage
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
        _baseStageController.OnStageClearRequested();

        Player player = _factoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(BaseLife.Name.Player) as Player;
        Vector3 entryPos = ReturnEntryPosition();
        player.transform.position = entryPos;

        EventBusManager.Instance.SubEventBus.Register(SubEventBus.State.SetPlayerInvincible, new SetPlayerInvincibleCommand(() => player.SetInvincible()));

        InputController inputController = FindObjectOfType<InputController>();
        inputController.Initialize();

        if (inputController != null)
        {
            inputController.AddEvent(InputController.Side.Left, InputController.Type.OnInputStart, player.OnLeftInputStart);
            inputController.AddEvent(InputController.Side.Left, InputController.Type.OnInput, player.OnLeftInput);
            inputController.AddEvent(InputController.Side.Left, InputController.Type.OnInputEnd, player.OnLeftInputEnd);

            inputController.AddEvent(InputController.Side.Right, InputController.Type.OnInputStart, player.OnRightInputStart);
            inputController.AddEvent(InputController.Side.Right, InputController.Type.OnInput, player.OnRightInput);
            inputController.AddEvent(InputController.Side.Right, InputController.Type.OnInputEnd, player.OnRightInputEnd);

            inputController.AddEvent(InputController.Side.Right, InputController.Type.OnDoubleTab, player.OnRightDoubleTab);
        }

        IFollowable followable = player.GetComponent<IFollowable>();
        if (followable != null) EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.AddFollableCamera, followable);

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