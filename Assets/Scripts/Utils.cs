using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector2Int DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.None: return Vector2Int.zero;
            case Direction.Up: return Vector2Int.up;
            case Direction.Down: return Vector2Int.down;
            case Direction.Left: return Vector2Int.left;
            case Direction.Right: return Vector2Int.right;
        }
        return Vector2Int.zero;
    }
}
