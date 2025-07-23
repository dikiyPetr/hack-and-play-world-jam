using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ActorsManager : MonoBehaviour
{
    [Header("References")] public EnemyScript[] enemies;
    public PlayerScript player;

    [Header("Settings")] public int enemiesToSpawn = 5;
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 10f;

    private PlayerScript playerObject;

    // Хранилище всех врагов по уникальному ID
    private Dictionary<int, EnemyScript> enemyMap = new Dictionary<int, EnemyScript>();

    // ID-счётчик
    private int nextEnemyId = 0;

    void Start()
    {
        // Спавним игрока
        playerObject = Instantiate(player, transform.position, Quaternion.identity, transform);

        // Спавним врагов
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        foreach (var enemyMapValue in enemyMap.Values)
        {
            enemyMapValue.target = playerObject.transform.position;
        }
    }

    private void SpawnEnemy()
    {
        if (enemies.Length == 0 || playerObject == null)
            return;

        EnemyScript enemyPrefab = enemies[Random.Range(0, enemies.Length)];

        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector3 spawnPosition = playerObject.transform.position + (Vector3)(direction * distance);

        EnemyScript newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);

        int enemyId = nextEnemyId++;
        enemyMap.Add(enemyId, newEnemy);

        newEnemy.SetId(enemyId);
    }
}