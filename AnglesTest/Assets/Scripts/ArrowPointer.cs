using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform player;       // �÷��̾��� Transform
    public Transform target;       // ���� Transform
    public RectTransform arrowUI;  // ȭ��ǥ UI�� RectTransform
    public Canvas canvas;          // UI�� ��ġ�� ĵ����
    public float edgeOffset = 20f; // ȭ�� �𼭸����� ����

    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.position);

        // Ÿ���� ȭ�� �ۿ� �ִ� ���
        if (screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height)
        {
            // ȭ�� �߽ɰ� Ÿ�� ���� ���
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 direction = ((Vector2)screenPoint - screenCenter).normalized;

            // �𼭸��� ���� ȭ��ǥ ��ġ ���
            float canvasWidth = canvas.pixelRect.width;
            float canvasHeight = canvas.pixelRect.height;
            float halfWidth = canvasWidth / 2 - edgeOffset;
            float halfHeight = canvasHeight / 2 - edgeOffset;

            // ȭ�� �𼭸� ������ ���
            Vector2 edgePosition = Vector2.zero;
            float slope = direction.y / direction.x;

            if (Mathf.Abs(slope) > halfHeight / halfWidth) // ��/�Ʒ��� ��� ���
            {
                edgePosition.y = Mathf.Sign(direction.y) * halfHeight;
                edgePosition.x = edgePosition.y / slope;
            }
            else // ��/�쿡 ��� ���
            {
                edgePosition.x = Mathf.Sign(direction.x) * halfWidth;
                edgePosition.y = edgePosition.x * slope;
            }

            // ȭ��ǥ UI ��ġ �� ȸ�� ����
            arrowUI.anchoredPosition = edgePosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0, 0, angle);

            // ȭ��ǥ Ȱ��ȭ
            arrowUI.gameObject.SetActive(true);
        }
        else
        {
            // Ÿ���� ȭ�� �ȿ� ������ ȭ��ǥ �����
            arrowUI.gameObject.SetActive(false);
        }
    }
}
