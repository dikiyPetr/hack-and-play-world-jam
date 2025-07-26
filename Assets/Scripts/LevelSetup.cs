using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tilemap backgroud;
    public UnityEngine.Tilemaps.Tilemap foreground;
    [SerializeField] public Vector2Int _humonPos;
    [SerializeField] public Vector2Int _demonPos;
    public int collectableCount;
    
}