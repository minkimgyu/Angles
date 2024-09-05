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

        BaseLife player = _factoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(BaseLife.Name.Player);
        Vector3 entryPos = ReturnEntryPosition();
        player.transform.position = entryPos;


        IFollowable followable = player.GetComponent<IFollowable>();
        if (followable != null) SubEventBus.Publish(SubEventBus.State.RegisterFollableCamera, followable);

        Target = followable;

        ISkillUser skillUser = player.GetComponent<ISkillUser>();
        if (skillUser != null) SubEventBus.Publish(SubEventBus.State.RegisterSkillUpgradeable, skillUser);

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