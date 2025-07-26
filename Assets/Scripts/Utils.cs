using System.Collections.Generic;
using Data;
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
    }    public static dangerousTargetType TargetToDangerousTargetType(Target target)
    {
        switch (target)
        {
            case Target.All: return dangerousTargetType.All;
            case Target.Human: return dangerousTargetType.Human;
            case Target.Demon: return dangerousTargetType.Demon;
        }

        return dangerousTargetType.All;
    }
    public static string FormatTime(float milliseconds)
    {
        int totalMs = Mathf.FloorToInt(milliseconds);
        int minutes = totalMs / 60000;
        int seconds = (totalMs % 60000) / 1000;
        int ms = totalMs % 1000;

        return $"{minutes:00}:{seconds:00}:{ms:00}";
    }
    
}
