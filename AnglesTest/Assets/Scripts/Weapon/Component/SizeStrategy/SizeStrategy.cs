using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public interface ISizeModifyStat
//{
//    public float SizeMultiplier { get; set; }
//}

public interface ISizeStrategy
{
    void OnActivate();
    void ChangeSize(float size) { }
}

public class NoSizeStrategy : ISizeStrategy
{
    public void OnActivate() { }
}

public class ChangeableSizeStrategy : ISizeStrategy
{
    Transform _transform;
    float _sizeMultiplier;

    public ChangeableSizeStrategy(
        Transform transform) 
    { 
        _transform = transform;
        _sizeMultiplier = 1.0f;
    }

    public void ChangeSize(float sizeMultiplier) 
    {
        _sizeMultiplier = sizeMultiplier;
    }


    public void OnActivate() { _transform.localScale *= _sizeMultiplier; }
}