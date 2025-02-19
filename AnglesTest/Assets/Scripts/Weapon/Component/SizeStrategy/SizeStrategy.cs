using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISizeModifyStat
{
    public float SizeMultiplier { get; set; }
}

public interface ISizeStrategy
{
    void OnActivate();
}

public class NoSizeStrategy : ISizeStrategy
{
    public void OnActivate() { }
}

public class ChangeableSizeStrategy : ISizeStrategy
{
    Transform _transform;
    ISizeModifyStat _sizeStat;

    public ChangeableSizeStrategy(Transform transform, ISizeModifyStat sizeStat) { _transform = transform; _sizeStat = sizeStat; }

    public void OnActivate() { _transform.localScale *= _sizeStat.SizeMultiplier; }
}