using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Target
{
    None,
    All,
    Human,
    Demon,
}

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right,
}

public enum TileInfoType
{
    Ground,
    Wall,
    TeleportFrom,
    TeleportTo,
    Move,
    Dead
}

[Serializable]
public class TileInfo
{
    public Tile tile;
    public TileInfoType type;
    public int intValue;
    public Direction direction;
    public Target target;
}

public class TilemapPresenter : MonoBehaviour
{
    private UnityEngine.Tilemaps.Tilemap _tilemap;
    public Map map { get; private set; }

    [SerializeField] private TileInfo[] _tileInfos;
    [SerializeField] private Vector2Int _humonPos;
    [SerializeField] private Vector2Int _demonPos;

    private void Awake()
    {
        _tilemap = GetComponentInChildren<UnityEngine.Tilemaps.Tilemap>();
        FillMap();
    }

    public Vector3 GetTilePosition(Vector2Int pos)
    {
        return _tilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
    }

    private void FillMap()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        UnityEngine.Tilemaps.TileBase[] allTiles = _tilemap.GetTilesBlock(bounds);
        Dictionary<Vector2Int, Ceil> ceils = new();
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                UnityEngine.Tilemaps.TileBase tile = allTiles[(x - bounds.xMin) + (y - bounds.yMin) * bounds.size.x];


                if (tile != null)
                {
                    TileInfo found = _tileInfos.FirstOrDefault(info => info.tile == tile);
                    var entity = new Entity();
                    // entity.type = found.type;
                    var ceil = new Ceil(position, entity);
                    ceils.Add(position, ceil);
                }
            }
        }

        map = new Map(ceils, _humonPos, _demonPos);
    }
}