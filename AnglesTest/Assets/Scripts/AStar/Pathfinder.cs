#define Draw_Progress // Ȱ��ȭ

using System;
using System.Collections;
using System.Collections.Generic;
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

    // ���� ���� �ݿø��� ���� ���� ����� ��带 ã�´�.
    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos, BaseEnemy.Size size)
    {
#if Draw_Progress
            _openListPoints.Clear();
            _closeListPoints.Clear();
#endif

        //// ����Ʈ �ʱ�ȭ
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
            // ������ ��� �����������
            Node targetNode = _openList.ReturnMin();

            if (targetNode == endNode) // �������� Ÿ���� ������ ��
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

            _openList.DeleteMin(); // �ش� �׸��� ������
#if Draw_Progress
                _closeListPoints.Add(targetNode.WorldPos);
#endif

            _closedList.Add(targetNode); // �ش� �׸��� �߰�����
            AddNearGridInList(targetNode, endNode, size); // �ֺ� �׸��带 ã�Ƽ� �ٽ� �־���
        }

        // �� ���� ��θ� ã�� ���� ��Ȳ��
        return null;
    }

    void AddNearGridInList(Node targetNode, Node endNode, BaseLife.Size size)
    {
        List<Node> nearNodes = targetNode.NearNodes[size];
        if (nearNodes == null) return;

        for (int i = 0; i < nearNodes.Count; i++)
        {
            Node nearNode = nearNodes[i];

            // ���⼭ bfs ������ �ֺ� 3X3 ĭ�� �̵� �Ұ����� ��ΰ� �ִٸ� �ٽ� �̾��ش�.
            // ���� ��� ��带 �� ���� ��� ���Ͻ�Ų��.

            if (_closedList.Contains(nearNode)) continue; // ������� ���ϰų� ���� ����Ʈ�� �ִ� ��� ���� �׸��� Ž��

            // �� �κ� �߿�! --> �Ÿ��� �����ؼ� ������Ʈ ���� �ʰ� ��� �����ִ� ������� �����ؾ���
            float moveCost = targetNode.G + Vector2.Distance(targetNode.WorldPos, nearNode.WorldPos);
            bool isOpenListContainNearGrid = _openList.Contain(nearNode);

            // ���� ����Ʈ�� �ִ��� G ���� ����ȴٸ� �ٽ� �������ֱ�
            if (isOpenListContainNearGrid == false || moveCost < nearNode.G)
            {
                // ���⼭ grid �� �Ҵ� �ʿ�
                nearNode.G = moveCost;
                nearNode.H = Vector2.Distance(nearNode.WorldPos, endNode.WorldPos);
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