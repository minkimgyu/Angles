using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemCommand : BaseCommand
{
    public DropItemCommand(Action<DropData, Vector3> DropItemAction)
    {
        this.DropItemAction = DropItemAction;
    }

    Action<DropData, Vector3> DropItemAction;

    public override void Execute(DropData value1, Vector3 value2)
    {
        DropItemAction?.Invoke(value1, value2);
    }
}

public class RegisterFollowerCommand : BaseCommand
{
    public RegisterFollowerCommand(Action<IFollowable> RegisterFollowerAction)
    {
        this.RegisterFollowerAction = RegisterFollowerAction;
    }

    Action<IFollowable> RegisterFollowerAction;

    public override void Execute(IFollowable followable)
    {
        RegisterFollowerAction?.Invoke(followable);
    }
}

public class CreateCardCommand : BaseCommand
{
    public CreateCardCommand(Action<ICaster, int> CreateCardAction)
    {
        this.CreateCardAction = CreateCardAction;
    }

    Action<ICaster, int> CreateCardAction;

    public override void Execute(ICaster skillUser, int cardCount)
    {
        CreateCardAction?.Invoke(skillUser, cardCount);
    }
}

public class CreateReusableCardCommand : BaseCommand
{
    public CreateReusableCardCommand(Action<ICaster, int, int> CreateReusableCardAction)
    {
        this.CreateReusableCardAction = CreateReusableCardAction;
    }

    Action<ICaster, int, int> CreateReusableCardAction;

    public override void Execute(ICaster skillUser, int cardCount, int recreateCount)
    {
        CreateReusableCardAction?.Invoke(skillUser, cardCount, recreateCount);
    }
}

public class SubEventBus : BaseEventBus<SubEventBus.State>
{
    public enum State
    {
        CreateCard,
        CreateReusableCard,
        
        DropItem,
        AddFollableCamera,
    }
}