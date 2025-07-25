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
    public LevelSetup levelSetup;
    [SerializeField] private TileInfo[] _tileInfos;
    private Dictionary<int, TeleportEntity> teleports = new Dictionary<int, TeleportEntity>();

    private void Awake()
    {
        var ceils = new Dictionary<Vector2Int, Ceil>();
        map = new Map(ceils, levelSetup._humonPos, levelSetup._demonPos);
        FillMap(levelSetup.backgroud, false);
        FillMap(levelSetup.foreground, true);
    }

    private void Start()
    {
        gameManager.humanController.TeleportTo(GetTilePosition(levelSetup._humonPos));
        gameManager.demonController.TeleportTo(GetTilePosition(levelSetup._demonPos));
    }

    public Vector3 GetTilePosition(Vector2Int pos)
    {
        return levelSetup.backgroud.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
    }

    public async UniTask Move(Vector2 move)
    {
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
            if (effect is TeleportEffect)
            {
                ResolveTeleport((TeleportEffect)effect, posHolder, character);
                continue;
            }

            if (effect is PushEffect)
            {
                await ResolveMove((MoveEffect)effect, posHolder, character);
                // рекурсия
                var newEffects = map.MoveByPos(Vector2Int.zero, posHolder);
                await ResolveEffect(newEffects, posHolder, character);
                continue;
            }

            if (effect is MoveEffect)
            {
                await ResolveMove((MoveEffect)effect, posHolder, character);
                continue;
            }
        }
    }

    private async UniTask ResolveMove(MoveEffect effect, PosHolder posHolder, CharacterController character)
    {
        posHolder.pos = effect.target;
        var position = GetTilePosition(posHolder.pos);
        await character.MoveTo(position);
    }

    private void ResolveTeleport(TeleportEffect effect, PosHolder posHolder, CharacterController character)
    {
        posHolder.pos = effect.to;
        var position = GetTilePosition(posHolder.pos);
        character.TeleportTo(position);
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

                    var entity = found != null ? MapEntry(found, position) : null;

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

    private Entity MapEntry(TileInfo info, Vector2Int position)
    {
        switch (info.type)
        {
            case TileInfoType.Ground:
                return new Entity { type = EntityType.Groud, isInteractable = true };
            case TileInfoType.Wall:
                return new Entity { type = EntityType.Obstacle, isInteractable = true };
            case TileInfoType.TeleportFrom:
                if (teleports.TryGetValue(info.intValue, out var teleportFrom))
                {
                    if (teleportFrom.from.HasValue)
                    {
                        if (teleportFrom.to.HasValue)
                        {
                            Debug.Log($"teleportFrom: {teleportFrom} intValue: {info.intValue} from: {teleportFrom.from} to:{teleportFrom.to}");
                            return teleportFrom;
                        }
                        Debug.LogError($"ТЫ ЧЕ ПИДОР, ИДИ НАСТРОЙ ТЕЛЕПОРТЫ, ЛОХ! intValue: {info.intValue}");
                    }
                    else
                    {
                        teleportFrom.from = position;
                    }
                }
                else
                {
                    var teleport = new TeleportEntity
                        { type = EntityType.Teleport, isInteractable = true, from = position };
                    teleports.Add(info.intValue, teleport);
                    Debug.Log($"teleportFrom: {teleport} intValue: {info.intValue} from: {teleport.from} to:{teleport.to}");
                    return teleport;
                }

                return teleportFrom;
            case TileInfoType.TeleportTo:
                if (teleports.TryGetValue(info.intValue, out var teleportTo))
                {
                    if (teleportTo.to.HasValue)
                    {
                        if (teleportTo.from.HasValue)
                        {
                            Debug.Log($"teleportFrom: {teleportTo} intValue: {info.intValue} from: {teleportTo.from} to:{teleportTo.to}");
                            return teleportTo;
                        }
                        Debug.LogError($"ТЫ ЧЕ ПИДОР, ИДИ НАСТРОЙ ТЕЛЕПОРТЫ, ЛОХ! intValue: {info.intValue}");
                    }
                    else
                    {
                        teleportTo.to = position;
                    }
                }
                else
                {
                    var teleport = new TeleportEntity
                        { type = EntityType.Teleport, isInteractable = true, to = position };
                    teleports.Add(info.intValue,teleport);
                    Debug.Log($"teleportFrom: {teleport} intValue: {info.intValue} from: {teleport.from} to:{teleport.to}");
                    return teleport;
                }
                Debug.Log($"teleportFrom: {teleportTo} intValue: {info.intValue} from: {teleportTo.from} to:{teleportTo.to}");
                return teleportTo;

            case TileInfoType.Move:
                return new PushEntity(info.direction) { type = EntityType.Push, isInteractable = true };
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

        return null;
    }
}