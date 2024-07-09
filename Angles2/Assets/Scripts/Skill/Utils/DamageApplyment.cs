using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrawDebug;
using System;

namespace DamageUtility
{
    class Damage
    {
        public static void HitBoxRange(DamageData damageData, Vector2 pos, Vector2 offset, Vector2 direction, Vector2 size,
            bool drawDebug = false, Color color = default(Color), float duration = 3)
        {
            // Calculate the angle between the direction vector and the positive x-axis
            float angle = Vector3.Angle(Vector3.right, direction);

            Collider2D[] colliders = Physics2D.OverlapBoxAll(pos + offset, size, angle);
            if(drawDebug == true) DebugShape.DrawBox2D(pos, offset, size, direction, color, duration);

            for (int i = 0; i < colliders.Length; i++)
            {
                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageable.GetDamage(damageData);
            }
        }

        public static void HitCircleRange(DamageData damageData, Vector2 pos, float range,
            bool drawDebug = false, Color color = default(Color), float duration = 3)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, range);
            if (drawDebug == true) DebugShape.DrawCircle2D(pos, range, color, duration);

            for (int i = 0; i < colliders.Length; i++)
            {
                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageable.GetDamage(damageData);
            }
        }

        public static void HitRaycast(DamageData damageData, int maxTargetCount, Vector2 pos, float range, out List<Vector2> hitPoints,
            bool drawDebug = false, Color color = default(Color), float duration = 3)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, range);
            if (drawDebug == true) DebugShape.DrawCircle2D(pos, range, color, duration);

            hitPoints = new List<Vector2>();

            int targetCount = 0;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (targetCount > maxTargetCount) break;

                IDamageable damageable = colliders[i].GetComponent<IDamageable>();
                if (damageable == null) continue;

                damageable.GetDamage(damageData);

                Vector2 endPoint = (Vector2)colliders[i].transform.position - pos;
                if (drawDebug == true) Debug.DrawLine(pos, endPoint, color, duration);

                hitPoints.Add(endPoint);
                targetCount++;
            }
        }
    }
}