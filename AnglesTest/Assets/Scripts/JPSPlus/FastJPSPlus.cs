using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;

namespace JPSPlus
{
    // Direction
    // ↖ ↑ ↗  7 0 1
    // ←    →  6   2
    // ↙ ↓ ↘  5 4 3

    public class FastJPSPlus
    {
        List<int>[] _validDirLookUpTable = new List<int>[8]
        {
            new List<int>{ 2, 1, 0, 7, 6 }, //new List<Direction>{ EAST,  NORTH_EAST, NORTH, NORTH_WEST, WEST },

            new List<int>{ 2, 1, 0}, //new List<Direction>{ EAST,  NORTH_EAST, NORTH },

            new List<int>{ 4, 3, 2, 1, 0 }, //new List<Direction>{ SOUTH, SOUTH_EAST, EAST,  NORTH_EAST, NORTH },

            new List<int>{ 4, 3, 2 }, //  SOUTH, SOUTH_EAST, EAST

            new List<int>{ 6, 5, 4, 3, 2  }, // WEST,  SOUTH_WEST, SOUTH, SOUTH_EAST, EAST

            new List<int>{ 6, 5, 4 }, // WEST,  SOUTH_WEST, SOUTH

            new List<int>{ 0, 7, 6, 5, 4 }, // NORTH, NORTH_WEST, WEST, SOUTH_WEST, SOUTH

            new List<int>{ 0, 7, 6 } // NORTH, NORTH_WEST, WEST
        };

        List<int> _allDirections = new List<int>
        { 
            0, // NORTH, 
            1, // NORTH_EAST, 
            2, // EAST, 
            3, // SOUTH_EAST, 
            4, // SOUTH, 
            5, // SOUTH_WEST, 
            6, // WEST, 
            7, // NORTH_WEST 
        };

        public int GetMaxDiffBetweenGrid(Grid2D a, Grid2D b)
        {
            int diffColumns = Mathf.Abs(b.Column - a.Column);
            int diffRows = Mathf.Abs(b.Row - a.Row);

            return Mathf.Max(diffRows, diffColumns);
        }

        private List<int> GetAllValidDirections(Node node, bool findFirst = false)
        {
            if(node.ParentNode == null || findFirst == true) return _allDirections;
            else return _validDirLookUpTable[node.DirectionFromParent];
        }

        private bool IsCardinal(int dir)
        {
            switch (dir)
            {
                case 4: // SOUTH
                case 2: // EAST
                case 0: // NORTH
                case 6: // WEST
                    return true;
            }

            return false;
        }

        private bool IsDiagonal(int dir)
        {
            switch (dir)
            {
                case 3: // SOUTH_EAST
                case 5: // SOUTH_WEST
                case 1: // NORTH_EAST
                case 7: // NORTH_WEST
                    return true;
            }

            return false;
        }

        private bool GoalIsInExactDirection(Grid2D current, int dir, Grid2D goal)
        {
            int diffColumn = goal.Column - current.Column;
            int diffRow = goal.Row - current.Row;

            switch (dir)
            {
                case 0: // NORTH
                    return diffRow < 0 && diffColumn == 0;
                case 1: // NORTH_EAST
                    return diffRow < 0 && diffColumn > 0 && Mathf.Abs(diffRow) == Mathf.Abs(diffColumn);
                case 2: // EAST
                    return diffRow == 0 && diffColumn > 0;
                case 3: // SOUTH_EAST
                    return diffRow > 0 && diffColumn > 0 && Mathf.Abs(diffRow) == Mathf.Abs(diffColumn);
                case 4: // SOUTH
                    return diffRow > 0 && diffColumn == 0;
                case 5: // SOUTH_WEST
                    return diffRow > 0 && diffColumn < 0 && Mathf.Abs(diffRow) == Mathf.Abs(diffColumn);
                case 6: // WEST
                    return diffRow == 0 && diffColumn < 0;
                case 7: // NORTH_WEST
                    return diffRow < 0 && diffColumn < 0 && Mathf.Abs(diffRow) == Mathf.Abs(diffColumn);
            }

            return false;
        }

        private bool GoalIsInGeneralDirection(Grid2D current, int dir, Grid2D goal)
        {
            int diffColumn = goal.Column - current.Column;
            int diffRow = goal.Row - current.Row;

            switch (dir)
            {
                case 0: // NORTH
                    return diffRow < 0 && diffColumn == 0;
                case 1: // NORTH_EAST
                    return diffRow < 0 && diffColumn > 0;
                case 2: // EAST
                    return diffRow == 0 && diffColumn > 0;
                case 3: // SOUTH_EAST
                    return diffRow > 0 && diffColumn > 0;
                case 4: // SOUTH
                    return diffRow > 0 && diffColumn == 0;
                case 5: // SOUTH_WEST
                    return diffRow > 0 && diffColumn < 0;
                case 6: // WEST
                    return diffRow == 0 && diffColumn < 0;
                case 7: // NORTH_WEST
                    return diffRow < 0 && diffColumn < 0;
            }

            return false;
        }

        const float _sqrt2 = 1.414f;

        // 옥타일 휴리스틱 사용
        float GetHeuristic(int CurrentRow, int CurrentColumn, int GoalRow, int GoalColumn)
        {
            float heuristic;
            int row_dist = Mathf.Abs(GoalRow - CurrentRow);
            int column_dist = Mathf.Abs(GoalColumn - CurrentColumn); // 휴리스틱 값이 음수가 나오는 문제 발생

            heuristic = Mathf.Max(row_dist, column_dist) + (_sqrt2 - 1) * Mathf.Min(row_dist, column_dist);

            return heuristic;
        }

         
        Func<Vector2, Grid2D> ReturnNodeIndex;
        Func<Grid2D, Node> ReturnNode;
        Func<int, int, int, int, Node> GetNodeDist;

        const int maxSize = 1000;

        Heap<Node> _openList = new Heap<Node>(maxSize);
        HashSet<Node> _closedList = new HashSet<Node>(maxSize);

        BaseLife.Size _size;

        public void Initialize(GridComponent gridComponent)
        {
            ReturnNodeIndex = gridComponent.ReturnNodeIndex;
            ReturnNode = gridComponent.ReturnNode;
            GetNodeDist = gridComponent.GetNodeDist;
            _size = gridComponent.Size;
        }

        List<Vector2> ConvertNodeToV2(Stack<Node> stackNode)
        {
            Node beforeNode = null;

            List<Vector2> points = new List<Vector2>();
            while (stackNode.Count > 0)
            {
                Node node = stackNode.Pop();
                points.Add(node.WorldPos);
                beforeNode = node;
            }

            return points;
        }

        int _loopCount = 0;

        // 가장 먼저 반올림을 통해 가장 가까운 노드를 찾는다.
        public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
        {
            // 리스트 초기화
            _openList.Clear();
            _closedList.Clear();
            _loopCount = 0;

            Grid2D startIndex = ReturnNodeIndex(startPos);
            Grid2D endIndex = ReturnNodeIndex(targetPos);

            Node startNode = ReturnNode(startIndex);
            Node endNode = ReturnNode(endIndex);

            if (startNode == null || endNode == null)
            {
                return null;
            }
            if (startNode.IsBlock(_size) == true ||
                endNode.IsBlock(_size) == true)
            {
                return null;
            }

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
                _closedList.Add(targetNode);

                bool findFirst = false;
                if(_loopCount == 0) findFirst = true; // 시작 노드인 경우 모든 방향 적용해줘야 한다. -> 중요함!

                List<int> directions = GetAllValidDirections(targetNode, findFirst);
                for (int i = 0; i < directions.Count; i++)
                {
                    Node successor = null;
                    float givenCost = 0;

                    if (IsCardinal(directions[i]) &&
                     GoalIsInExactDirection(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), directions[i], new Grid2D(endNode.Index.Row, endNode.Index.Column)) &&
                     GetMaxDiffBetweenGrid(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), new Grid2D(endNode.Index.Row, endNode.Index.Column)) <= Mathf.Abs(targetNode.JumpPointDistances[directions[i]]))
                    {
                        successor = endNode;
                        givenCost = targetNode.G + GetMaxDiffBetweenGrid(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), new Grid2D(endNode.Index.Row, endNode.Index.Column));
                    }
                    else if(IsDiagonal(directions[i]) &&
                        GoalIsInGeneralDirection(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), directions[i], new Grid2D(endNode.Index.Row, endNode.Index.Column)) &&
                        (Mathf.Abs(endNode.Index.Column - targetNode.Index.Column) <= Mathf.Abs(targetNode.JumpPointDistances[directions[i]]) ||
                         Mathf.Abs(endNode.Index.Row - targetNode.Index.Row) <= Mathf.Abs(targetNode.JumpPointDistances[directions[i]])))
                    {
                        int minDiff = Mathf.Min(Mathf.Abs(endNode.Index.Column - targetNode.Index.Column), Mathf.Abs(endNode.Index.Row - targetNode.Index.Row));

                        successor = GetNodeDist(
                            targetNode.Index.Row,
                            targetNode.Index.Column,
                            directions[i],
                            minDiff);

                        givenCost = targetNode.G + _sqrt2 * GetMaxDiffBetweenGrid(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), new Grid2D(successor.Index.Row, successor.Index.Column));
                    }
                    else if (targetNode.JumpPointDistances[directions[i]] > 0)
                    {
                        // 이 방향에 점프 포인트가 있는 경우
                        successor = GetNodeDist(
                           targetNode.Index.Row,
                           targetNode.Index.Column,
                           directions[i],
                           targetNode.JumpPointDistances[directions[i]]);

                        givenCost = GetMaxDiffBetweenGrid(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), new Grid2D(successor.Index.Row, successor.Index.Column));

                        if (IsDiagonal(directions[i])) givenCost = givenCost * _sqrt2;
                        givenCost += targetNode.G;
                    }

                    // A* 알고리즘과 유사한 부분
                    if (successor != null)
                    {
                        if (_closedList.Contains(successor) == true) continue;

                        if (_openList.Contains(successor) == false)
                        {
                            successor.ParentNode = targetNode;
                            successor.G = givenCost;
                            successor.DirectionFromParent = directions[i];
                            successor.H = GetHeuristic(successor.Index.Row, successor.Index.Column, endNode.Index.Row, endNode.Index.Column);

                            _openList.Insert(successor);
                        }
                        else if (givenCost < successor.G)
                        {
                            successor.ParentNode = targetNode;
                            successor.G = givenCost;
                            successor.DirectionFromParent = directions[i];
                            successor.H = GetHeuristic(successor.Index.Row, successor.Index.Column, endNode.Index.Row, endNode.Index.Column);

                            _openList.Insert(successor);
                        }
                    }
                }

                _loopCount++; // 루프 카운트 증가
            }

            // 이 경우는 경로를 찾지 못한 상황임
            return null;
        }
    }
}
