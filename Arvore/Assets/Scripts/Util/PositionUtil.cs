using System;
using UnityEngine;

namespace Snake
{
    static class PositionUtil
    {
        public static int Dist(Position p1,Position p2)
        {
            int result = 0;
            result = (Mathf.Abs(p1.x - p2.x)) + (Mathf.Abs(p1.y - p2.y));

            return result;
        }
    }
}
