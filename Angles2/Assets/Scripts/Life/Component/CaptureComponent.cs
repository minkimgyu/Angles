using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaptureComponent<T> : MonoBehaviour
{
    List<T> _targets;

    Action<Collider2D> OnTargetEnter;
    Action<Collider2D> OnTargetExit;

    public void Initialize()
    {
        _targets = new List<T>();
    }

    public void Initialize(Action<Collider2D> OnTargetEnter, Action<Collider2D> OnTargetExit)
    {
        _targets = new List<T>();

        this.OnTargetEnter = OnTargetEnter;
        this.OnTargetExit = OnTargetExit;
    }

    public List<T> ReturnTargets() { return _targets; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        T target = collision.GetComponent<T>();
        if (target == null) return;

        OnTargetEnter?.Invoke(collision);
        _targets.Add(target);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        T target = collision.GetComponent<T>();
        if (target == null) return;

        OnTargetExit?.Invoke(collision);
        _targets.Remove(target);
    }
}
