// #define Draw_Progress // 활성화

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinder : MonoBehaviour, IPathfinder
{
    Func<Vector2, Grid2D> ReturnNodeIndex;
    Func<Grid2D, Node> ReturnNode;

    const int maxSize = 1000;

    Heap<Node> _openList = new Heap<Node>(maxSize);
    HashSet<Node> _closedList = new HashSet<Node>();

    public void Initialize(GridComponent gridComponent)
    {
        ReturnNodeIndex = gridComponent.ReturnNodeIndex;
        ReturnNode = gridComponent.ReturnNode;
    }

    List<Vector2> ConvertNodeToV2(Stack<Node> stackNode)
    {
        List<Vector2> points = new List<Vector2>();
        while (stackNode.Count > 0)
        {
            Node node = stackNode.Peek();
            points.Add(node.WorldPos);
            stackNode.Pop();
        }

        return points;
    }

    // 가장 먼저 반올림을 통해 가장 가까운 노드를 찾는다.
    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos, BaseEnemy.Size size)
    {
#if Draw_Progress
            _openListPoints.Clear();
            _closeListPoints.Clear();
#endif

        //// 리스트 초기화
        _openList.Clear();
        _closedList.Clear();

        Grid2D startIndex = ReturnNodeIndex(startPos);
        Grid2D endIndex = ReturnNodeIndex(targetPos);

        Node startNode = ReturnNode(startIndex);
        Node endNode = ReturnNode(endIndex);

        if (startNode == null || endNode == null) return null;

        _openList.Insert(startNode);

#if Draw_Progress
            _openListPoints.Add(startNode.WorldPos);
#endif

        while (_openList.Count > 0)
        {
            // 시작의 경우 제외해줘야함
            Node targetNode = _openList.ReturnMin();

            if (targetNode == endNode) // 목적지와 타겟이 같으면 끝
            {
                Stack<Node> finalList = new Stack<Node>();

                Node TargetCurNode = targetNode;
                while (TargetCurNode != startNode)
                {
                    finalList.Push(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }

                return ConvertNodeToV2(finalList);
            }

            _openList.DeleteMin(); // 해당 그리드 지워줌
#if Draw_Progress
            _closeListPoints.Add(targetNode.WorldPos);
#endif

            _closedList.Add(targetNode); // 해당 그리드 추가해줌
            AddNearGridInList(targetNode, endNode, size); // 주변 그리드를 찾아서 다시 넣어줌
        }

        // 이 경우는 경로를 찾지 못한 상황임
        return null;
    }

    const float sqrt = 1.414f;

    // 옥타일 거리 휴리스틱
    float GetDistance(Vector2 start, Vector2 end)
    {
        float diffX = Mathf.Abs(start.x - end.x);
        float diffY = Mathf.Abs(start.y - end.y);
        return Mathf.Max(diffX, diffY) + (sqrt - 1) * Mathf.Min(diffX, diffY);
    }

    void AddNearGridInList(Node targetNode, Node endNode, BaseLife.Size size)
    {
        List<Node> nearNodes = targetNode.NearNodes[size];
        if (nearNodes == null) return;

        for (int i = 0; i < nearNodes.Count; i++)
        {
            Node nearNode = nearNodes[i];

            // 여기서 bfs 돌려서 주변 3X3 칸에 이동 불가능한 경로가 있다면 다시 뽑아준다.
            // 만약 모든 노드를 다 뽑은 경우 리턴시킨다.

            if (nearNode.Block == true || _closedList.Contains(nearNode)) continue; // 통과하지 못하거나 닫힌 리스트에 있는 경우 다음 그리드 탐색

            // 이 부분 중요! --> 거리를 측정해서 업데이트 하지 않고 계속 더해주는 방식으로 진행해야함
            float moveCost = targetNode.G + GetDistance(targetNode.WorldPos, nearNode.WorldPos);
            bool isOpenListContainNearGrid = _openList.Contain(nearNode);

            // 오픈 리스트에 있더라도 G 값이 변경된다면 다시 리셋해주기
            if (isOpenListContainNearGrid == false || moveCost < nearNode.G)
            {
                // 여기서 grid 값 할당 필요
                nearNode.G = moveCost;
                nearNode.H = GetDistance(nearNode.WorldPos, endNode.WorldPos);
                nearNode.ParentNode = targetNode;
            }

            if (isOpenListContainNearGrid == false)
            {
#if Draw_Progress
                _openListPoints.Add(nearNode.WorldPos);
#endif
                _openList.Insert(nearNode);
            }
        }
    }

#if Draw_Progress

        List<Vector2> _closeListPoints = new List<Vector2>();
        List<Vector2> _openListPoints = new List<Vector2>();

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _openListPoints.Count; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(_openListPoints[i], new Vector2(0.8f, 0.8f));
            }

            for (int i = 0; i < _closeListPoints.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(_closeListPoints[i], new Vector2(0.8f, 0.8f));
            }
        }
#endif
}