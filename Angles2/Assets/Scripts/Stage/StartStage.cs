using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage : BaseStage
{
    [SerializeField] List<Transform> _bonusPostions;

    public override void Spawn(List<IInteractable.Name> names)
    {
        OnClearRequested?.Invoke();

        int randomRange = Random.Range(0, _bonusPostions.Count);
        for (int i = 0; i < randomRange; i++)
        {
            IInteractable.Name randomName = names[Random.Range(0, names.Count)];
            InteractableObjectFactory.Create(randomName);
        }
    }
}