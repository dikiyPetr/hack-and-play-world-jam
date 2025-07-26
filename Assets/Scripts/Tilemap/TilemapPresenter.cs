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
    Dead,
    MovedObject,
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

    public Vector2Int WorldToCell(Vector3 pos)
    {
        return (Vector2Int)levelSetup.backgroud.WorldToCell(pos);
    }

    public async UniTask MoveTile(Vector2Int fromRaw, Vector2Int toRaw, float duration = 0.1f)
    {
        var tempCeil = map.ceils[fromRaw].foreground.FirstOrDefault(e => e.type == EntityType.MovedObject);
        map.ceils[fromRaw].foreground.Remove(tempCeil);
        
        // Получаем тайл и проверяем
        var tilemap = levelSetup.foreground;
        var from = new Vector3Int(fromRaw.x, fromRaw.y, 0);
        var to = new Vector3Int(toRaw.x, toRaw.y, 0);
        TileBase tile = tilemap.GetTile(from);
        if (tile == null)
        {
            Debug.LogWarning("Нет тайла в позиции " + from);
            return;
        }

        // Получаем спрайт тайла
        Tile tileAsset = tile as Tile;
        if (tileAsset == null)
        {
            Debug.LogWarning("Тайл не Tile, а другой тип. Используй Tile со спрайтом.");
            return;
        }

        // Создаём временный объект
        GameObject temp = new GameObject("MovingTile");
        SpriteRenderer sr = temp.AddComponent<SpriteRenderer>();
        sr.sprite = tileAsset.sprite;
        sr.sortingOrder = 10;

        Vector3 worldFrom = tilemap.CellToWorld(from) + tilemap.tileAnchor;
        Vector3 worldTo = tilemap.CellToWorld(to) + tilemap.tileAnchor;
        temp.transform.position = worldFrom;

        // Удаляем тайл из начальной позиции
        tilemap.SetTile(from, null);

        // Плавное движение
        float elapsed = 0f;
        while (elapsed < duration)
        {
            temp.transform.position = Vector3.Lerp(worldFrom, worldTo, elapsed / duration);
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }

        // Завершаем движение
        temp.transform.position = worldTo;

        // Ставим тайл на новое место и удаляем временный объект
        tilemap.SetTile(to, tile);
        Destroy(temp);
        
        map.ceils[toRaw].foreground.Add(tempCeil);
        
        // TODO Добавить проверки что объект может двигаться
    }
    public async UniTask MoveCharacters(Vector2 move)
    {
        if (gameManager.humanController.isMoved || gameManager.demonController.isMoved)
        {
            return;
        }

        var intMove = new Vector2Int((int)move.x, (int)move.y);
        var result = map.Move(intMove);
        await (
            ResolveEffect(result.humanEffect, map.humanPos, gameManager.humanController, intMove),
            ResolveEffect(result.demonEffect, map.demonPos, gameManager.demonController, intMove)
        );
    }

    private async UniTask ResolveEffect(List<Effect> resultHumanEffect, PosHolder posHolder,
        CharacterController character, Vector2Int moveTarget, int recursionCount = 0)
    {
        if (recursionCount > 15)
        {
            gameManager.gameState.StopGame(StopGameType.Recursion);
        }

        foreach (var effect in resultHumanEffect)
        {
            switch (effect)
            {
                case ContactWithDangerousEffect dangerousEffect:
                    ResolveContactWithDangerous(dangerousEffect, character);
                    continue;
                case PushMovedObjectEffect movedObjectEffect:
                    await ResolvePushMovedObject(movedObjectEffect, moveTarget, character);
                    continue;
                case TeleportEffect teleportEffect:
                    ResolveTeleport(teleportEffect, posHolder, character);
                    continue;
                case PushEffect pushEffect:
                {
                    await ResolveMove(pushEffect, posHolder, character);
                    // рекурсия
                    var newEffects = map.MoveByPos(Vector2Int.zero, posHolder);
                    await ResolveEffect(newEffects, posHolder, character, moveTarget, recursionCount + 1);
                    continue;
                }
                case MoveEffect moveEffect:
                    await ResolveMove(moveEffect, posHolder, character);
                    continue;
            }
        }
    }

    void OnMove()
    {
        if (map.demonPos.pos == map.humanPos.pos)
        {
            OnHumanWithDeminMerged();
        }
    }

    void OnHumanWithDeminMerged()
    {
        gameManager.gameState.StopGame(StopGameType.Merge);
    }

    private async UniTask ResolveMove(MoveEffect effect, PosHolder posHolder, CharacterController character)
    {
        posHolder.pos = effect.target;
        var position = GetTilePosition(posHolder.pos);
        OnMove();
        gameManager.audioManager.PlaySteps();
        await character.MoveTo(position);
        gameManager.audioManager.StopSteps();
    }

    private void ResolveTeleport(TeleportEffect effect, PosHolder posHolder, CharacterController character)
    {
        var cell = WorldToCell(character.transform.position);
        if (cell != effect.to)
        {
            posHolder.pos = effect.to;
            var position = GetTilePosition(posHolder.pos);
            gameManager.audioManager.PlayTeleport();
            OnMove();
            character.TeleportTo(position);
        }
    }

    private async UniTask ResolvePushMovedObject(PushMovedObjectEffect effect, Vector2Int moveDirection,
        CharacterController character)
    {
        var position = WorldToCell(character.transform.position);
        await MoveTile(position + moveDirection, position + moveDirection * 2);
    }

    private void ResolveContactWithDangerous(ContactWithDangerousEffect effect, CharacterController character)
    {
        if (effect.DangerousTargetType == dangerousTargetType.All)
        {
            gameManager.gameState.StopGame(StopGameType.Death);
        }
        else if (effect.DangerousTargetType == dangerousTargetType.Human && character.isHuman)
        {
            gameManager.gameState.StopGame(StopGameType.Death);
        }
        else if (effect.DangerousTargetType == dangerousTargetType.Demon && !character.isHuman)
        {
            gameManager.gameState.StopGame(StopGameType.Death);
        }
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
                            Debug.Log(
                                $"teleportFrom: {teleportFrom} intValue: {info.intValue} from: {teleportFrom.from} to:{teleportFrom.to}");
                            return teleportFrom;
                        }
                    }
                    else
                    {
                        teleportFrom.from = position;
                    }
                }
                else
                {
                    var teleport = new TeleportEntity
                        { type = EntityType.Teleport, isInteractable = true, from = position, isFrom = true };
                    teleports.Add(info.intValue, teleport);
                    Debug.Log(
                        $"teleportFrom: {teleport} intValue: {info.intValue} from: {teleport.from} to:{teleport.to}");
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
                            Debug.Log(
                                $"TeleportTo: {teleportTo} intValue: {info.intValue} from: {teleportTo.from} to:{teleportTo.to}");
                            return teleportTo;
                        }
                    }
                    else
                    {
                        teleportTo.to = position;
                    }
                }
                else
                {
                    var teleport = new TeleportEntity
                        { type = EntityType.Teleport, isInteractable = true, to = position, isFrom = false };
                    teleports.Add(info.intValue, teleport);
                    Debug.Log(
                        $"teleportFrom: {teleport} intValue: {info.intValue} from: {teleport.from} to:{teleport.to}");
                    return teleport;
                }

                Debug.Log(
                    $"teleportFrom: {teleportTo} intValue: {info.intValue} from: {teleportTo.from} to:{teleportTo.to}");
                return teleportTo;

            case TileInfoType.Move:
                return new PushEntity(info.direction) { type = EntityType.Push, isInteractable = true };
            case TileInfoType.MovedObject:
                return new MovedObjectEntity() { type = EntityType.MovedObject, isInteractable = true };
            case TileInfoType.Dead:
                return new DangerousEntity
                {
                    type = EntityType.Dangerous,
                    isInteractable = true,
                    dangerousTargetType = Utils.TargetToDangerousTargetType(info.target)
                };
            default:
                throw new ArgumentException($"Unknown TileInfoType: {info.type}");
        }

        return null;
    }
}