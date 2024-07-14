using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaptureComponent<T> : MonoBehaviour
{
    public void Initialize(Action<T> OnEnter)
    {
        this.OnEnter = OnEnter;
        this.OnExit = null;
    }

    public void Initialize(Action<T> OnEnter, Action<T> OnExit)
    {
        this.OnEnter = OnEnter;
        this.OnExit = OnExit;
    }

    Action<T> OnEnter;
    Action<T> OnExit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        T target = collision.GetComponent<T>();
        if (target == null) return;

        OnEnter?.Invoke(target);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        T target = collision.GetComponent<T>();
        if (target == null) return;

        OnExit?.Invoke(target);
    }
}

public class CaptureComponent<T1, T2> : MonoBehaviour
{
    public void Initialize(Action<T1, T2> OnEnter)
    {
        this.OnEnter = OnEnter;
        this.OnExit = null;
    }

    public void Initialize(Action<T1, T2> OnEnter, Action<T1, T2> OnExit)
    {
        this.OnEnter = OnEnter;
        this.OnExit = OnExit;
    }

    Action<T1, T2> OnEnter;
    Action<T1, T2> OnExit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        T1 target1 = collision.GetComponent<T1>();
        T2 target2 = collision.GetComponent<T2>();

        if (target1 == null || target2 == null) return;
        OnEnter?.Invoke(target1, target2);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        T1 target1 = collision.GetComponent<T1>();
        T2 target2 = collision.GetComponent<T2>();

        if (target1 == null || target2 == null) return;
        OnExit?.Invoke(target1, target2);
    }
}
