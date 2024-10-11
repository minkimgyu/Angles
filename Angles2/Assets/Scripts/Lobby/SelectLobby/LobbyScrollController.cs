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

            // 현재 sizeDelta 값을 가져옴 (현재 크기의 높이는 유지하고 너비만 수정)
            Vector2 newSize = rectTransform.sizeDelta;
            newSize.x = Screen.width;  // 너비 설정
            newSize.y = Screen.height;  // 너비 설정

            rectTransform.sizeDelta = newSize;  // 변경된 값을 다시 할당
        }

        // 거리에 따라 0~1인 pos대입
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) pos[i] = distance * i;
    }

    float SetPos()
    {
        // 절반거리를 기준으로 가까운 위치를 반환
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

        // 절반거리를 넘지 않아도 마우스를 빠르게 이동하면
        if (curPos == targetPos)
        {
            // ← 으로 가려면 목표가 하나 감소
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }

            // → 으로 가려면 목표가 하나 증가
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
        // 목표가 수직스크롤이고, 옆에서 옮겨왔다면 수직스크롤을 맨 위로 올림
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