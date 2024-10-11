using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LobbyScrollController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTr;

    [SerializeField] Button _leftButton, _rightButton;

    const int SIZE = 3;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag;
    int targetIndex;

    void Start()
    {
        _leftButton.onClick.AddListener(() => TabClick(true));
        _rightButton.onClick.AddListener(() => TabClick(false));

        for (int i = 0; i < SIZE; i++)
        {
            RectTransform rectTransform = contentTr.GetChild(i).GetComponent<RectTransform>();

            // ���� sizeDelta ���� ������ (���� ũ���� ���̴� �����ϰ� �ʺ� ����)
            Vector2 newSize = rectTransform.sizeDelta;
            newSize.x = Screen.width;  // �ʺ� ����
            newSize.y = Screen.height;  // �ʺ� ����

            rectTransform.sizeDelta = newSize;  // ����� ���� �ٽ� �Ҵ�
        }

        // �Ÿ��� ���� 0~1�� pos����
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
    }

    float SetPos()
    {
        // ���ݰŸ��� �������� ����� ��ġ�� ��ȯ
        for (int i = 0; i < SIZE; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        return 0;
    }

    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();

    public void OnDrag(PointerEventData eventData) => isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        // ���ݰŸ��� ���� �ʾƵ� ���콺�� ������ �̵��ϸ�
        if (curPos == targetPos)
        {
            // �� ���� ������ ��ǥ�� �ϳ� ����
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }

            // �� ���� ������ ��ǥ�� �ϳ� ����
            else if (eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }
        }

        VerticalScrollUp();
    }

    void VerticalScrollUp()
    {
        // ��ǥ�� ������ũ���̰�, ������ �ŰܿԴٸ� ������ũ���� �� ���� �ø�
        for (int i = 0; i < SIZE; i++)
            if (contentTr.GetChild(i).GetComponent<ScrollScript>() && curPos != pos[i] && targetPos == pos[i])
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
    }

    void Update()
    {
        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
        }
    }

    public void TabClick(bool isLeft)
    {
        curPos = SetPos();

        if(isLeft)
        {
            targetIndex--;
            if (targetIndex < 0) targetIndex = SIZE - 1;
        }
        else
        {
            targetIndex = ++targetIndex % SIZE;
        }

        targetPos = pos[targetIndex];
        VerticalScrollUp();
    }
}