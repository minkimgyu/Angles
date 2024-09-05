using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BuffCommand : BaseCommand
{
    public virtual void Execute(float value, out BuffCommand command) { command = default; }
    public virtual void UnDo() { }
}

public class BuffValueCommand : BuffCommand
{
    BuffFloat _buffAttribute;
    float _storedValue;

    public BuffValueCommand(BuffFloat buffAttribute, float storedValue = 0)
    {
        _buffAttribute = buffAttribute;
        _storedValue = storedValue;
    }

    public override void Execute(float value, out BuffCommand buffCommand)
    {
        _storedValue = value;
        _buffAttribute.Value += _storedValue;

        buffCommand = new BuffValueCommand(_buffAttribute, _storedValue);
    }

    public override void UnDo()
    {
        _buffAttribute.Value -= _storedValue;
    }
}

public class BuffRatioCommand : BuffCommand
{
    BuffFloat _buffAttribute;
    float _storedValue;

    public BuffRatioCommand(BuffFloat buffAttribute, float storedValue = 0)
    {
        _buffAttribute = buffAttribute;
        _storedValue = storedValue;
    }

    public override void Execute(float ratio, out BuffCommand buffCommand)
    {
        _storedValue = _buffAttribute.Value * ratio;
        _buffAttribute.Value += _storedValue;

        buffCommand = new BuffRatioCommand(_buffAttribute, _storedValue);
    }

    public override void UnDo()
    {
        _buffAttribute.Value -= _storedValue;
    }
}