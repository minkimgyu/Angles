using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;

namespace JPSPlus
{
    // ↖ ↑ ↗  7 0 1
    // ←    →  6   2
    // ↙ ↓ ↘  5 4 3

    //public enum Direction
    //{
    //    NORTH = 0,
    //    NORTH_EAST = 1,
    //    EAST = 2,
    //    SOUTH_EAST = 3,
    //    SOUTH = 4,
    //    SOUTH_WEST = 5,
    //    WEST = 6,
    //    NORTH_WEST = 7,
    //}

    public class FastJPSPlus
    {
        List<int>[] _validDirLookUpTable = new List<int>[8]
        {
            new List<int>{ 2, 1, 0, 7, 6 }, //new List<Direction>{ Direction.EAST,  Direction.NORTH_EAST, Direction.NORTH, Direction.NORTH_WEST, Direction.WEST },

            new List<int>{ 2, 1, 0}, //new List<Direction>{ Direction.EAST,  Direction.NORTH_EAST, Direction.NORTH },

            new List<int>{ 4, 3, 2, 1, 0 }, //new List<Direction>{ Direction.SOUTH, Direction.SOUTH_EAST, Direction.EAST,  Direction.NORTH_EAST, Direction.NORTH },

            new List<int>{ 4, 3, 2 }, //  Direction.SOUTH, Direction.SOUTH_EAST, Direction.EAST

            new List<int>{ 6, 5, 4, 3, 2  }, // Direction.WEST,  Direction.SOUTH_WEST, Direction.SOUTH, Direction.SOUTH_EAST, Direction.EAST

            new List<int>{ 6, 5, 4 }, // Direction.WEST,  Direction.SOUTH_WEST, Direction.SOUTH

            new List<int>{ 0, 7, 6, 5, 4 }, // Direction.NORTH, Direction.NORTH_WEST, Direction.WEST, Direction.SOUTH_WEST, Direction.SOUTH

            new List<int>{ 0, 7, 6 } // Direction.NORTH, Direction.NORTH_WEST, Direction.WEST
        };

        List<int> _allDirections = new List<int>
        { 
            0, //Direction.NORTH, 
            1, //Direction.NORTH_EAST, 
            2, //Direction.EAST, 
            3, //Direction.SOUTH_EAST, 
            4, //Direction.SOUTH, 
            5, //Direction.SOUTH_WEST, 
            6, //Direction.WEST, 
            7, //Direction.NORTH_WEST 
        };

        private List<int> GetAllValidDirections(Node node, bool findFirst = false)
        {
            if(node.ParentNode == null || findFirst == true) return _allDirections;
            else return _validDirLookUpTable[node.DirectionFromParent];
        }

        private bool IsCardinal(int dir)
        {
            switch (dir)
            {
                case 4: // Direction.SOUTH
                case 2: // Direction.EAST
                case 0: // Direction.NORTH
                case 6: // Direction.WEST
                    return true;
            }

            return false;
        }

        private bool IsDiagonal(int dir)
        {
            switch (dir)
            {
                case 3: // Direction.SOUTH_EAST
                case 5: // Direction.SOUTH_WEST
                case 1: // Direction.NORTH_EAST
                case 7: // Direction.NORTH_WEST
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
                case 0: // Direction.NORTH
                    return diffRow < 0 && diffColumn == 0;
                case 1: // Direction.NORTH_EAST
                    return diffRow < 0 && diffColumn > 0 && Mathf.Abs(diffRow) == Mathf.Abs(diffColumn);
                case 2: // Direction.EAST
                    return diffRow == 0 && diffColumn > 0;
                case 3: // Direction.SOUTH_EAST
                    return diffRow > 0 && diffColumn > 0 && Mathf.Abs(diffRow) == Mathf.Abs(diffColumn);
                case 4: // Direction.SOUTH
                    return diffRow > 0 && diffColumn == 0;
                case 5: // Direction.SOUTH_WEST
                    return diffRow > 0 && diffColumn < 0 && Mathf.Abs(diffRow) == Mathf.Abs(diffColumn);
                case 6: // Direction.WEST
                    return diffRow == 0 && diffColumn < 0;
                case 7: // Direction.NORTH_WEST
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
                case 0: // Direction.NORTH
                    return diffRow < 0 && diffColumn == 0;
                case 1: // Direction.NORTH_EAST
                    return diffRow < 0 && diffColumn > 0;
                case 2: // Direction.EAST
                    return diffRow == 0 && diffColumn > 0;
                case 3: // Direction.SOUTH_EAST
                    return diffRow > 0 && diffColumn > 0;
                case 4: // Direction.SOUTH
                    return diffRow > 0 && diffColumn == 0;
                case 5: // Direction.SOUTH_WEST
                    return diffRow > 0 && diffColumn < 0;
                case 6: // Direction.WEST
                    return diffRow == 0 && diffColumn < 0;
                case 7: // Direction.NORTH_WEST
                    return diffRow < 0 && diffColumn < 0;
            }

            return false;
        }

        readonly float sqrt2 = Mathf.Sqrt(2);
        readonly float sqrt2MinusOne = Mathf.Sqrt(2) - 1.0f;

        float GetHeuristic(int curr_row, int curr_column, int goal_row, int goal_column)
        {
            float heuristic;
            int row_dist = Mathf.Abs(goal_row - curr_row);
            int column_dist = Mathf.Abs(goal_column - curr_column); // 휴리스틱 값이 음수가 나오는 문제 발생

            heuristic = Mathf.Max(row_dist, column_dist) + sqrt2MinusOne * Mathf.Min(row_dist, column_dist);

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

                    //finalList.Push(startNode);
                    return ConvertNodeToV2(finalList);
                }

                _openList.DeleteMin(); // 해당 그리드 지워줌
                _closedList.Add(targetNode);

                bool findFirst = false;
                if(_loopCount == 0) findFirst = true; // 시작 노드인 경우 모든 방향 적용해줘야 한다.

                List<int> directions = GetAllValidDirections(targetNode, findFirst);
                for (int i = 0; i < directions.Count; i++)
                {
                    Node successor = null;
                    float givenCost = 0;

                    if (IsCardinal(directions[i]) &&
                     GoalIsInExactDirection(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), directions[i], new Grid2D(endNode.Index.Row, endNode.Index.Column)) &&
                     Grid2D.Diff(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), new Grid2D(endNode.Index.Row, endNode.Index.Column)) <= Mathf.Abs(targetNode.JumpPointDistances[directions[i]]))
                    {
                        successor = endNode;
                        givenCost = targetNode.G + Grid2D.Diff(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), new Grid2D(endNode.Index.Row, endNode.Index.Column));
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

                        givenCost = targetNode.G + sqrt2 * Grid2D.Diff(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), new Grid2D(successor.Index.Row, successor.Index.Column));
                    }
                    else if (targetNode.JumpPointDistances[directions[i]] > 0)
                    {
                        // Jump Point in this direction
                        successor = GetNodeDist(
                           targetNode.Index.Row,
                           targetNode.Index.Column,
                           directions[i],
                           targetNode.JumpPointDistances[directions[i]]);

                        givenCost = Grid2D.Diff(new Grid2D(targetNode.Index.Row, targetNode.Index.Column), new Grid2D(successor.Index.Row, successor.Index.Column));

                        if (IsDiagonal(directions[i])) givenCost = givenCost * sqrt2;
                        givenCost += targetNode.G;
                    }

                    // Traditional A* from this point
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
