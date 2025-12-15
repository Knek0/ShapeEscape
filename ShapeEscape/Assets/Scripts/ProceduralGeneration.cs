using UnityEngine;
using System.Collections.Generic;

public class ProceduralGeneration : MonoBehaviour
{
    // Difficulty Increase Parameters
    public float difficulty = 2f;
    public float difficultyIncreaseRate = 0.05f; // per second
    public float maxDifficulty = 8f;
    public float SurvivalTime { get; private set; }

    // Reference to the player transform
    public Transform player;

    // Prefabs to spawn
    public GameObject wallPrefab; 
    public GameObject enemyPrefab;

    // Spawning parameters
    public float spawnDistanceAhead = 12f;
    public float spawnWidth = 10f;          
    public float moveThreshold = 1f;     
    public int itemsPerSpawn = 3;
    private float enemyTimer = 3f;

    // Culling parameters
    public float cullDistance = 25f;

    // Spawn position tracking
    private Vector3 lastSpawnPos;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Initialize spawn position
        lastSpawnPos = player.position;

        // Reset difficulty
        difficulty = 2f;
    }

    void Update()
    {
        // Stop if player is dead
        if (PlayerDie.gameOver) return;

        // difficulty increase each second
        SurvivalTime += Time.deltaTime;
        if (difficulty < maxDifficulty)
        {
            difficulty += difficultyIncreaseRate * Time.deltaTime;
        }

        Vector2 movementDir = GetMovementDirection();

        // Spawn enemies every 3 seconds
        enemyTimer += Time.deltaTime;

        if (enemyTimer >= 3)
        {
            SpawnEnemies();
            enemyTimer = 0f;
        }

        // Only generate when player moves
        if (movementDir.sqrMagnitude > 0.1f &&
            Vector3.Distance(player.position, lastSpawnPos) >= moveThreshold)
        {
            SpawnAhead(movementDir.normalized);
            lastSpawnPos = player.position;
        }

        CullObjects();
    }

    private Vector2 GetMovementDirection()
    {
        // Direction from last spawn position to current player position
        return (player.position - lastSpawnPos);
    }

    private void SpawnAhead(Vector2 direction)
    {
        // Spawn multiple items ahead of the player
        for (int i = 0; i < itemsPerSpawn; i++)
        {
            // Pick wall or enemy
            GameObject prefab = (Random.value > 0.05f) ? wallPrefab : enemyPrefab;

            // Calculate spawn position
            Vector2 spawnPos = (Vector2)player.position + direction * spawnDistanceAhead;

            // Random offset perpendicular to movement direction
            Vector2 perp = new Vector2(-direction.y, direction.x);
            spawnPos += perp * Random.Range(-spawnWidth / 2f, spawnWidth / 2f);

            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedObjects.Add(obj);
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < difficulty; i++)
        {
            // Spawn idle enemies
            Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnDistanceAhead;
            Vector2 spawnPos = (Vector2)player.position + randomOffset;

            GameObject obj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            spawnedObjects.Add(obj);
        }
    }

    private void CullObjects()
    {
        // Remove objects that are too far from the player
        Vector2 p = player.position;

        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            // Check if object is destroyed
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            // Cull based on distance
            if (Vector2.Distance(p, spawnedObjects[i].transform.position) > cullDistance)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}

