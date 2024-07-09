using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawDebug
{
    class DebugShape
    {
        public static void DrawBox2D(Vector2 pos, Vector2 offset, Vector2 size, Vector2 direction, Color color, float duration = 3)
        {
            // ���÷� ������ ���� ���ͷκ��� Quaternion ����
            Quaternion rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            // OverlapBox�� ��踦 �׸��� ���� ������ ���
            Vector2 halfSize = size / 2f;

            Vector2[] vertices = new Vector2[]
            {
                rotation * (new Vector2(-halfSize.x, -halfSize.y) + offset),
                rotation * (new Vector2(halfSize.x, -halfSize.y) + offset),
                rotation * (new Vector2(halfSize.x, halfSize.y) + offset),
                rotation * (new Vector2(-halfSize.x, halfSize.y) + offset),
                rotation * (new Vector2(-halfSize.x, -halfSize.y) + offset) // ������ ���� �������� ����
            };

            // �� �������� �̾ ������ �׸��ϴ�.
            Debug.DrawLine(vertices[0] + pos, vertices[1] + pos, color, duration);
            Debug.DrawLine(vertices[1] + pos, vertices[2] + pos, color, duration);
            Debug.DrawLine(vertices[2] + pos, vertices[3] + pos, color, duration);
            Debug.DrawLine(vertices[3] + pos, vertices[0] + pos, color, duration);
        }


        public static void DrawCircle2D(Vector2 pos, float range, Color color, float duration = 3, int segments = 30)
        {
            // ���� �׸� ����
            float angle = 0f;
            float angleStep = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                // ���� ��
                Vector2 startPoint = pos + new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * range;

                // ���� ��
                angle += angleStep;
                Vector2 endPoint = pos + new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * range;

                // �� �� ���̿� ���� �׸�
                Debug.DrawLine(startPoint, endPoint, color, duration);
            }
        }
    }
}