using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage : MonoBehaviour
{
    protected System.Action OnClearRequested;

    [SerializeField] Transform _entryPoint;
    protected Portal _exitPortal;

    public void ActivePortal(Vector2 movePos)
    {
        _exitPortal.Active(movePos);
    }

    public void Initialize(System.Action OnClearRequested)
    {
        this.OnClearRequested = OnClearRequested;

        //InteractableObjectFactory.Create();
        _exitPortal.Initialize();
    }

    public Vector3 ReturnEntryPosition()
    {
        return _entryPoint.position;
    }

    public virtual void Spawn(List<BaseLife.Name> names) { }
    public virtual void Spawn(List<IInteractable.Name> names) { }
}






public class BonusStage : BaseStage
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
