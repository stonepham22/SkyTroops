using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 2f; // Time interval between spawns
    [SerializeField] private float _spawnTimer; // Timer for spawn interval
    [SerializeField] private float _SpawnPointPositionY = 6f;
    [SerializeField] private EnemyPoolManager _enemyPoolManager;

    void Start()
    {
        SpawnEnemy(); // Spawn an enemy at the start
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            SpawnEnemy();
            _spawnTimer -= _spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = GetPrefabFromPool();
        Vector3 spawnPosition = GetSpawnPosition();
        Object.Instantiate(enemyPrefab,spawnPosition,Quaternion.identity);
    }

    private GameObject GetPrefabFromPool()
    {
        return _enemyPoolManager.Get(); 
    }

    private Vector3 GetSpawnPosition()
    {    
        return new Vector3(Random.Range(-1.8f, 1.8f),_SpawnPointPositionY,0);
    }
}
