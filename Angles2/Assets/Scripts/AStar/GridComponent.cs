using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Linq;

[Serializable]
public struct Grid2D
{
    [SerializeField] int row;
    public int Row { get { return row; } }

    [SerializeField] int column;
    public int Column { get { return column; } }

    public Grid2D(int row, int column)
    {
        this.row = row; 
        this.column = column;
    }
}

public class GridComponent : MonoBehaviour
{
    [SerializeField] Tilemap _wallTile;

    Node[,] _nodes; // r, c
    [SerializeField] Transform _topLeftPoint;
    Vector2Int TopLeftLocalPos { get { return new Vector2Int(Mathf.RoundToInt(_topLeftPoint.localPosition.x), Mathf.RoundToInt(_topLeftPoint.localPosition.y)); } }
    Vector2 TopLeftWorldPos { get { return new Vector2(_topLeftPoint.position.x, _topLeftPoint.position.y); } }

    [SerializeField] Grid2D _gridSize;

    const int _nodeSize = 1;

    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _endPoint;
    [SerializeField] Pathfinder _pathfinder;

    public Node ReturnNode(Grid2D grid) { return _nodes[grid.Row, grid.Column]; }
    public Node ReturnNode(int r, int c) { return _nodes[r, c]; }

    public Vector2 ReturnClampedRange(Vector2 pos)
    {
        Vector2 topLeftPos = ReturnNode(0, 0).WorldPos;
        Vector2 bottomRightPos = ReturnNode(_nodes.GetLength(0) - 1, _nodes.GetLength(1) - 1).WorldPos;

        // 반올림하고 범위 안에 맞춰줌
        // 이 부분은 GridSize 바뀌면 수정해야함
        float xPos = Mathf.Clamp(pos.x, topLeftPos.x, bottomRightPos.x);
        float yPos = Mathf.Clamp(pos.y, bottomRightPos.y, topLeftPos.y);

        return new Vector2(xPos, yPos);
    }

    public Grid2D ReturnNodeIndex(Vector2 worldPos)
    {
        Vector2 clampedPos = ReturnClampedRange(worldPos);
        Vector2 topLeftPos = ReturnNode(0, 0).WorldPos;

        int r = Mathf.RoundToInt(Mathf.Abs(topLeftPos.y - clampedPos.y) / _nodeSize);
        int c = Mathf.RoundToInt(Mathf.Abs(topLeftPos.x - clampedPos.x) / _nodeSize); // 인덱스이므로 1 빼준다.
        return new Grid2D(r, c);
    }

    public bool HaveBlockNodeInNearPosition(Grid2D grid, int count)
    {
        if (count == 0) return false;

        Node node = _nodes[grid.Row, grid.Column];
        if (node.Block == true) return true;

        HashSet<Node> closeHash = new HashSet<Node>();
        Queue<Node> nodeQueue = new Queue<Node>();
        nodeQueue.Enqueue(node);

        int queueCnt = nodeQueue.Count;
        for (int i = 0; i < queueCnt; i++)
        {
            Node frontNode = nodeQueue.Dequeue();

            if (frontNode.Block == true) return true;
            Grid2D frontGrid = frontNode.Index;

            List<Grid2D> nodeIndexes = ReturnNearNodeIndexes(frontGrid);
            for (int k = 0; k < nodeIndexes.Count; k++)
            {
                Node nearNode = _nodes[nodeIndexes[k].Row, nodeIndexes[k].Column];
                if (nearNode.Block == true) return true;

                bool nowHave = closeHash.Contains(nearNode);
                if (nowHave == true) continue;

                closeHash.Add(nearNode);
                nodeQueue.Enqueue(nearNode); // 가지고 있지 않다면 넣는다.
            }
        }

        return false;
    }

    public List<Grid2D> ReturnNearNodeIndexes(Grid2D index)
    {
        List<Grid2D> closeNodeIndexes = new List<Grid2D>();

        // 주변 그리드
        Grid2D[] closeIndexes = new Grid2D[]  // r, c
        {
            new Grid2D(index.Row - 1, index.Column - 1), new Grid2D(index.Row - 1, index.Column), new Grid2D(index.Row - 1, index.Column + 1),

            new Grid2D(index.Row, index.Column - 1), new Grid2D(index.Row, index.Column + 1),

            new Grid2D(index.Row + 1, index.Column - 1), new Grid2D(index.Row + 1, index.Column), new Grid2D(index.Row + 1, index.Column + 1)
        };

        for (int i = 0; i < closeIndexes.Length; i++)
        {
            bool isOutOfRange = 
            closeIndexes[i].Row < 0 || closeIndexes[i].Column < 0 || 
            closeIndexes[i].Row >= _gridSize.Row || closeIndexes[i].Column >= _gridSize.Column;

            if (isOutOfRange == true) continue;
            closeNodeIndexes.Add(closeIndexes[i]);
        }

        return closeNodeIndexes;
    }

    //public List<Grid2D> ReturnPosibleNodes(Grid2D index)
    //{
    //    List<Grid2D> closeNodeIndexes = ReturnNearNodeIndexes(index);
    //    List<Grid2D> result = new List<Grid2D>();

    //    for (int i = 0; i < closeNodeIndexes.Count; i++)
    //    {
    //        bool haveNearBlockNode = HaveNearBlockNode(closeNodeIndexes[i], 1);
    //        if (haveNearBlockNode == true) continue;

    //        result.Add(closeNodeIndexes[i]);
    //    }

    //    return result;
    //}

    void CreateNode()
    {
        for (int i = 0; i < _gridSize.Row; i++)
        {
            for (int j = 0; j < _gridSize.Column; j++)
            {
                Vector2Int localPos = TopLeftLocalPos + new Vector2Int(j, -i);
                Vector2 worldPos = TopLeftWorldPos + new Vector2Int(j, -i);

                TileBase tile = _wallTile.GetTile(new Vector3Int(localPos.x, localPos.y, 0));
                if (tile == null)
                {
                    Debug.Log("Pass " + localPos.x + " " + localPos.y);
                    _nodes[i, j] = new Node(worldPos, new Grid2D(i, j), false);
                }
                else
                {
                    Debug.Log("NonPass " + localPos.x + " " + localPos.y);
                    _nodes[i, j] = new Node(worldPos, new Grid2D(i, j), true);
                }
                // 타일이 없다면 바닥
                // 타일이 존재한다면 벽
            }
        }

        Debug.Log("CreateNode");
    }

    private void OnDrawGizmos()
    {
        if (_points == null) return;

        for (int i = 1; i < _points.Count; i++)
        {
            Gizmos.color = new Color(0, 1, 1, 0.1f);
            Gizmos.DrawLine(_points[i - 1], _points[i]);
        }

        if (_nodes == null) return;

        for (int i = 0; i < _nodes.GetLength(0); i++)
        {
            for (int j = 0; j < _nodes.GetLength(1); j++)
            {
                if (_nodes[i, j].Block)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.1f);
                    Gizmos.DrawCube(_nodes[i, j].WorldPos, Vector3.one);
                }
                else
                {
                    Gizmos.color = new Color(0, 0, 1, 0.1f);
                    Gizmos.DrawCube(_nodes[i, j].WorldPos, Vector3.one);
                }
            }
        }
    }

    private void Start()
    {
        Initialize();
    }

    List<Vector2> _points;

    public void Initialize()
    {
        _points = new List<Vector2>();
        _nodes = new Node[_gridSize.Row, _gridSize.Column];
        CreateNode();

        _pathfinder.Initialize(this);

        // Stopwatch 객체 생성
        Stopwatch stopwatch = new Stopwatch();

        // 타이머 시작
        stopwatch.Start();
        _points = _pathfinder.FindPath(_startPoint.position, _endPoint.position);
        // 타이머 멈춤
        stopwatch.Stop();

        // 경과 시간 출력 (밀리초 단위)
        UnityEngine.Debug.Log("Time taken: " + stopwatch.ElapsedMilliseconds + " ms");
    }
}
