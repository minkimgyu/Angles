using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseEffect : MonoBehaviour
{
    public enum Name
    {
        DamageText,
        Explosion,
        Knockback,
        Impact,
        Signal,
        Laser,

        Blade,
    }

    [SerializeField] protected float _destoryDelay = 0;

    public virtual void Initialize() { }
    public abstract void Play();
    public virtual void ResetDestoryDelay(float delay) { _destoryDelay = delay; }

    protected virtual void DestoryMe() => Destroy(gameObject);
    public void DestoryAfterDelay() => Invoke("DestoryMe", _destoryDelay);

    protected virtual void OnDestroy() => CancelInvoke();

    public virtual void ResetText(float damage) { }
    public virtual void ResetLine(Vector3 endPoint) { }
   

    public virtual void ResetPosition(Vector3 pos) { transform.position = pos; }
    public virtual void ResetPosition(Vector3 pos, Vector3 direction) { }
}
