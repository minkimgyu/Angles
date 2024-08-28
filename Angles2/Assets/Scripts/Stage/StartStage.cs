using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartStage : BaseStage
{
    [SerializeField] Transform _bonusPostion;

    public override void Spawn(int totalStageCount, int currentStageCount, IFactory factory)
    {
        _events.OnStageClearRequested?.Invoke();

        BaseLife player = factory.Create(BaseLife.Name.Player);
        player.transform.position = _entryPoint.position;

        IFollowable followable = player.GetComponent<IFollowable>();
        if (followable == null) return;

        Target = followable;

        ISkillUser skillUser = player.GetComponent<ISkillUser>();
        if (skillUser != null) _events.CommandCollection.AddSkillUserCommand.Execute(skillUser);
        _events.CommandCollection.AddCameraTrackerCommand.Execute(followable);

        BaseViewer hpViewer = factory.Create(BaseViewer.Name.HpViewer);
        BaseViewer directionViewer = factory.Create(BaseViewer.Name.DirectionViewer);

        hpViewer.SetFollower(followable);

        player.AddObserverEvent(
            _events.ObserberEventCollection.OnGameOverRequested, 
            _events.ObserberEventCollection.OnDachRatioChangeRequested, 
            _events.ObserberEventCollection.OnChargeRatioChangeRequested, 
            _events.ObserberEventCollection.OnAddSkillRequested,
            _events.ObserberEventCollection.OnRemoveSkillRequested,
            hpViewer.UpdateViewer,
            directionViewer.TurnOnViewer,
            directionViewer.UpdateViewer
        );

        IInteractable interactableObject = factory.Create(IInteractable.Name.CardTable);
        _spawnedObjects.Add(interactableObject.ReturnGameObject());
        interactableObject.ResetPosition(_bonusPostion.position);
        interactableObject.AddCommand(_events.CommandCollection.CreateCardsCommand);
    }
}