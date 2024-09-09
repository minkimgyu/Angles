using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IItem<Node>
{
    public Node(Vector2 worldPos, Grid2D index, bool block)
    {
        _worldPos = worldPos;
        _index = index;
        _block = block;
        StoredIndex = -1;
    }

    Vector2 _worldPos; // 실제 위치
    public Vector2 WorldPos { get { return _worldPos; } }

    Grid2D _index; // 그리드 상 인덱스
    public Grid2D Index { get { return _index; } }

    bool _block;
    public bool Block { get { return _block; } }

    // g는 시작 노드부터의 거리
    // h는 끝 노드부터의 거리
    // f는 이 둘을 합친 값
    float g, h = 0;
    public float G { get { return g; } set { g = value; } }
    public float H { get { return h; } set { h = value; } }
    public float F { get { return g + h; } }

    public Node ParentNode { get; set; }
    public int StoredIndex { get; set; }

    public int CompareTo(Node other)
    {
        int compareValue = F.CompareTo(other.F);
        if (compareValue == 0) compareValue = H.CompareTo(other.H);
        return compareValue;
    }

    public void Dispose()
    {
        StoredIndex = -1;
        ParentNode = null;
    }
}