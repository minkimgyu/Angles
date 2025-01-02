using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseEventBus<T>
{
    protected Dictionary<T, BaseCommand> commands = new Dictionary<T, BaseCommand>();

    public void Register(T state, BaseCommand command)
    {
        commands.Add(state, command);
    }

    //이벤트 해제
    public void Unregister(T state, BaseCommand command)
    {
        if (!commands.ContainsKey(state)) return;
        commands.Remove(state);
    }

    //이벤트 실행
    public virtual void Publish(T state)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute();
    }

    public virtual void Publish(T state, bool item)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(item);
    }

    public virtual void Publish(T state, float item)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(item);
    }

    public virtual void Publish(T state, BaseSkill.Name name, BaseSkill skill)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(name, skill);
    }

    public virtual void Publish(T state, BaseSkill.Name name)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(name);
    }


    public virtual void Publish(T state, IFollowable value1)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(value1);
    }

    public virtual void Publish(T state, DropData value1, Vector3 value2)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(value1, value2);
    }

    public virtual void Publish(T state, ICaster caster, int value1)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(caster, value1);
    }

    public virtual void Publish(T state, ICaster caster, int value1, int value2)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(caster, value1, value2);
    }
}
