using DrawDebug;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skill.Strategy
{
    // 엑션을 진행하기 위해 주변 적을 타겟팅함
    public interface ITargetStrategy
    {
        List<IDamageable> GetDamageables(GameObject targetObject, CircleRangeTargetingStrategy.ChangeableData data, int targetCount, out List<Vector2> targetPoints) { targetPoints = new List<Vector2>(); return default; }
        List<IDamageable> GetDamageables(GameObject targetObject, CircleRangeTargetingStrategy.ChangeableData data) { return default; }
        List<IDamageable> GetDamageables(CircleRangeTargetingStrategy.ChangeableData data) { return default; }
        List<IDamageable> GetDamageables() { return default; }


        List<IDamageable> GetDamageables(GameObject targetObject, BoxRangeTargetingStrategy.ChangeableData data) { return default; }
        List<IDamageable> GetDamageables(BoxRangeTargetingStrategy.ChangeableData data) { return default; }

        IDamageable GetDamageable(GameObject targetObject) { return default; }
        ITarget GetTarget(GameObject targetObject) { return default; }
        IFollowable GetFollowableTarget(GameObject targetObject) { return default; }

        List<IDamageable> GetDamageables(out List<Vector2> startPoints, out List<Vector2> hitPoints) 
        {
            startPoints = new List<Vector2>();
            hitPoints = new List<Vector2>(); 
            return default; 
        }
    }

    public class NoTargetingStrategy : ITargetStrategy
    {
    }

    public class ContactTargetingStrategy : ITargetStrategy
    {
        List<ITarget.Type> _targetTypes;

        public ContactTargetingStrategy(
            List<ITarget.Type> targetTypes)
        {
            _targetTypes = targetTypes;
        }

        public IDamageable GetDamageable(GameObject targetObject)
        {
            ITarget target = targetObject.GetComponent<ITarget>();
            if (target == null) return null;

            bool isTarget = target.IsTarget(_targetTypes);
            if (isTarget == false) return null;

            IDamageable damageable = targetObject.GetComponent<IDamageable>();
            return damageable;
        }

        public ITarget GetTarget(GameObject targetObject)
        {
            ITarget target = targetObject.GetComponent<ITarget>();
            if (target == null) return null;

            bool isTarget = target.IsTarget(_targetTypes);
            if (isTarget == false) return null;

            return target;
        }

        public IFollowable GetFollowableTarget(GameObject targetObject)
        {
            ITarget target = targetObject.GetComponent<ITarget>();
            if (target == null) return null;

            bool isTarget = target.IsTarget(_targetTypes);
            if (isTarget == false) return null;

            IFollowable followable = targetObject.GetComponent<IFollowable>();
            return followable;
        }
    }

    public class RandomDirectionTargetingStrategy : ITargetStrategy
    {
        List<ITarget.Type> _targetTypes;
        float _distanceFromCaster;
        float _maxDistance;
        float _shootableLaserCount;
        int _totalLaserCount;
        Transform _myTransform;

        public RandomDirectionTargetingStrategy(
            float distanceFromCaster,
            float maxDistance,
            float shootableLaserCount,
            int totalLaserCount,
            Transform myTransform,
            List<ITarget.Type> targetTypes)
        {
            _distanceFromCaster = distanceFromCaster;
            _maxDistance = maxDistance;
            _shootableLaserCount = shootableLaserCount;
            _totalLaserCount = totalLaserCount;
            _myTransform = myTransform;
            _targetTypes = targetTypes;
        }

        List<float> GetAngles(int count)
        {
            List<float> angles = new List<float>();
            List<int> pickCount = new List<int>();
            while (pickCount.Count < _shootableLaserCount)
            {
                int random = Random.Range(1, _totalLaserCount + 1);
                if (pickCount.Contains(random) == false) pickCount.Add(random);
            }

            for (int i = 0; i < pickCount.Count; i++)
            {
                float angle = 360f / _totalLaserCount * pickCount[i];
                angles.Add(angle);
            }

            return angles;
        }

        Vector2 GetDirection(float angle)
        {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = Mathf.Cos(angle * Mathf.Deg2Rad);
            return new Vector2(x, y);
        }

        List<IDamageable> CastRayToDirection(List<float> angles, out List<Vector2> startPoints, out List<Vector2> hitPoints)
        {
            List<IDamageable> damageables = new List<IDamageable>();
            List<Vector2> tmpHitPoints = new List<Vector2>();
            List<Vector2> tmpStartPoints = new List<Vector2>();

            for (int i = 0;i < angles.Count; i++)
            {
                Vector2 direction = GetDirection(angles[i]);
                Vector2 spawnPosition = (Vector2)_myTransform.position + direction * _distanceFromCaster;

                RaycastHit2D[] hit2D = Physics2D.RaycastAll(spawnPosition, direction, _maxDistance);
                Array.Sort(hit2D, (x, y) => x.distance.CompareTo(y.distance));

                Vector2 stopPoint = spawnPosition + (direction * _maxDistance);

                for (int j = 0; j < hit2D.Length; j++)
                {
                    if (hit2D[j].collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) // 벽에 부딪힌 경우
                    {
                        stopPoint = hit2D[j].point;
                        continue;
                    }

                    ITarget target = hit2D[j].collider.gameObject.GetComponent<ITarget>();
                    if (target == null || target.IsTarget(_targetTypes) == false) continue;

                    IDamageable damageable = hit2D[j].collider.gameObject.GetComponent<IDamageable>();
                    if (damageable == null) continue;

                    damageables.Add(damageable);
                }

                tmpStartPoints.Add(spawnPosition);
                tmpHitPoints.Add(stopPoint);
            }

            hitPoints = tmpHitPoints;
            startPoints = tmpStartPoints;
            return damageables;
        }

        public List<IDamageable> GetDamageables(out List<Vector2> startPoints, out List<Vector2> hitPoints)
        {
            List<float> angles = GetAngles(_totalLaserCount);
            List<IDamageable> damageables = CastRayToDirection(angles, out startPoints, out hitPoints);
            return damageables;
        }
    }

    public class BoxRangeTargetingStrategy : ITargetStrategy
    {
        public struct ChangeableData
        {
            float _rangeMultiplier;

            public ChangeableData(float rangeMultiplier)
            {
                _rangeMultiplier = rangeMultiplier;
            }

            public float RangeMultiplier { get => _rangeMultiplier; }
        }

        ICaster _caster;
        Vector2 _size;
        Vector2 _offset;
        List<ITarget.Type> _targetTypes;

        public BoxRangeTargetingStrategy(
            ICaster caster,
            Vector2 size,
            Vector2 offset,
            List<ITarget.Type> targetTypes)
        {
            _caster = caster;
            _size = size;
            _offset = offset;
            _targetTypes = targetTypes;
        }

        public List<IDamageable> GetDamageables(ChangeableData data)
        {
            List<IDamageable> damageables = new List<IDamageable>();

            Transform casterTransform = _caster.GetComponent<Transform>();
            Vector2 casterPos = casterTransform.position;
            Vector2 convertedSize = _size * data.RangeMultiplier;

            // Calculate the angle between the direction vector and the positive x-axis
            float angle = Vector3.Angle(Vector3.right, casterTransform.forward);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(casterPos + _offset, convertedSize, angle);

            for (int i = 0; i < colliders.Length; i++)
            {
                ITarget target = colliders[i].GetComponent<ITarget>();
                if (target == null || target.IsTarget(_targetTypes) == false) continue;

                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageables.Add(damageable);
            }

#if UNITY_EDITOR
            DebugShape.DrawBox2D(casterPos, _offset, convertedSize, casterTransform.forward, Color.red, 3f);
#endif
            return damageables;
        }

        public List<IDamageable> GetDamageables(GameObject targetObject, ChangeableData data)
        {
            List<IDamageable> damageables = new List<IDamageable>();

            ITarget target = targetObject.GetComponent<ITarget>();
            if (target == null) return damageables;

            bool isTarget = target.IsTarget(_targetTypes);
            if (isTarget == false) return damageables;

            Transform casterTransform = _caster.GetComponent<Transform>();
            Vector2 casterPos = casterTransform.position;
            Vector2 convertedSize = _size * data.RangeMultiplier;

            // Calculate the angle between the direction vector and the positive x-axis
            float angle = Vector3.Angle(Vector3.right, casterTransform.forward);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(casterPos + _offset, convertedSize, angle);

            for (int i = 0; i < colliders.Length; i++)
            {
                ITarget closeTarget = colliders[i].GetComponent<ITarget>();
                if (closeTarget == null || closeTarget.IsTarget(_targetTypes) == false) continue;

                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageables.Add(damageable);
            }

#if UNITY_EDITOR
            DebugShape.DrawBox2D(casterPos, _offset, convertedSize, casterTransform.forward, Color.red, 3f);
#endif
            return damageables;
        }
    }

    public class CircleRangeTargetingStrategy : ITargetStrategy
    {
        public struct ChangeableData
        {
            float _rangeMultiplier;

            public ChangeableData(float rangeMultiplier)
            {
                _rangeMultiplier = rangeMultiplier;
            }

            public float RangeMultiplier { get => _rangeMultiplier; }
        }

        ICaster _caster;
        float _range;
        List<ITarget.Type> _targetTypes;

        public CircleRangeTargetingStrategy(
            ICaster caster,
            float range,
            List<ITarget.Type> targetTypes)
        {
            _caster = caster;
            _range = range;
            _targetTypes = targetTypes;
        }

        public List<IDamageable> GetDamageables()
        {
            List<IDamageable> damageables = new List<IDamageable>();

            Transform casterTransform = _caster.GetComponent<Transform>();
            Vector2 casterPos = casterTransform.position;
            float convertedRange = _range;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(casterPos, convertedRange);

            for (int i = 0; i < colliders.Length; i++)
            {
                ITarget target = colliders[i].GetComponent<ITarget>();
                if (target == null || target.IsTarget(_targetTypes) == false) continue;

                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageables.Add(damageable);
            }

#if UNITY_EDITOR
            DebugShape.DrawCircle2D(casterPos, convertedRange, Color.red, 3f);
#endif

            return damageables;
        }


        public List<IDamageable> GetDamageables(ChangeableData data)
        {
            List<IDamageable> damageables = new List<IDamageable>();

            Transform casterTransform = _caster.GetComponent<Transform>();
            Vector2 casterPos = casterTransform.position;
            float convertedRange = _range * data.RangeMultiplier;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(casterPos, convertedRange);

            for (int i = 0; i < colliders.Length; i++)
            {
                ITarget target = colliders[i].GetComponent<ITarget>();
                if (target == null || target.IsTarget(_targetTypes) == false) continue;

                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageables.Add(damageable);
            }

#if UNITY_EDITOR
            DebugShape.DrawCircle2D(casterPos, convertedRange, Color.red, 3f);
#endif

            return damageables;
        }

        public List<IDamageable> GetDamageables(GameObject targetObject, ChangeableData data)
        {
            List<IDamageable> damageables = new List<IDamageable>();

            ITarget target = targetObject.GetComponent<ITarget>(); // collision의 ITarget이 존재하는 경우만 진행
            if (target == null) return damageables;

            bool isTarget = target.IsTarget(_targetTypes);
            if (isTarget == false) return damageables;

            Transform casterTransform = _caster.GetComponent<Transform>();
            Vector2 casterPos = casterTransform.position;
            float convertedRange = _range * data.RangeMultiplier;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(casterPos, convertedRange);

            for (int i = 0; i < colliders.Length; i++)
            {
                ITarget closeTarget = colliders[i].GetComponent<ITarget>();
                if (closeTarget == null || closeTarget.IsTarget(_targetTypes) == false) continue;

                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageables.Add(damageable);
            }

#if UNITY_EDITOR
            DebugShape.DrawCircle2D(casterPos, convertedRange, Color.red, 3f);
#endif

            return damageables;
        }

        public List<IDamageable> GetDamageables(GameObject targetObject, ChangeableData data, int maxTargetCount, out List<Vector2> targetPoints)
        {
            List<IDamageable> damageables = new List<IDamageable>();
            targetPoints = new List<Vector2>();

            ITarget target = targetObject.GetComponent<ITarget>(); // collision의 ITarget이 존재하는 경우만 진행
            if (target == null)
            {
                return damageables;
            }

            bool isTarget = target.IsTarget(_targetTypes);
            if (isTarget == false)
            {
                return damageables;
            }

            Transform casterTransform = _caster.GetComponent<Transform>();
            Vector2 casterPos = casterTransform.position;
            float convertedRange = _range * data.RangeMultiplier;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(casterPos, convertedRange);
            int targetCount = 0;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (targetCount == maxTargetCount) break;

                ITarget closeTarget = colliders[i].GetComponent<ITarget>();
                if (closeTarget == null || closeTarget.IsTarget(_targetTypes) == false) continue;

                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageables.Add(damageable);
                targetPoints.Add(colliders[i].transform.position);
                targetCount++;
            }

#if UNITY_EDITOR
            DebugShape.DrawCircle2D(casterPos, convertedRange, Color.red, 3f);
#endif

            return damageables;
        }
    }
}