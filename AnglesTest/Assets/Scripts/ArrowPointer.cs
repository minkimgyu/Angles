using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform player;       // 플레이어의 Transform
    public Transform target;       // 적의 Transform
    public RectTransform arrowUI;  // 화살표 UI의 RectTransform
    public Canvas canvas;          // UI가 위치한 캔버스
    public float edgeOffset = 20f; // 화면 모서리와의 여백

    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.position);

        // 타겟이 화면 밖에 있는 경우
        if (screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height)
        {
            // 화면 중심과 타겟 방향 계산
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 direction = ((Vector2)screenPoint - screenCenter).normalized;

            // 모서리를 따라 화살표 위치 계산
            float canvasWidth = canvas.pixelRect.width;
            float canvasHeight = canvas.pixelRect.height;
            float halfWidth = canvasWidth / 2 - edgeOffset;
            float halfHeight = canvasHeight / 2 - edgeOffset;

            // 화면 모서리 교차점 계산
            Vector2 edgePosition = Vector2.zero;
            float slope = direction.y / direction.x;

            if (Mathf.Abs(slope) > halfHeight / halfWidth) // 위/아래에 닿는 경우
            {
                edgePosition.y = Mathf.Sign(direction.y) * halfHeight;
                edgePosition.x = edgePosition.y / slope;
            }
            else // 좌/우에 닿는 경우
            {
                edgePosition.x = Mathf.Sign(direction.x) * halfWidth;
                edgePosition.y = edgePosition.x * slope;
            }

            // 화살표 UI 위치 및 회전 설정
            arrowUI.anchoredPosition = edgePosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0, 0, angle);

            // 화살표 활성화
            arrowUI.gameObject.SetActive(true);
        }
        else
        {
            // 타겟이 화면 안에 있으면 화살표 숨기기
            arrowUI.gameObject.SetActive(false);
        }
    }
}
