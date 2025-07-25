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
    public GameManager gameManager;
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

    public void Move(Vector2 move)
    {
        if (gameManager.humanController.isMoved)
        {
            return;
        }

        var intMove = new Vector2Int((int)move.x, (int)move.y);
        var result = map.Move(intMove);
        ResolveEffect(result.humanEffect, map.humanPos, gameManager.humanController);
    }

    void ResolveEffect(List<Effect> resultHumanEffect, PosHolder posHolder, CharacterController character)
    {
        for (var i = 0; i < resultHumanEffect.Count; i++)
        {
            var effect = resultHumanEffect[i];
            if (effect is MoveEffect)
            {
                ResolveMove((MoveEffect)effect, posHolder, character);
            }
        }
    }

    void ResolveMove(MoveEffect effect, PosHolder posHolder, CharacterController character)
    {
        posHolder.pos = effect.target;
        var position = GetTilePosition(posHolder.pos);
        character.MoveTo(position);
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

                    var entity = found != null ? MapEntry(found) : null;

                    if (ceils.ContainsKey(position))
                    {
                        ceils[position].foreground.Add(entity);
                    }
                    else
                    {
                        var ceil = new Ceil(position, entity);
                        ceils.Add(position, ceil);
                    }
                }
            }
        }

        map = new Map(ceils, _humonPos, _demonPos);
    }

    private Entity MapEntry(TileInfo info)
    {
        switch (info.type)
        {
            case TileInfoType.Ground:
                return new Entity { type = EntityType.Groud, isInteractable = true };
            case TileInfoType.Wall:
                return new Entity { type = EntityType.Obstacle, isInteractable = true };
            case TileInfoType.TeleportFrom:
                return new InteractiveEntity
                {
                    type = EntityType.Interactive,
                    isInteractable = true,
                    interactionType = InteractionType.Portal,
                    value = info.intValue
                };
            case TileInfoType.TeleportTo:
                return new InteractiveEntity
                {
                    type = EntityType.Interactive,
                    isInteractable = false,
                    interactionType = InteractionType.Portal,
                    value = info.intValue
                };
            case TileInfoType.Move:
                return new Entity { type = EntityType.Groud, isInteractable = true };
            case TileInfoType.Dead:
                return new DangerousEntity
                {
                    type = EntityType.Dangerous,
                    isInteractable = true,
                    dangerousType = DangerousType.Lava
                };
            default:
                throw new ArgumentException($"Unknown TileInfoType: {info.type}");
        }
    }
}