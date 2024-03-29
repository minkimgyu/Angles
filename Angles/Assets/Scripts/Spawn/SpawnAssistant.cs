using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawGizmo
{
    public Color color;
    public float width;
    public float height;
    public float radius;
    public Vector3 offset;

    public LayerMask layer;

    public void DrawBoxGizmo(Transform tr)
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(tr.position + offset, new Vector3(width, height));
    }

    public void DrawCircleGizmo(Transform tr)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(tr.position + offset, radius);
    }

    public RaycastHit2D[] CheckBoxArea(Transform tr)
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(tr.position, new Vector3(width, height), 0, Vector2.right, 0, layer);
        return hit;
    }
}

public class SpawnAssistant : MonoBehaviour
{
    public DrawGizmo playerViewArea;
    public DrawGizmo canSpawnArea;

    public List<Transform> FindSpawnPoint()
    {
        List<Transform> activeSpawnPoints = new List<Transform>();

        RaycastHit2D[] spawnAreaObjects =  canSpawnArea.CheckBoxArea(transform);
        RaycastHit2D[] aroundPlayerObjects = playerViewArea.CheckBoxArea(transform);

        for (int i = 0; i < spawnAreaObjects.Length; i++)
        {
            activeSpawnPoints.Add(spawnAreaObjects[i].transform);
        }

        for (int i = 0; i < aroundPlayerObjects.Length; i++)
        {
            activeSpawnPoints.Remove(aroundPlayerObjects[i].transform);
        }

        return activeSpawnPoints;
    }

    public List<Transform> FindItemSpawnPoint()
    {
        List<Transform> activeSpawnPoints = new List<Transform>();

        RaycastHit2D[] aroundPlayerObjects = playerViewArea.CheckBoxArea(transform);

        for (int i = 0; i < aroundPlayerObjects.Length; i++)
        {
            activeSpawnPoints.Add(aroundPlayerObjects[i].transform);
        }

        return activeSpawnPoints;
    }

    void OnDrawGizmos()
    {
        playerViewArea.DrawBoxGizmo(transform);
        canSpawnArea.DrawBoxGizmo(transform);
    }
}
