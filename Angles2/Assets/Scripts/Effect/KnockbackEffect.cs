using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackEffect : ParticleEffect
{
    [SerializeField] ParticleSystem _impact;
    [SerializeField] float _impactVelocity = 0;

    public override void ResetPosition(Vector3 pos, Vector3 direction)
    {
        transform.position = pos;
        transform.right = direction;

        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule;
        velocityOverLifetimeModule = _impact.velocityOverLifetime;

        // 속도 그래프를 Linear로 설정
        velocityOverLifetimeModule.x = new ParticleSystem.MinMaxCurve(direction.x * _impactVelocity);
        velocityOverLifetimeModule.y = new ParticleSystem.MinMaxCurve(direction.y * _impactVelocity);
        velocityOverLifetimeModule.z = new ParticleSystem.MinMaxCurve(direction.z * _impactVelocity);
    }
}
