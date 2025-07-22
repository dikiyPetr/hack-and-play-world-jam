using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Math = System.Math;

namespace Tilemap
{
    public class TilemapGenerator : MonoBehaviour
    {
        private UnityEngine.Tilemaps.Tilemap _tilemap;
        [SerializeField] private TileBase[] tileBases;
        private System.Random _random;
        [SerializeField] private int seed;
        private readonly Dictionary<Vector3Int, bool> _existingTiles = new();

        [SerializeField] private int radius;
        [SerializeField] private Vector3Int point;
        private Vector3Int _oldPoint;

        private void Awake()
        {
            _tilemap = GetComponentInChildren<UnityEngine.Tilemaps.Tilemap>();
        }

        private void Start()
        {
            Generate();
        }

        private void Update()
        {
            if (_oldPoint.Equals(point))
            {
                return;
            }

            Generate();
            _oldPoint = point;
        }

        private int GetPositionSeed(Vector3Int pos)
        {
            return seed + pos.x * 73856093 + pos.y * 19349663 + pos.z * 83492791;
        }

        private bool IsWithinRadius(Vector3Int tilePos, Vector3Int center)
        {
            return Math.Pow(tilePos.x - center.x, 2) + Math.Pow(tilePos.y - center.y, 2) <= Math.Pow(radius, 2);
        }

        private void Generate()
        {
            var allTilePositions = new HashSet<Vector3Int>();

            foreach (var pos in _tilemap.cellBounds.allPositionsWithin)
            {
                if (_tilemap.HasTile(pos))
                {
                    allTilePositions.Add(pos);
                }
            }

            foreach (var tilePos in allTilePositions.Where(tilePos => !IsWithinRadius(tilePos, point)))
            {
                _tilemap.SetTile(tilePos, null);
                _existingTiles.Remove(tilePos);
            }

            for (var i = -radius; i < radius; i++)
            {
                for (var j = -radius; j < radius; j++)
                {
                    var newPos = point + new Vector3Int(i, j, 0);
                    if (!IsWithinRadius(newPos, point) || _existingTiles.ContainsKey(newPos)) continue;
                    _random = new System.Random(GetPositionSeed(newPos));
                    _tilemap.SetTile(newPos, tileBases[_random.Next(0, tileBases.Length)]);
                    _existingTiles[newPos] = true;
                }
            }
        }
    }
}