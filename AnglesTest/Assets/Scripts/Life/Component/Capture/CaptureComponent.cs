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

    public void Initialize(float range, Action<T> OnEnter)
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider == null) return;

        collider.radius = range;
        this.OnEnter = OnEnter;
        this.OnExit = null;
    }

    public void Initialize(float range, Action<T> OnEnter, Action<T> OnExit)
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider == null) return;

        collider.radius = range;
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

    public void Initialize(float range, Action<T1, T2> OnEnter)
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider == null) return;

        collider.radius = range;
        this.OnEnter = OnEnter;
        this.OnExit = null;
    }

    public void Initialize(float range, Action<T1, T2> OnEnter, Action<T1, T2> OnExit)
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider == null) return;

        collider.radius = range;
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

public class CaptureComponent<T1, T2, T3> : MonoBehaviour
{
    public void Initialize(Action<T1, T2, T3> OnEnter, Action<T1, T2, T3> OnExit)
    {
        this.OnEnter = OnEnter;
        this.OnExit = OnExit;
    }

    Action<T1, T2, T3> OnEnter;
    Action<T1, T2, T3> OnExit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        T1 target1 = collision.GetComponent<T1>();
        T2 target2 = collision.GetComponent<T2>();
        T3 target3 = collision.GetComponent<T3>();

        if (target1 == null || target2 == null || target3 == null) return;
        OnEnter?.Invoke(target1, target2, target3);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        T1 target1 = collision.GetComponent<T1>();
        T2 target2 = collision.GetComponent<T2>();
        T3 target3 = collision.GetComponent<T3>();

        if (target1 == null || target2 == null || target3 == null) return;
        OnExit?.Invoke(target1, target2, target3);
    }
}