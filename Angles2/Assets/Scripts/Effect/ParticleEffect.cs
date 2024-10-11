using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : BaseEffect
{
    List<ParticleSystem> _effects;

    public override void Initialize()
    {
        base.Initialize();
        _effects = new List<ParticleSystem>();

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particles.Length; i++)
        {
            _effects.Add(particles[i]);
        }
    }

    public override void Play()
    {
        for (int i = 0; i < _effects.Count; i++)
        {
            _effects[i].Play();
        }

        DestoryAfterDelay();
    }
}
