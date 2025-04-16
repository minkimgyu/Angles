using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill.Strategy
{
    public interface IEffectStrategy
    {
        void SpawnEffect(Vector2 pos) { }
        void SpawnEffect(Vector2 pos, float sizeMultiply) { }
        void SpawnEffect(Vector2 pos, Vector2 direction) { }
        void SpawnEffect(Vector2 startPoint, List<Vector2> hitPoints) { }
        void SpawnEffect(List<Vector2> startPoints, List<Vector2> hitPoints) { }
    }

    public class NoEffectStrategy : IEffectStrategy
    {
    }

    public class ParticleEffectStrategy : IEffectStrategy
    {
        ICaster _caster;
        BaseEffect.Name _effectName;
        BaseFactory _effectFactory;

        public ParticleEffectStrategy(
            BaseEffect.Name effectName,
            BaseFactory effectFactory)
        {
            _effectName = effectName;
            _effectFactory = effectFactory;
        }

        public void SpawnEffect(Vector2 pos)
        {
            BaseEffect effect = _effectFactory.Create(_effectName);
            if (effect != null)
            {
                effect.ResetPosition(pos);
                effect.Play();
            }
        }

        public void SpawnEffect(Vector2 pos, float sizeMultiply) 
        {
            BaseEffect effect = _effectFactory.Create(_effectName);
            if (effect != null)
            {
                effect.ResetPosition(pos);
                effect.ResetSize(sizeMultiply);
                effect.Play();
            }
        }
    }

    public class DirectionParticleEffectStrategy : IEffectStrategy
    {
        ICaster _caster;
        BaseEffect.Name _effectName;
        BaseFactory _effectFactory;
      

        public DirectionParticleEffectStrategy(
            BaseEffect.Name effectName,
            BaseFactory effectFactory)
        {
            _effectName = effectName;
            _effectFactory = effectFactory;
        }

        public void SpawnEffect(Vector2 pos, Vector2 direction)
        {
            BaseEffect effect = _effectFactory.Create(_effectName);
            if (effect != null)
            {
                effect.ResetPosition(pos, direction);
                effect.Play();
            }
        }
    }

    public class LaserEffectStrategy : IEffectStrategy
    {
        BaseEffect.Name _effectName;
        Color _startColor;
        Color _endColor;
        BaseFactory _effectFactory;

        public LaserEffectStrategy(
            BaseEffect.Name effectName,
            Color startColor,
            Color endColor,
            BaseFactory effectFactory)
        {
            _effectName = effectName;
            _startColor = startColor;
            _endColor = endColor;
            _effectFactory = effectFactory;
        }

        public void SpawnEffect(Vector2 startPoint, List<Vector2> hitPoints)
        {
            for (int i = 0; i < hitPoints.Count; i++)
            {
                BaseEffect effect = _effectFactory.Create(BaseEffect.Name.LaserEffect);
                effect.ResetColor(_startColor, _endColor);
                effect.ResetPosition(Vector3.zero);
                effect.ResetLine(startPoint, hitPoints[i]);

                effect.Play();
            }
        }

        public void SpawnEffect(List<Vector2> startPoints, List<Vector2> hitPoints)
        {
            for (int i = 0; i < hitPoints.Count; i++)
            {
                BaseEffect effect = _effectFactory.Create(BaseEffect.Name.LaserEffect);
                effect.ResetColor(_startColor, _endColor);
                effect.ResetPosition(Vector3.zero);
                effect.ResetLine(startPoints[i], hitPoints[i]);

                effect.Play();
            }
        }
    }
}