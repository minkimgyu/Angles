using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

abstract public class BaseEffect : MonoBehaviour
{
    public enum Name
    {
        DamageTextEffect,
        ExplosionEffect,
        KnockbackEffect,
        ImpactEffect,
        LaserEffect,

        HitEffect,
        ShockwaveEffect,

        MultipleShockwaveEffect,

        TriangleDestroyEffect,
        RectangleDestroyEffect,
        PentagonDestroyEffect,
        HexagonDestroyEffect,
    }

    [SerializeField] protected float _destoryDelay = 0;
    protected Timer _timer = new Timer();

    public virtual void Initialize() 
    {
    }

    public virtual void Play() { }

    void DestroyMe() { Destroy(gameObject); }

    public void DestoryAfterDelay() { Invoke("DestroyMe", _destoryDelay); }

    protected virtual void OnDestroy() => CancelInvoke();

    public virtual void ResetText(float damage) { }
    public virtual void ResetColor(Color color) { }
    public virtual void ResetLine(Vector3 endPoint) { }

    public virtual void AddFollower(IFollowable followable) { }

    public virtual void ResetSize(float ratio) { transform.localScale *= ratio; }
    public virtual void ResetSize(Vector3 scale) { transform.localScale = scale; }
    public virtual void ResetRotation(Quaternion quaternion) { transform.rotation = quaternion; }

    public virtual void ResetSize(Vector3 startScale, Vector3 endScale, float duration) { }


    public virtual void ResetPosition(Vector3 pos) { transform.position = pos; }
    public virtual void ResetPosition(Vector3 pos, Vector3 direction) { }
}
