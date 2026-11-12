using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    public GameObject minionPrefab;
    public Transform enemyBase; 
    public Transform spawnPoint;
    public float spawnInterval = 5f;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnMinion();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnMinion()
    {
        GameObject newMinion = Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);
        MinionAI ai = newMinion.GetComponent<MinionAI>();
        if (ai != null)
        {
            ai.targetBase = enemyBase;
        }
    }
}
