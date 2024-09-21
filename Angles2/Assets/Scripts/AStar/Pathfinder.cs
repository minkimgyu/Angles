using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Func<Grid2D, BaseEnemy.Size, bool> HaveBlockNodeInNearPosition; // 주변에 이동 불가능한 노드가 존재하는지 검사한다.

    Func<Grid2D, List<Grid2D>> ReturnNearNodeIndexes; // --> 주변 노드를 반환한다.
    Func<Vector2, Grid2D> ReturnNodeIndex;

    Func<Grid2D, Node> ReturnNode;
    const int maxSize = 1000;

    Heap<Node> _openList = new Heap<Node>(maxSize);
    HashSet<Node> _closedList = new HashSet<Node>();

    public void Initialize(GridComponent gridComponent)
    {
        HaveBlockNodeInNearPosition = gridComponent.HaveBlockNodeInNearPosition;

        ReturnNearNodeIndexes = gridComponent.ReturnNearNodeIndexes;
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
        //// 리스트 초기화
        _openList.Clear();
        _closedList.Clear();

        Grid2D startIndex = ReturnNodeIndex(startPos);
        Grid2D endIndex = ReturnNodeIndex(targetPos);

        Node startNode = ReturnNode(startIndex);
        Node endNode = ReturnNode(endIndex);

        if (startNode == null || endNode == null) return null;

        _openList.Insert(startNode);

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
            _closedList.Add(targetNode); // 해당 그리드 추가해줌
            AddNearGridInList(targetNode, endNode.Index, size); // 주변 그리드를 찾아서 다시 넣어줌
        }

        // 이 경우는 경로를 찾지 못한 상황임
        return null;
    }

    void AddNearGridInList(Node targetGrid, Grid2D targetNodeIndex, BaseEnemy.Size size)
    {
        List<Grid2D> nearGridIndexes = ReturnNearNodeIndexes(targetGrid.Index);
        if (nearGridIndexes == null) return;

        for (int i = 0; i < nearGridIndexes.Count; i++)
        {
            Node nearNode = ReturnNode(nearGridIndexes[i]);

            bool nowHave = HaveBlockNodeInNearPosition(nearNode.Index, size);
            if (nowHave) continue;
            // 여기서 bfs 돌려서 주변 3X3 칸에 이동 불가능한 경로가 있다면 다시 뽑아준다.
            // 만약 모든 노드를 다 뽑은 경우 리턴시킨다.

            if (nearNode.Block == true || _closedList.Contains(nearNode)) continue; // 통과하지 못하거나 닫힌 리스트에 있는 경우 다음 그리드 탐색

            // 이 부분 중요! --> 거리를 측정해서 업데이트 하지 않고 계속 더해주는 방식으로 진행해야함
            float moveCost = Vector2.Distance(targetGrid.WorldPos, nearNode.WorldPos);
            moveCost += targetGrid.G;

            bool isOpenListContainNearGrid = _openList.Contain(nearNode);

            // 오픈 리스트에 있더라도 G 값이 변경된다면 다시 리셋해주기
            if (isOpenListContainNearGrid == false || moveCost < nearNode.G)
            {
                // 여기서 grid 값 할당 필요
                nearNode.G = moveCost;
                nearNode.H = Vector2.Distance(nearNode.WorldPos, targetGrid.WorldPos);
                nearNode.ParentNode = targetGrid;
            }

            if (isOpenListContainNearGrid == false) _openList.Insert(nearNode);
        }
    }
}