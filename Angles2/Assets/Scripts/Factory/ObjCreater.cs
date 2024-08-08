using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseCreater<Output>
{
    public virtual Output Create() { return default; }
}

abstract public class ObjCreater<Input> : BaseCreater<Input>
{
    protected Input _prefab;
    public virtual void Initialize(Input prefab) { _prefab = prefab; }
}