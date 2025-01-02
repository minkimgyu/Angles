using UnityEngine;

public class HPBar : MonoBehaviour
{
    public SpriteRenderer foreground; // 체력 바
    public float maxHP = 100f;        // 최대 체력
    private float currentHP;          // 현재 체력

    void Start()
    {
        currentHP = maxHP; // 체력 초기화
    }

    // 체력 업데이트 메서드
    [ContextMenu("")]
    public void UpdateHP(float newHP)
    {
        currentHP = Mathf.Clamp(newHP, 0, maxHP); // 체력을 0~최대 값으로 제한
        UpdateBar();
    }

    // 체력 바 갱신
    private void UpdateBar()
    {
        if (foreground != null)
        {
            float healthRatio = currentHP / maxHP; // 체력 비율 계산
            foreground.transform.localScale = new Vector3(healthRatio, 1, 1); // 가로 크기 조정
        }
    }
}
