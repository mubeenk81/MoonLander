using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-8f, 8f), 6f);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            nextSpawnTime = Time.time + spawnRate;

            // Increase difficulty
            spawnRate = Mathf.Max(0.5f, spawnRate - 0.01f);
        }
    }
}
