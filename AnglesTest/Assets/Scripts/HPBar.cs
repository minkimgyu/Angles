using UnityEngine;

public class HPBar : MonoBehaviour
{
    public SpriteRenderer foreground; // ü�� ��
    public float maxHP = 100f;        // �ִ� ü��
    private float currentHP;          // ���� ü��

    void Start()
    {
        currentHP = maxHP; // ü�� �ʱ�ȭ
    }

    // ü�� ������Ʈ �޼���
    [ContextMenu("")]
    public void UpdateHP(float newHP)
    {
        currentHP = Mathf.Clamp(newHP, 0, maxHP); // ü���� 0~�ִ� ������ ����
        UpdateBar();
    }

    // ü�� �� ����
    private void UpdateBar()
    {
        if (foreground != null)
        {
            float healthRatio = currentHP / maxHP; // ü�� ���� ���
            foreground.transform.localScale = new Vector3(healthRatio, 1, 1); // ���� ũ�� ����
        }
    }
}
