using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseCreater<Input, Output>
{
    public abstract void Initialize(Input input);
    public virtual Output Create() { return default; }
}