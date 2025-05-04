using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JPSPlus
{
    public class JPSPathfinder : MonoBehaviour, IPathfinder
    {
        FastJPSPlus _jPSPlusx1;
        FastJPSPlus _jPSPlusx2;

        public FastJPSPlus JPSPlusx1 { get => _jPSPlusx1; }
        public FastJPSPlus JPSPlusx2 { get => _jPSPlusx2; }

        public void Initialize(GridComponent gridComponent, BaseEnemy.Size size)
        {
            switch (size)
            {
                case BaseLife.Size.Small:
                    _jPSPlusx1 = new FastJPSPlus();
                    _jPSPlusx1.Initialize(gridComponent);
                    break;
                case BaseLife.Size.Medium:
                    _jPSPlusx2 = new FastJPSPlus();
                    _jPSPlusx2.Initialize(gridComponent);
                    break;
            }
        }

        public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos, BaseEnemy.Size size)
        {
            List<Vector2> path = null;

            switch (size)
            {
                case BaseLife.Size.Small:
                    path = _jPSPlusx1.FindPath(startPos, targetPos);
                    break;
                case BaseLife.Size.Medium:
                    path = _jPSPlusx2.FindPath(startPos, targetPos);
                    break;
            }

            return path;
        }
    }
}