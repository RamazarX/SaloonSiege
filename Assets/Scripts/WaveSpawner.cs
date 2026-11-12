using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject minionPrefab;
    public Transform spawnPoint;
    public Transform targetBaseForMinions;
    public int minionsPerWave = 3;
    public float spawnDelay = 1f;
    public float waveInterval = 15f;

    void Start()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        while (true)
        {
            for (int i = 0; i < minionsPerWave; i++)
            {
                if (minionPrefab != null && spawnPoint != null)
                {
                    GameObject minion = Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);
                    MinionAI ai = minion.GetComponent<MinionAI>();
                    if (ai != null && targetBaseForMinions != null)
                    {
                        ai.targetBase = targetBaseForMinions;
                    }
                }
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(waveInterval);
        }
    }
}