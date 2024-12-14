using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
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
        //// ����Ʈ �ʱ�ȭ
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
            _closedList.Add(targetNode); // �ش� �׸��� �߰�����
            AddNearGridInList(targetNode, endNode.Index, size); // �ֺ� �׸��带 ã�Ƽ� �ٽ� �־���
        }

        // �� ���� ��θ� ã�� ���� ��Ȳ��
        return null;
    }

    void AddNearGridInList(Node targetGrid, Grid2D targetNodeIndex, BaseLife.Size size)
    {
        List<Node> nearNodes = targetGrid.NearNodes[size];
        if (nearNodes == null) return;

        for (int i = 0; i < nearNodes.Count; i++)
        {
            Node nearNode = nearNodes[i];
            //bool nowHave = HaveBlockNodeInNearPosition(nearNode.Index, size);
            //if (nowHave) continue;

            // ���⼭ bfs ������ �ֺ� 3X3 ĭ�� �̵� �Ұ����� ��ΰ� �ִٸ� �ٽ� �̾��ش�.
            // ���� ��� ��带 �� ���� ��� ���Ͻ�Ų��.

            if (_closedList.Contains(nearNode)) continue; // ������� ���ϰų� ���� ����Ʈ�� �ִ� ��� ���� �׸��� Ž��

            // �� �κ� �߿�! --> �Ÿ��� �����ؼ� ������Ʈ ���� �ʰ� ��� �����ִ� ������� �����ؾ���
            float moveCost = Vector2.Distance(targetGrid.WorldPos, nearNode.WorldPos);
            moveCost += targetGrid.G;

            bool isOpenListContainNearGrid = _openList.Contain(nearNode);

            // ���� ����Ʈ�� �ִ��� G ���� ����ȴٸ� �ٽ� �������ֱ�
            if (isOpenListContainNearGrid == false || moveCost < nearNode.G)
            {
                // ���⼭ grid �� �Ҵ� �ʿ�
                nearNode.G = moveCost;
                nearNode.H = Vector2.Distance(nearNode.WorldPos, targetGrid.WorldPos);
                nearNode.ParentNode = targetGrid;
            }

            if (isOpenListContainNearGrid == false) _openList.Insert(nearNode);
        }
    }
}