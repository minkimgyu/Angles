using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : BaseWeapon
{
    AbsorbableCaptureComponent _absorbCaptureComponent;

    float _lifeTime;
    float _absorbForce;
    float _maxTargetCount;

    public override void Initialize(float damage, float lifeTime, float absorbForce, int maxTargetCount) 
    {
        _damage = damage;
        _lifeTime = lifeTime;
        _absorbForce = absorbForce;
        _maxTargetCount = maxTargetCount;

        _absorbCaptureComponent = GetComponentInChildren<AbsorbableCaptureComponent>();
        _absorbCaptureComponent.Initialize(OnTargetEnter, OnTargetExit);
    }

    void OnTargetEnter(Collider2D collider)
    {

    }

    void OnTargetExit(Collider2D collider)
    {

    }

    private void Update()
    {
        
    }
}
