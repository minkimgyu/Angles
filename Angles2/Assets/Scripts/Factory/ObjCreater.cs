using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseCreater<Output>
{
    public virtual Output Create() { return default; }
}

abstract public class ObjCreater<Output> : BaseCreater<Output>
{
    protected GameObject _prefab;
    public virtual void Initialize(GameObject prefab) { _prefab = prefab; }
}