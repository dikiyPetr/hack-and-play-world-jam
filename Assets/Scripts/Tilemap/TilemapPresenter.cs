using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
    public Map map { get; private set; }

    public UnityEngine.Tilemaps.Tilemap backgroud;
    public UnityEngine.Tilemaps.Tilemap foreground;
    [SerializeField] private TileInfo[] _tileInfos;
    [SerializeField] private Vector2Int _humonPos;
    [SerializeField] private Vector2Int _demonPos;

    private void Awake()
    {
        var ceils = new Dictionary<Vector2Int, Ceil>();
        map = new Map(ceils, _humonPos, _demonPos);
        FillMap(backgroud, false);
        FillMap(foreground, true);
    }

    public Vector3 GetTilePosition(Vector2Int pos)
    {
        return backgroud.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
    }

    public async UniTask Move(Vector2 move)
    {
        Debug.Log($"Move {move.x}, {move.y}");
        if (gameManager.humanController.isMoved || gameManager.demonController.isMoved)
        {
            return;
        }

        var intMove = new Vector2Int((int)move.x, (int)move.y);
        var result = map.Move(intMove);
        await (
            ResolveEffect(result.humanEffect, map.humanPos, gameManager.humanController),
            ResolveEffect(result.demonEffect, map.demonPos, gameManager.demonController)
        );
    }

    private async UniTask ResolveEffect(List<Effect> resultHumanEffect, PosHolder posHolder,
        CharacterController character)
    {
        for (var i = 0; i < resultHumanEffect.Count; i++)
        {
            var effect = resultHumanEffect[i];
            if (effect is MoveEffect)
            {
                await ResolveMove((MoveEffect)effect, posHolder, character);
            }
        }
    }

    private async UniTask ResolveMove(MoveEffect effect, PosHolder posHolder, CharacterController character)
    {
        posHolder.pos = effect.target;
        var position = GetTilePosition(posHolder.pos);
        await character.MoveTo(position);
    }

    private void FillMap(UnityEngine.Tilemaps.Tilemap tilemap, bool isForeground)
    {
        BoundsInt bounds = tilemap.cellBounds;
        UnityEngine.Tilemaps.TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Dictionary<Vector2Int, Ceil> ceils = map.ceils;
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

                    if (isForeground)
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